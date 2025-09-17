using System.ComponentModel.DataAnnotations;

namespace VoteApp.API.Models.Dto
{
    public class CandidateDto
    {
        public int Id { get; set; } // facultatif pour POST, utile pour PUT

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Url]
        public string PhotoUrl { get; set; } = null!;

        [Required]
        public int CampaignId { get; set; }
    }
}