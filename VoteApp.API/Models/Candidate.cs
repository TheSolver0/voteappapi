using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VoteApp.API.Models;

public class Candidate
{
    public int Id { get; set; }

    
    public required string FullName { get; set; }

    public  string? PhotoUrl { get; set; }

    
    public int CampaignId { get; set; }

    public required Campaign Campaign { get; set; }

    public required ICollection<Vote> Votes { get; set; }
}
