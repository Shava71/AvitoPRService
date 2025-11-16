using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AvitoPRService.Tests.Integration.Controllers;

public class TeamControllerTests : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly string _teamName = $"team_{Guid.NewGuid():N}";

    public TeamControllerTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddTeam_ShouldReturn201_AndCreateTeam()
    {
        var team = new
        {
            team_name = _teamName,
            members = new[]
            {
                new { user_id = "u1", username = "Alice", is_active = true },
                new { user_id = "u2", username = "Bob", is_active = true }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(team, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/team/add", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetTeam_ShouldReturn200_AndTeamData()
    {
        await AddTeamAsync(_teamName, "u3", "u4");

        var response = await _client.GetAsync($"/team/get?team_name={_teamName}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var team = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(_teamName, team.GetProperty("team_name").GetString());
        Assert.Equal(2, team.GetProperty("members").GetArrayLength());
    }

    private async Task AddTeamAsync(string teamName, params string[] userIds)
    {
        var members = userIds.Select((id, i) => new
        {
            user_id = id,
            username = $"User{i + 1}",
            is_active = true
        }).ToArray();

        var team = new { team_name = teamName, members };
        var content = new StringContent(
            JsonSerializer.Serialize(team, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/team/add", content);
        response.EnsureSuccessStatusCode();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}