using AvitoPRService.Application.Dto;
using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Application.Services.Implementations;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepo;
    private readonly IUserRepository _userRepo;
    private readonly IReviewerRepository _reviewerRepo;
    private readonly IPullRequestRepository _pullRequestRepo;
    private readonly IPullRequestService _pullRequestService;
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(ITeamRepository teamRepo, IUserRepository userRepo, IUnitOfWork unitOfWork, IReviewerRepository reviewerRepo, IPullRequestRepository pullRequestRepo, IPullRequestService pullRequestService)
    {
        _teamRepo = teamRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _reviewerRepo = reviewerRepo;
        _pullRequestRepo = pullRequestRepo;
        _pullRequestService = pullRequestService;
    }
    public async Task<Team> CreateTeamAsync(string teamName, List<(string userId, string username, bool isActive)> members, CancellationToken cancellationToken = default)
    {
        Team? team = await _teamRepo.GetByNameAsync(teamName, cancellationToken);

        if (team != null)
        {
            throw new TeamExistsException(); // такая команда уже существует
        }
        else
        {
            team = new Team(teamName);
            await _teamRepo.AddAsync(team, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach ((string userId, string username, bool isActive) member in members)
        {
            User? user = await _userRepo.GetByIdAsync(member.userId, cancellationToken);

            if (user == null)
            {
                user = new User(member.userId, member.username, member.isActive, team);
                await _userRepo.AddAsync(user, cancellationToken);
            }
            else
            {
                user.Username = member.username;
                user.SetActive(member.isActive);
                user.TeamName = teamName;
                user.Team = team;

                await _userRepo.UpdateAsync(user, cancellationToken);
            }

            if (!team.Members.Contains(user))
                team.Members.Add(user);
        }

        await _teamRepo.UpdateAsync(team, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return team;
    }

    public async Task<Team?> GetTeamAsync(string teamName, CancellationToken cancellationToken = default)
    {
        Team? team = await _teamRepo.GetByNameAsync(teamName, cancellationToken);

        if (team == null)
        {
            throw new NotFoundException();
        }
        
        return team;
    }
    
    public async Task<DeactivationResult> DeactivateUsersAsync(
        List<string> userIds, 
        bool reassignOpenPRs,
        CancellationToken cancellationToken = default)
    {
        DeactivationResult result = new DeactivationResult();
        
        List<User> usersToDeactivate = new List<User>();
        foreach (string userId in userIds)
        {
            User user = await _userRepo.GetByIdAsync(userId, cancellationToken);
            if (user != null && user.IsActive)
            {
                usersToDeactivate.Add(user);
            }
        }

        foreach (User user in usersToDeactivate)
        {
            user.SetActive(false);
            result.DeactivatedUsers.Add(user.UserId);
        }

        if (reassignOpenPRs)
        {
            await ReassignPRsForDeactivatedUsersAsync(usersToDeactivate, result, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task ReassignPRsForDeactivatedUsersAsync(
        List<User> deactivatedUsers, 
        DeactivationResult result,
        CancellationToken cancellationToken = default)
    {
        List<string> userIds = deactivatedUsers.Select(u => u.UserId).ToList();
        
        List<Reviewer> allUserReviews = await _reviewerRepo.GetByUserIdsAsync(userIds, cancellationToken);
        
        var reviewsByUser = allUserReviews
            .GroupBy(r => r.UserId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (User user in deactivatedUsers)
        {
            if (reviewsByUser.TryGetValue(user.UserId, out var userReviews))
            {
                foreach (Reviewer review in userReviews)
                {
                    if (review.PullRequest?.Status == PullRequestStatus.OPEN)
                    {
                        try
                        {
                            PullRequest reassignedPr = await _pullRequestService.ReassignReviewerAsync(
                                review.PullRequestId, user.UserId, cancellationToken);
                            
                            Reviewer newReviewer = reassignedPr.Reviewers
                                .FirstOrDefault(r => r.UserId != user.UserId);
                            
                            if (newReviewer != null)
                            {
                                result.ReassignedPRs.Add(new PRReassignment(
                                    review.PullRequestId, 
                                    user.UserId, 
                                    newReviewer.UserId
                                ));
                            }
                        }
                        catch (Exception ex) when (
                            ex is NoCandidateException || 
                            ex is NotAssignedException ||
                            ex is PrMergedException)
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}