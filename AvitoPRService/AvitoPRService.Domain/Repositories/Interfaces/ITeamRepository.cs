using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Domain.Repositories.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByNameAsync(string teamName, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string teamName, CancellationToken cancellationToken = default);
    Task AddAsync(Team team, CancellationToken cancellationToken = default);
    Task UpdateAsync(Team team, CancellationToken cancellationToken = default);
    Task DeleteAsync(string teamName, CancellationToken cancellationToken = default);
    
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}