using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VoteApp.API.Models;
public class Campaign
{
    public int Id { get; set; }

    
    public required string Title { get; set; }

    public  string? Description { get; set; }

    
    public required DateTime StartDate { get; set; }

    
    public required DateTime EndDate { get; set; }

    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

    public required ICollection<Candidate> Candidates { get; set; }
}