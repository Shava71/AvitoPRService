using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.Repositories.Interfaces;

namespace AvitoPRService.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IPullRequestRepository _pullrequestRepo;
    private readonly IReviewerRepository _reviewerRepo;
    private readonly IUnitOfWork _unitOfWork;
    
    public UserService(
        IUserRepository userRepo,
        IPullRequestRepository pullrequestRepo,
        IReviewerRepository reviewerRepo,
        IUnitOfWork unitOfWork)
    {
        _userRepo = userRepo;
        _pullrequestRepo = pullrequestRepo;
        _reviewerRepo = reviewerRepo;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<User> SetUserActiveAsync(string userId, bool active, CancellationToken cancellationToken = default)
    {
        User user = await _userRepo.GetByIdAsync(userId, cancellationToken)
                   ?? throw new NotFoundException();

        user.SetActive(active);
        await _userRepo.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<List<PullRequest>> GetUserReviewsAsync(string userId, CancellationToken cancellationToken = default)
    {
        User user = await _userRepo.GetByIdAsync(userId, cancellationToken)
                   ?? throw new NotFoundException();

        List<Reviewer> reviewers = await _reviewerRepo.GetByPRIdReviewers(userId, cancellationToken);

        List<string> pullRequestIds = reviewers.Select(r => r.PullRequestId).ToList();

        var result = new List<PullRequest>();

        foreach (string id in pullRequestIds)
        {
            PullRequest? pr = await _pullrequestRepo.GetByIdAsync(id, cancellationToken);
            if (pr != null)
                result.Add(pr);
        }

        return result;
    }
}