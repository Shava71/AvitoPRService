using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Domain.Repositories.Interfaces;

public interface IReviewerRepository
{
    Task<List<Reviewer>> GetByPRIdReviewers(string pullRequestId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Reviewer> reviewers, CancellationToken cancellationToken = default);
    Task RemoveAsync(Reviewer reviewer, CancellationToken cancellationToken = default);
    
    Task<int> GetTotalReviewersCountAsync(CancellationToken cancellationToken = default);
    Task<List<(string UserId, string Username, int AssignmentCount)>> GetTopReviewersAsync(int topCount, CancellationToken cancellationToken = default);
    Task<List<Reviewer>> GetByUserIdsAsync(List<string> userIds, CancellationToken cancellationToken = default);
}