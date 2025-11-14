using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Api.ApiMapper;

public static class PullRequestStatusMapper
{
    public static PullRequestStatusDto ToDto(this PullRequestStatus status)
    {
        return status switch
        {
            PullRequestStatus.OPEN => PullRequestStatusDto.OPEN,
            PullRequestStatus.MERGED => PullRequestStatusDto.MERGED,
            
        };
    }
}