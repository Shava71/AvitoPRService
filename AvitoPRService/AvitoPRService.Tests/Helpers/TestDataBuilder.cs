using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Tests.Helpers;

public static class TestDataBuilder
{
    public static Team CreateTeam(string name, params (string id, string name, bool active)[] users)
    {
        var team = new Team(name);
        foreach (var (id, username, active) in users)
        {
            var user = new User(id, username, active, team);
            team.Members.Add(user);
        }
        return team;
    }

    public static PullRequest CreatePR(string id, string name, User author, params User[] reviewers)
    {
        var pr = new PullRequest(id, name, author);
        pr.AssignReviewers(reviewers);
        return pr;
    }
}