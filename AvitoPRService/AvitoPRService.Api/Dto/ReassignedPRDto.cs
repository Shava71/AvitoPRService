using System.Text.Json.Serialization;

namespace AvitoPRService.Application;

public class ReassignedPRDto
{
    [JsonPropertyName("pull_request_id")]
    public string PullRequestId { get; set; } = string.Empty;

    [JsonPropertyName("old_reviewer_id")] 
    public string OldReviewerId { get; set; } = string.Empty;

    [JsonPropertyName("new_reviewer_id")]
    public string NewReviewerId { get; set; } = string.Empty;
}