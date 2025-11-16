namespace AvitoPRService.Application.Dto;

public class StatsDto
{
    public int TotalPullRequests { get; set; }
    public int OpenPullRequests { get; set; }
    public int MergedPullRequests { get; set; }
    public int TotalReviewers { get; set; }
    public int TotalTeams { get; set; }
    public int ActiveUsers { get; set; }
    
    public List<TopReviewerDto> TopReviewers { get; set; } = new();
}