using System.Text.Json.Serialization;
using AvitoPRService.Application;

namespace AvitoPRService.Api;

public class DeactivateUsersResponse
{
    [JsonPropertyName("deactivated_users")]
    public List<string> DeactivatedUsers { get; set; } = new();

    [JsonPropertyName("reassigned_prs")]
    public List<ReassignedPRDto> ReassignedPRs { get; set; } = new();
}