using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.Repositories.Interfaces;

namespace AvitoPRService.Application.Services.Implementations;

public class PullRequestService : IPullRequestService
{
    private readonly IPullRequestRepository _pullRequestRepo;
    private readonly IUserRepository _userRepo;
    private readonly IReviewerRepository _reviewerRepo;

    public PullRequestService(
        IPullRequestRepository pullRequestRepo,
        IUserRepository userRepo,
        IReviewerRepository reviewerRepo)
    {
        _pullRequestRepo = pullRequestRepo;
        _userRepo = userRepo;
        _reviewerRepo = reviewerRepo;
    }

    public async Task<PullRequest> CreateAsync(string pullRequestId, string pullRequestName, string authorId,
        CancellationToken cancellationToken = default)
    {
        PullRequest? existingPr = await _pullRequestRepo.GetByIdAsync(pullRequestId, cancellationToken);
        if (existingPr != null)
        {
            throw new PrExistsException(); // такой PullRequest уже существует
        }
        
        User author = await _userRepo.GetByIdAsync(authorId, cancellationToken)
                     ?? throw new NotFoundException(); // профиль автора не найдет

        List<User> activeMembers = await _userRepo.GetTeamActiveMembersAsync(author.TeamName, cancellationToken); // находим людей из команты автора

        List<User> reviewers = activeMembers
            .Where(u => u.UserId != authorId)
            .OrderBy(_ => Guid.NewGuid())
            .Take(2)
            .ToList(); // берём двух людей из команды кроме автора

        PullRequest pr = new PullRequest(pullRequestId, pullRequestName, author);
        
        if (reviewers.Any())
        {
            await _pullRequestRepo.AddAsync(pr, cancellationToken);
        }
        pr.AssignReviewers(reviewers); // добавляем кандидатов

        return pr;
    }

    public async Task<PullRequest> MergeAsync(string pullRequestId, CancellationToken cancellationToken = default)
    {
        PullRequest pullRequest = await _pullRequestRepo.GetByIdAsync(pullRequestId, cancellationToken)
                 ?? throw new NotFoundException();

        pullRequest.Merge();

        await _pullRequestRepo.UpdateAsync(pullRequest, cancellationToken);

        return pullRequest;
    }

    public async Task<PullRequest> ReassignReviewerAsync(string pullRequestId, string oldReviewerId, CancellationToken cancellationToken = default)
    {
        var pr = await _pullRequestRepo.GetByIdAsync(pullRequestId, cancellationToken)
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
                          ?? throw new NoCandidateException()
                          ;

        pr.ReplaceReviewer(user, replacement);

        await _pullRequestRepo.UpdateAsync(pr, cancellationToken);

        return pr;
    }
}