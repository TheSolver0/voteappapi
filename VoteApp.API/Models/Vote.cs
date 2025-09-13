using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VoteApp.API.Models;

public class Vote
{
    public int Id { get; set; }

  
    public required string VoterId { get; set; } 

   public required int CandidateId { get; set; }

    public required Candidate Candidate { get; set; }

    public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}