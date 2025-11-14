using AvitoPRService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvitoPRService.Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("team");

        builder.HasKey(t => t.TeamName);

        builder.Property(t => t.TeamName)
            .HasColumnName("team_name")
            .HasMaxLength(100);

        builder.HasIndex(t => t.TeamName)
            .IsUnique(); 

        builder.HasMany(t => t.Members)
            .WithOne(u => u.Team)
            .HasForeignKey(u => u.TeamName)
            .OnDelete(DeleteBehavior.Restrict);
    }
}