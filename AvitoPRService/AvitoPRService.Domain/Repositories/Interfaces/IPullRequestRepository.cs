using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Domain.Repositories.Interfaces;

public interface IPullRequestRepository
{
    Task<PullRequest?> GetByIdAsync(string pullRequestId, CancellationToken cancellationToken = default);
    Task AddAsync(PullRequest pullRequest, CancellationToken cancellationToken = default);
    Task UpdateAsync(PullRequest pullRequest, CancellationToken cancellationToken = default);
    Task DeleteAsync(string pullRequestId, CancellationToken cancellationToken = default);
    
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountByStatusAsync(string status, CancellationToken cancellationToken = default);
}