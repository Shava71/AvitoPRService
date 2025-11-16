using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using Moq;

namespace AvitoPRService.Tests.Unit.Mocks;

public static class MockRepositories
{
    public static Mock<IUserRepository> GetMockUserRepo(List<User> users)
    {
        Mock<IUserRepository> mock = new Mock<IUserRepository>();
        mock.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string id, CancellationToken ct) => users.FirstOrDefault(u => u.UserId == id));

        mock.Setup(r => r.GetTeamActiveMembersAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string team, CancellationToken ct) =>
                users.Where(u => u.TeamName == team && u.IsActive).ToList());

        mock.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, ct) => users.Add(u));

        mock.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, ct) => { });

        return mock;
    }

    public static Mock<IPullRequestRepository> GetMockPRRepo(List<PullRequest> prs)
    {
        Mock<IPullRequestRepository> mock = new Mock<IPullRequestRepository>();
        mock.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string id, CancellationToken ct) => prs.FirstOrDefault(p => p.PullRequestId == id));

        mock.Setup(r => r.AddAsync(It.IsAny<PullRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PullRequest, CancellationToken>((p, ct) => prs.Add(p));

        mock.Setup(r => r.UpdateAsync(It.IsAny<PullRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PullRequest, CancellationToken>((p, ct) => { });

        return mock;
    }
}