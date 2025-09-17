async function loadCandidates() {
  const res = await fetch(`${API_BASE_URL}/candidates`);
  const candidates = await res.json();

  const list = document.getElementById("candidate-list");
  list.innerHTML = "<h2>Candidats</h2>";

  candidates.forEach(c => {
    const div = document.createElement("div");
    div.className = "candidate";
    div.innerHTML = `
      <img src="${c.photoUrl}" alt="${c.fullName}">
      <span>${c.fullName}</span>
      <button class="vote-btn" onclick="vote('${c.id}')">Voter</button>
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
    div.innerHTML = `<strong>${r.candidateName}</strong> : ${r.votes} vote(s)`;
    container.appendChild(div);
  });
}

loadCandidates();
loadResults();