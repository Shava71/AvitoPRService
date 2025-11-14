using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetTeamActiveMembersAsync(string teamName, CancellationToken cancellationToken = default);
    
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(string userId, CancellationToken cancellationToken = default);
}