using System.ComponentModel.DataAnnotations;

namespace VoteApp.API.Models.Dto
{
    public class CampaignDto
    {
        public int Id { get; set; } // facultatif pour POST, utile pour PUT

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}