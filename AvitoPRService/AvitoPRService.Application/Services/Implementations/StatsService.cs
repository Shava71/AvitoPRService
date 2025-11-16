using AvitoPRService.Application.Dto;
using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Repositories.Interfaces;

namespace AvitoPRService.Application.Services.Implementations;

public class StatsService : IStatsService
{
    private readonly IPullRequestRepository _pullRequestRepo;
    private readonly IReviewerRepository _reviewerRepo;
    private readonly ITeamRepository _teamRepo;
    private readonly IUserRepository _userRepo;

    public StatsService(
        IPullRequestRepository pullRequestRepo,
        IReviewerRepository reviewerRepo,
        ITeamRepository teamRepo,
        IUserRepository userRepo)
    {
        _pullRequestRepo = pullRequestRepo;
        _reviewerRepo = reviewerRepo;
        _teamRepo = teamRepo;
        _userRepo = userRepo;
    }

    public async Task<StatsDto> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        var stats = new StatsDto
        {
            TotalPullRequests = await _pullRequestRepo.GetTotalCountAsync(cancellationToken),
            OpenPullRequests = await _pullRequestRepo.GetCountByStatusAsync("OPEN", cancellationToken),
            MergedPullRequests = await _pullRequestRepo.GetCountByStatusAsync("MERGED", cancellationToken),
            TotalReviewers = await _reviewerRepo.GetTotalReviewersCountAsync(cancellationToken),
            TotalTeams = await _teamRepo.GetTotalCountAsync(cancellationToken),
            ActiveUsers = await _userRepo.GetActiveUsersCountAsync(cancellationToken)
        };

        var topReviewers = await _reviewerRepo.GetTopReviewersAsync(5, cancellationToken); // top5 reviewrs
        stats.TopReviewers = topReviewers.Select(tr => new TopReviewerDto
        {
            UserId = tr.UserId,
            Username = tr.Username,
            AssignmentCount = tr.AssignmentCount
        }).ToList();

        return stats;
    }
}