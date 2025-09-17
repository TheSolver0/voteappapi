using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VoteApp.API.Data;
using VoteApp.API.Models;
using VoteApp.API.Models.Dto;

namespace VoteApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CandidatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CandidateDto>), 200)]

        public async Task<ActionResult<IEnumerable<CandidateDto>>> GetCandidates()
        {
            var candidates = await _context.Candidates.ToListAsync();
            var result = candidates.Select(c => new CandidateDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhotoUrl = c.PhotoUrl ?? string.Empty,
                CampaignId = c.CampaignId
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }

        [HttpPost]
        public async Task<ActionResult<Candidate>> CreateCandidate(Candidate candidate)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate URL format
            if (!Regex.IsMatch(campaign.Url, @"^(http|https)://"))
                return BadRequest("Invalid URL format.");*/ // Proposition automatique , envie de tester plus tard

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCandidate), new { id = candidate.Id }, candidate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, Candidate updatedCandidate)
        {
            if (id != updatedCandidate.Id)
                return BadRequest("L'ID du candidat ne correspond pas");

            var existingCandidate = await _context.Candidates.FindAsync(id);
            if (existingCandidate == null)
                return NotFound();

            // Mise à jour des propriétés

            existingCandidate.FullName = updatedCandidate.FullName;
            existingCandidate.CampaignId = updatedCandidate.CampaignId;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound();
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
