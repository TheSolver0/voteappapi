using Microsoft.EntityFrameworkCore;
using VoteApp.API.Models;

namespace VoteApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Campaign → Candidates (1:N)
            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.Candidates)
                .WithOne(ca => ca.Campaign)
                .HasForeignKey(ca => ca.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            // Candidate → Votes (1:N)
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Votes)
                .WithOne(v => v.Candidate)
                .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // VoterId index (pour éviter les doublons)
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.VoterId, v.CandidateId })
                .IsUnique();


            // Seeders

            

        }
    }
}