using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoPRService.Infrastructure.Repositories.Implementations;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _dbContext;

    public TeamRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Team?> GetByNameAsync(string teamName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.TeamName == teamName, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string teamName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Teams.AnyAsync(t => t.TeamName == teamName, cancellationToken);
    }

    public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        await _dbContext.Teams.AddAsync(team, cancellationToken);
    }

    public async Task UpdateAsync(Team team, CancellationToken cancellationToken = default)
    {
        _dbContext.Teams.Update(team);
    }

    public async Task DeleteAsync(string teamName, CancellationToken cancellationToken = default)
    {
        Team? team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.TeamName == teamName, cancellationToken);
        if (team == null)
        {
            return;
        }
        _dbContext.Teams.Remove(team);
    }
}