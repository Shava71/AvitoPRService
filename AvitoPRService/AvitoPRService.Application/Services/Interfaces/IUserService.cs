using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Application.Services.Interfaces;

public interface IUserService
{
    Task<User> SetUserActiveAsync(string userId, bool active, CancellationToken cancellationToken = default);
    Task<List<PullRequest>> GetUserReviewsAsync(string userId, CancellationToken cancellationToken = default);
}