using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using VoteApp.API.Controllers;
using VoteApp.API.Data;
using VoteApp.API.Models;
using VoteApp.API.Models.Dto;
using Xunit;

public class VotesControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "VoteAppTestDb")
            .Options;

        var context = new AppDbContext(options);

        // Seed test data
        var campaign = new Campaign
        {
            Title = "Test Campaign",
            Description = "Test",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(3)
        };
        context.Campaigns.Add(campaign);
        context.SaveChanges();

        var candidate = new Candidate
        {
            FullName = "Test Candidate",
            CampaignId = campaign.Id,
            Campaign = campaign,
            Votes = new List<Vote>()
        };
        context.Candidates.Add(candidate);
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task CreateVote_ShouldReturnCreated_WhenValid()
    {
        var context = GetDbContext();
        var controller = new VotesController(context);

        var dto = new VoteDto
        {
            VoterId = "test@example.com",
            CandidateId = context.Candidates.First().Id
        };

        var result = await controller.CreateVote(dto);

        Assert.NotNull(result);
    }
    
}