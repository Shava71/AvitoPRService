using AvitoPRService.Application;
using AvitoPRService.Domain.Entities;

namespace AvitoPRService.Mapper;

public static class DtoMapper
{
    public static TeamDto ToTeamDto(Team team) =>
        new TeamDto
        {
            Team_name = team.TeamName,
            Members = team.Members
                .Select(m => new TeamMemberDto
                {
                    User_id = m.UserId,
                    Username = m.Username,
                    Is_active = m.IsActive
                }).ToList()
        };

    public static UserDto ToUserDto(User u) =>
        new UserDto
        {
            User_id = u.UserId,
            Username = u.Username,
            Team_name = u.TeamName,
            Is_active = u.IsActive
        };

    public static PullRequestDto ToPullRequestDto(PullRequest pr) =>
        new PullRequestDto
        {
            Pull_request_id = pr.PullRequestId,
            Pull_request_name = pr.PullRequestName,
            Author_id = pr.AuthorId,
            StatusDto = (PullRequestStatusDto)pr.Status,
            Assigned_reviewers = pr.Reviewers.Select(r => r.UserId).ToList(),
            CreatedAt = pr.CreatedAt,
            MergedAt = pr.MergedAt
        };

    public static PullRequestShortDto ToPullRequestShortDto(PullRequest pr) =>
        new PullRequestShortDto
        {
            Pull_request_id = pr.PullRequestId,
            Pull_request_name = pr.PullRequestName,
            Author_id = pr.AuthorId,
            StatusDto = (PullRequestShortStatusDto)pr.Status
        };
}