using AvitoPRService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvitoPRService.Infrastructure.Data.Configurations;

public class ReviewerConfiguration : IEntityTypeConfiguration<Reviewer>
{
    public void Configure(EntityTypeBuilder<Reviewer> builder)
    {
        builder.ToTable("reviewer");

        builder.HasKey(r => new { r.UserId, r.PullRequestId });

        builder.Property(r => r.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(50);

        builder.Property(r => r.PullRequestId)
            .HasColumnName("pull_request_id")
            .HasMaxLength(50);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviewers)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PullRequest)
            .WithMany(pr => pr.Reviewers)
            .HasForeignKey(r => r.PullRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ограничение: максимум 2 ревьювера на PR
        builder.HasIndex(r => r.PullRequestId)
            .HasDatabaseName("idx_reviewer_pr");
    }
}