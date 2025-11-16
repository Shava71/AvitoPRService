using AvitoPRService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvitoPRService.Infrastructure.Data.Configurations;

public class PullRequestConfiguration : IEntityTypeConfiguration<PullRequest>
{
    public void Configure(EntityTypeBuilder<PullRequest> builder)
    {
        builder.ToTable("pull_request");

        builder.HasKey(pr => pr.PullRequestId);

        builder.Property(pr => pr.PullRequestId)
            .HasColumnName("pull_request_id")
            .HasMaxLength(50);

        builder.Property(pr => pr.PullRequestName)
            .HasColumnName("pull_request_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pr => pr.AuthorId)
            .HasColumnName("author_id")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pr => pr.Status)
            .HasColumnName("status")
            .HasConversion<string>() // OPEN | MERGED
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(pr => pr.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(pr => pr.MergedAt)
            .HasColumnName("merged_at");

        builder.HasIndex(pr => pr.AuthorId);

        builder.HasOne(pr => pr.Author)
            .WithMany(u => u.PullRequests)
            .HasForeignKey(pr => pr.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pr => pr.Reviewers)
            .WithOne(r => r.PullRequest)
            .HasForeignKey(r => r.PullRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}