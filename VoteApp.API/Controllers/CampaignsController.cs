using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace VoteApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new[] { "Campagne 1", "Campagne 2" });
        }
    }
}
