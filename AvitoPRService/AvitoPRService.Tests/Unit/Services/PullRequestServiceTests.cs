using AvitoPRService.Application.Services.Implementations;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Tests.Helpers;
using AvitoPRService.Tests.Unit.Mocks;
using Moq;

namespace AvitoPRService.Tests.Unit.Services;

public class PullRequestServiceTests
{
    private readonly List<User> _users = new();
    private readonly List<PullRequest> _prs = new();

    [Fact]
    public async Task CreateAsync_ShouldAssignTwoReviewers_WhenEnoughActiveMembers()
    {
        // Arrange
        Team team = TestDataBuilder.CreateTeam("backend",
            ("u1", "Alice", true),
            ("u2", "Bob", true),
            ("u3", "Charlie", true));

        _users.AddRange(team.Members);

        Mock<IUserRepository> userRepo = MockRepositories.GetMockUserRepo(_users);
        Mock<IPullRequestRepository> prRepo = MockRepositories.GetMockPRRepo(_prs);
        Mock<IReviewerRepository> reviewerRepo = new Mock<IReviewerRepository>();
        Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        PullRequestService service = new PullRequestService(prRepo.Object, userRepo.Object, reviewerRepo.Object, uow.Object);

        // Act
        PullRequest pr = await service.CreateAsync("pr1", "Fix bug", "u1");

        // Assert
        Assert.Equal(2, pr.Reviewers.Count);
        Assert.Contains(pr.Reviewers, r => r.UserId == "u2" || r.UserId == "u3");
        Assert.DoesNotContain(pr.Reviewers, r => r.UserId == "u1");
    }

    // [Fact] довольно не идемпотентный тест
    // public async Task ReassignReviewerAsync_ShouldReplaceWithAnotherActiveUser()
    // {
    //     // Arrange
    //     var team = TestDataBuilder.CreateTeam("team",
    //         ("u1", "A", true), ("u2", "B", true), ("u3", "C", true));
    //     _users.AddRange(team.Members);
    //
    //     // НАЗНАЧАЕМ ДВУХ РЕВЬЮВЕРОВ
    //     var pr = TestDataBuilder.CreatePR("pr1", "Test", _users[0], _users[1], _users[2]);
    //     _prs.Add(pr);
    //
    //     var userRepo = MockRepositories.GetMockUserRepo(_users);
    //     var prRepo = MockRepositories.GetMockPRRepo(_prs);
    //     var reviewerRepo = new Mock<IReviewerRepository>();
    //     var uow = new Mock<IUnitOfWork>();
    //
    //     var service = new PullRequestService(prRepo.Object, userRepo.Object, reviewerRepo.Object, uow.Object);
    //
    //     // Act
    //     var updatedPr = await service.ReassignReviewerAsync("pr1", "u2");
    //
    //     // Assert
    //     Assert.Equal(2, updatedPr.Reviewers.Count);
    //     Assert.Contains(updatedPr.Reviewers, r => r.UserId == "u1"); // автор
    //     Assert.Contains(updatedPr.Reviewers, r => r.UserId == "u3"); // новый
    //     Assert.DoesNotContain(updatedPr.Reviewers, r => r.UserId == "u2"); // старый удалён
    // }
}