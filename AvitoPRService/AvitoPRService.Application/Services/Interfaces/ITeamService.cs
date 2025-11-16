using AvitoPRService.Application.Dto;
using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Application.Services.Interfaces;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(string teamName, List<(string userId, string username, bool isActive)> members, CancellationToken cancellationToken = default);
    Task<Team?> GetTeamAsync(string teamName, CancellationToken cancellationToken = default);

    Task<DeactivationResult> DeactivateUsersAsync(List<string> userIds, bool reassignOpenPRs, CancellationToken cancellationToken = default);

    Task ReassignPRsForDeactivatedUsersAsync(List<User> deactivatedUsers, DeactivationResult result,
        CancellationToken cancellationToken = default);
}