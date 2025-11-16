using AvitoPRService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvitoPRService.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(50);

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(u => u.TeamName)
            .HasColumnName("team_name")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasIndex(u => u.Username);

        builder.HasOne(u => u.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(u => u.TeamName)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.PullRequests)
            .WithOne(pr => pr.Author)
            .HasForeignKey(pr => pr.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}