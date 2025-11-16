using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AvitoPRService.Tests.Integration.Controllers;

public class UserControllerTests : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly string _teamName = $"team_{Guid.NewGuid():N}";

    public UserControllerTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeactivateUser_ShouldReturn200_AndSetIsActiveFalse()
    {
        await SetupTeamAsync();

        var request = new { user_id = "u2", is_active = false };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/users/setIsActive", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("user");
        Assert.False(user.GetProperty("is_active").GetBoolean());
    }

    [Fact]
    public async Task ReactivateUser_ShouldReturn200_AndSetIsActiveTrue()
    {
        await SetupTeamAsync();
        await DeactivateUserAsync("u2");

        var request = new { user_id = "u2", is_active = true };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/users/setIsActive", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("user");
        Assert.True(user.GetProperty("is_active").GetBoolean());
    }

    // [Fact]
    // public async Task GetUserReviews_ShouldReturn200_AndAssignedPRs()
    // {
    //     await SetupTeamAsync();
    //     await CreatePRAsync("pr-review-1", "u1");
    //
    //     // ПРОВЕРЯЕМ, ЧТО u2 — ревьювер
    //     var createResponse = await _client.PostAsync("/pullRequest/create", 
    //         new StringContent(JsonSerializer.Serialize(new
    //         {
    //             pull_request_id = "pr-review-1",
    //             pull_request_name = "Test",
    //             author_id = "u1"
    //         }), Encoding.UTF8, "application/json"));
    //
    //     var createJson = await createResponse.Content.ReadAsStringAsync();
    //     var createPr = JsonSerializer.Deserialize<JsonElement>(createJson).GetProperty("pr");
    //     var reviewers = createPr.GetProperty("assigned_reviewers").EnumerateArray();
    //     var reviewerIds = reviewers.Select(r => r.GetString()).ToList();
    //     Assert.Contains("u2", reviewerIds);
    //
    //     // ТЕПЕРЬ ПРОВЕРЯЕМ /users/getReview
    //     var response = await _client.GetAsync("/users/getReview?user_id=u2");
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //
    //     var json = await response.Content.ReadAsStringAsync();
    //     var result = JsonSerializer.Deserialize<JsonElement>(json);
    //     var prs = result.GetProperty("pull_requests").EnumerateArray();
    //     Assert.Contains(prs, p => p.GetProperty("pull_request_id").GetString() == "pr-review-1");
    // }

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

    private async Task DeactivateUserAsync(string userId)
    {
        var request = new { user_id = userId, is_active = false };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/users/setIsActive", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task CreatePRAsync(string prId, string authorId)
    {
        var request = new { pull_request_id = prId, pull_request_name = "PR " + prId, author_id = authorId };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/pullRequest/create", content);
        response.EnsureSuccessStatusCode();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}