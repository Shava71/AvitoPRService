using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.Repositories.Interfaces;

namespace AvitoPRService.Application.Services.Implementations;

public class PullRequestService : IPullRequestService
{
    private readonly IPullRequestRepository _prRepo;
    private readonly IUserRepository _userRepo;
    private readonly IReviewerRepository _reviewerRepo;

    public PullRequestService(
        IPullRequestRepository prRepo,
        IUserRepository userRepo,
        IReviewerRepository reviewerRepo)
    {
        _prRepo = prRepo;
        _userRepo = userRepo;
        _reviewerRepo = reviewerRepo;
    }

    public async Task<PullRequest> CreateAsync(string pullRequestId, string pullRequestName, string authorId,
        CancellationToken cancellationToken = default)
    {
        User author = await _userRepo.GetByIdAsync(authorId, cancellationToken)
                     ?? throw new NotFoundException();

        List<User> activeMembers = await _userRepo.GetTeamActiveMembersAsync(author.TeamName, cancellationToken);

        List<User> reviewers = activeMembers
            .Where(u => u.UserId != authorId)
            .OrderBy(_ => Guid.NewGuid())
            .Take(2)
            .ToList();

        if (!reviewers.Any())
        {
            throw new NoCandidateException();
        }

        PullRequest pr = new PullRequest(pullRequestId, pullRequestName, author);
        pr.AssignReviewers(reviewers);

        await _prRepo.AddAsync(pr, cancellationToken);

        return pr;
    }

    public async Task<PullRequest> MergeAsync(string pullRequestId, CancellationToken cancellationToken = default)
    {
        PullRequest pullRequest = await _prRepo.GetByIdAsync(pullRequestId, cancellationToken)
                 ?? throw new NotFoundException();

        pullRequest.Merge();

        await _prRepo.UpdateAsync(pullRequest, cancellationToken);

        return pullRequest;
    }

    public async Task<PullRequest> ReassignReviewerAsync(string pullRequestId, string oldReviewerId, CancellationToken cancellationToken = default)
    {
        var pr = await _prRepo.GetByIdAsync(pullRequestId, cancellationToken)
                 ?? throw new NotFoundException();

        var oldReviewer = pr.Reviewers.FirstOrDefault(r => r.UserId == oldReviewerId)
                          ?? throw new NotAssignedException();

        var user = await _userRepo.GetByIdAsync(oldReviewerId, cancellationToken)
                   ?? throw new NotFoundException();

        List<User> candidates = await _userRepo.GetTeamActiveMembersAsync(user.TeamName, cancellationToken);

        User replacement = candidates
                              .Where(u => u.UserId != oldReviewerId)
                              .OrderBy(_ => Guid.NewGuid())
                              .FirstOrDefault()
                          //?? throw new NoCandidateException()
                          ;

        pr.ReplaceReviewer(user, replacement);

        await _prRepo.UpdateAsync(pr, cancellationToken);

        return pr;
    }
}