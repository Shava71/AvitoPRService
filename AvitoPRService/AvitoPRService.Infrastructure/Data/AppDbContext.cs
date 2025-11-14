using AvitoPRService.Domain.Entities;
using AvitoPRService.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AvitoPRService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<PullRequest> PullRequests { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ReviewerConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new PullRequestConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}