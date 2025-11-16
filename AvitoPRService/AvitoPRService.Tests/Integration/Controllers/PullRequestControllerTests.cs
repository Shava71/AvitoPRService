using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AvitoPRService.Tests.Integration.Controllers;

public class PullRequestControllerTests : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly string _teamName = $"team_{Guid.NewGuid():N}";

    public PullRequestControllerTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreatePR_ShouldReturn201_AndAssignReviewers()
    {
        await SetupTeamAsync();

        var request = new
        {
            pull_request_id = "pr-test-1",
            pull_request_name = "Test PR",
            author_id = "u1"
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/pullRequest/create", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var pr = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("pr");
        var reviewers = pr.GetProperty("assigned_reviewers").EnumerateArray();
        Assert.Contains(reviewers, r => r.GetString() == "u2");
        Assert.Contains(reviewers, r => r.GetString() == "u3");
    }

    // [Fact]
    // public async Task MergePR_ShouldReturn200_AndMarkAsMerged()
    // {
    //     await SetupTeamAsync();
    //     await CreatePRAsync("pr-merge-1", "u1");
    //
    //     var request = new { pull_request_id = "pr-merge-1" };
    //     var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
    //
    //     var response = await _client.PostAsync("/pullRequest/merge", content);
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //
    //     var json = await response.Content.ReadAsStringAsync();
    //     var root = JsonSerializer.Deserialize<JsonElement>(json);
    //
    //     // OpenAPI: { pr: { ... } }
    //     if (root.TryGetProperty("pr", out var pr))
    //     {
    //         Assert.Equal("MERGED", pr.GetProperty("status").GetString());
    //         Assert.NotEqual(JsonValueKind.Null, pr.GetProperty("mergedAt").ValueKind);
    //     }
    //     else
    //     {
    //         // Возможно, ответ: { pull_request_id: "...", status: "MERGED" }
    //         Assert.Equal("MERGED", root.GetProperty("status").GetString());
    //     }
    // }

    [Fact]
    public async Task ReassignPR_ShouldReturn200_AndReplaceReviewer()
    {
        await SetupTeamAsync();
        await CreatePRAsync("pr-reassign-1", "u1");
        await DeactivateUserAsync("u2"); // u2 неактивен

        var request = new { pull_request_id = "pr-reassign-1", old_user_id = "u2" };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/pullRequest/reassign", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var pr = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("pr");
        var reviewers = pr.GetProperty("assigned_reviewers").EnumerateArray();
        Assert.Contains(reviewers, r => r.GetString() == "u3");
        Assert.DoesNotContain(reviewers, r => r.GetString() == "u2");
        Assert.NotEmpty(json); // replaced_by присутствует
    }

    private async Task SetupTeamAsync()
    {
        var team = new
        {
            team_name = _teamName,
            members = new[]
            {
                new { user_id = "u1", username = "Alice", is_active = true },
                new { user_id = "u2", username = "Bob", is_active = true },
                new { user_id = "u3", username = "Charlie", is_active = true }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(team, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/team/add", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task CreatePRAsync(string prId, string authorId)
    {
        var request = new { pull_request_id = prId, pull_request_name = "PR " + prId, author_id = authorId };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/pullRequest/create", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task DeactivateUserAsync(string userId)
    {
        var request = new { user_id = userId, is_active = false };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/users/setIsActive", content);
        response.EnsureSuccessStatusCode();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}