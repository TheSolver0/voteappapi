using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteApp.API.Data;
using VoteApp.API.Models;
using VoteApp.API.Models.Dto;

namespace VoteApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/votes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoteDto>>> GetVotes()
        {
            var votes = await _context.Votes.ToListAsync();
            var result = votes.Select(c => new VoteDto
            {
                VoterId = c.VoterId,
                CandidateId = c.CandidateId,
                // CandidateName = c.Candidate.FullName,
            });
            return Ok(result);
        }

        // GET: api/votes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
                return NotFound();
            return Ok(vote);
        }

        // POST: api/votes
        [HttpPost]
        public async Task<ActionResult<Vote>> CreateVote([FromBody] VoteDto voteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var candidate = await _context.Candidates
                .Include(c => c.Campaign)
                .FirstOrDefaultAsync(c => c.Id == voteDto.CandidateId);

            if (candidate == null)
                return BadRequest("Candidat invalide.");

            bool hasVoted = await _context.Votes
                .Include(v => v.Candidate)
                .AnyAsync(v => v.VoterId == voteDto.VoterId && v.Candidate.CampaignId == candidate.CampaignId);

            if (hasVoted)
                return Conflict("Ce votant a déjà voté pour cette campagne.");

            var vote = new Vote
            {
                VoterId = voteDto.VoterId,
                CandidateId = voteDto.CandidateId,
                Candidate = candidate
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVote), new { id = vote.Id }, vote);
        }

        // DELETE: api/votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
                return NotFound();

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/votes/results?campaignId=1
        [HttpGet("results")]
        public async Task<ActionResult> GetResults([FromQuery] int campaignId)
        {
            var candidates = await _context.Candidates
                .Where(c => c.CampaignId == campaignId)
                .Include(c => c.Votes)
                .ToListAsync();

            if (!candidates.Any())
                return NotFound("Aucun candidat pour cette campagne.");

            var results = candidates.Select(c => new
            {
                CandidateName = c.FullName,
                VoteCount = c.Votes.Count
            });

            return Ok(results);
        }
    }
}