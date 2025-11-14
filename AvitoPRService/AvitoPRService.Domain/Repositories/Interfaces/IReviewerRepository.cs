using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Domain.Repositories.Interfaces;

public interface IReviewerRepository
{
    Task<List<Reviewer>> GetByPRIdReviewers(string pullRequestId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Reviewer> reviewers, CancellationToken cancellationToken = default);
    Task RemoveAsync(Reviewer reviewer, CancellationToken cancellationToken = default);
}