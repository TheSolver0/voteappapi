using VoteApp.API.Models;

namespace VoteApp.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // if (context.Campaigns.Any()) return;

            var campaign = new Campaign
            {
                Title = "Élections du comité 2025",
                Description = "Vote pour le nouveau comité directeur",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5)
            };

            context.Campaigns.Add(campaign);
            await context.SaveChangesAsync();

            var candidates = new List<Candidate>
            {
                new Candidate
                {
                    FullName = "Jean Mbarga",
                    PhotoUrl = "https://example.com/photos/jean.jpg",
                    CampaignId = campaign.Id,
                    Campaign = campaign,
                    Votes = new List<Vote>()
                },
                new Candidate
                {
                    FullName = "Amina Njoya",
                    PhotoUrl = "https://example.com/photos/amina.jpg",
                    CampaignId = campaign.Id,
                    Campaign = campaign,
                    Votes = new List<Vote>()
                }
            };

            context.Candidates.AddRange(candidates);
            await context.SaveChangesAsync();

            var votes = new List<Vote>
            {
                new Vote { VoterId = "luc.dev@example.com", CandidateId = candidates[0].Id, Candidate = candidates[0] },
                new Vote { VoterId = "test.user@example.com", CandidateId = candidates[1].Id, Candidate = candidates[1] }
            };

            context.Votes.AddRange(votes);
            await context.SaveChangesAsync();
        }
    }
}