using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Application.Services.Interfaces;

public interface IPullRequestService
{
    Task<PullRequest> CreateAsync(string pullRequestId, string pullRequestName, string authorId, CancellationToken cancellationToken= default);
    Task<PullRequest> MergeAsync(string pullRequestId, CancellationToken cancellationToken = default);
    Task<PullRequest> ReassignReviewerAsync(string pullRequestId, string oldReviewerId, CancellationToken cancellationToken = default);
}