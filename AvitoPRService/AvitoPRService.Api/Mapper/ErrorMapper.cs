using AvitoPRService.Application;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Application.Mapper;

public static class ErrorMapper
{
    public static ErrorResponseDto ToResponse(DomainException ex)
    {
        return new ErrorResponseDto()
        {
            ErrorDto = new ErrorDto()
            {
                CodeDto = ToErrorCodeDto(ex.Code),
                Message = ex.Message
            }
        };
    }

    private static ErrorCodeDto ToErrorCodeDto(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.TEAM_EXISTS => ErrorCodeDto.TEAM_EXISTS,
            ErrorCode.PR_EXISTS => ErrorCodeDto.PR_EXISTS,
            ErrorCode.PR_MERGED => ErrorCodeDto.PR_MERGED,
            ErrorCode.NOT_ASSIGNED => ErrorCodeDto.NOT_ASSIGNED,
            ErrorCode.NO_CANDIDATE => ErrorCodeDto.NO_CANDIDATE,
            ErrorCode.NOT_FOUND => ErrorCodeDto.NOT_FOUND,
            _ => ErrorCodeDto.NOT_FOUND,
        };
    }
}