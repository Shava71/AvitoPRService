namespace AvitoPRService.Application.Dto;

public class PRReassignment
{
    public string PullRequestId { get; set; } = string.Empty;
    public string OldReviewerId { get; set; } = string.Empty;
    public string NewReviewerId { get; set; } = string.Empty;

    public PRReassignment(string pullRequestId, string oldReviewerId, string newReviewerId)
    {
        PullRequestId = pullRequestId;
        OldReviewerId = oldReviewerId;
        NewReviewerId = newReviewerId;
    }
}