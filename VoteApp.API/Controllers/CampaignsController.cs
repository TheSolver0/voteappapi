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
    public class CampaignsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CampaignsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaigns()
        {
            var campaigns = await _context.Campaigns.ToListAsync();
            return Ok(campaigns);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
                return NotFound();
            return Ok(campaign);
        }

        [HttpPost]
        public async Task<ActionResult<Campaign>> CreateCampaign(Campaign campaign)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate URL format
            if (!Regex.IsMatch(campaign.Url, @"^(http|https)://"))
                return BadRequest("Invalid URL format.");*/ // Proposition automatique , envie de tester plus tard

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, campaign);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(int id, Campaign updatedCampaign)
        {
            if (id != updatedCampaign.Id)
                return BadRequest("L'ID de la campagne ne correspond pas");

            var existingCampaign = await _context.Campaigns.FindAsync(id);
            if (existingCampaign == null)
                return NotFound();

            // Mise à jour des propriétés

            existingCampaign.Title = updatedCampaign.Title;
            existingCampaign.Description = updatedCampaign.Description;
            existingCampaign.StartDate = updatedCampaign.StartDate;
            existingCampaign.EndDate = updatedCampaign.EndDate;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if(campaign == null)
                return NotFound();  
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
