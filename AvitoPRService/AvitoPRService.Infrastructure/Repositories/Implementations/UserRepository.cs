using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoPRService.Infrastructure.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Team)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<List<User>> GetTeamActiveMembersAsync(string teamName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Where(u => u.TeamName == teamName && u.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string userId, CancellationToken cancellationToken = default)
    {
        User? user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return;
        }
        _dbContext.Users.Remove(user);
    }
}