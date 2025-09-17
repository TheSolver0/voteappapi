using System.ComponentModel.DataAnnotations;

namespace VoteApp.API.Models.Dto
{
    public class VoteDto
    {
        [Required]
        public string VoterId { get; set; } = null!;

        [Required]
        public int CandidateId { get; set; }
    }
}