async function loadCandidates() {
  const res = await fetch(`${API_BASE_URL}/candidates`);
  const candidates = await res.json();

  const list = document.getElementById("candidate-list");

  candidates.forEach(c => {
    const div = document.createElement("div");
    div.className = "card";
    div.innerHTML = `
      <img src="${c.photoUrl}" alt="${c.fullName}">
      <span>${c.fullName}</span> <br> <br>

      <button class="vote-btn" onclick="vote('${c.id}')">Voter</button>
    `;
    list.appendChild(div);
  });
}
async function loadCampaigns() {
  const res = await fetch(`${API_BASE_URL}/campaigns`);
  const candidates = await res.json();

  const list = document.getElementById("campaigns-list");

  candidates.forEach(c => {
    const div = document.createElement("div");
    div.className = "card";
    div.innerHTML = `
      <h1>${c.title} </h1> <br>
      <span>${c.description}</span> <br> <br>

      <span>Date de début: ${new Date(c.startDate).toLocaleDateString()}</span> <br>
      <span>Date de fin: ${new Date(c.endDate).toLocaleDateString()}</span> <br> <br>
      
    `;
    list.appendChild(div);
  });
}

async function vote(candidateId) {
  const voterId = prompt("Entrez votre email pour voter :");
  if (!voterId) return;

  const res = await fetch(`${API_BASE_URL}/votes`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ voterId, candidateId })
  });

  if (res.ok) {
    alert("Vote enregistré !");
    loadResults();
  } else {
    const error = await res.text();
    alert("Erreur : " + error);
  }
}

async function loadResults() {
  const res = await fetch(`${API_BASE_URL}/votes`);
//   const res = await fetch(`${API_BASE_URL}/votes/results?campaignId=1`);
  const results = await res.json();

  const container = document.getElementById("results");
  container.innerHTML = "<h2>Résultats</h2>";

  results.forEach(r => {
    const div = document.createElement("div");
    div.innerHTML = `<strong>${r.candidateId}</strong> : ${r.voterId} vote(s)`;
    container.appendChild(div);
  });
}

loadCandidates();
loadCampaigns();
loadResults();
document.getElementById('themeToggle').addEventListener('click', () => {
  const currentTheme = document.documentElement.getAttribute('data-theme');
  document.documentElement.setAttribute('data-theme', currentTheme === 'dark' ? 'light' : 'dark');
});

