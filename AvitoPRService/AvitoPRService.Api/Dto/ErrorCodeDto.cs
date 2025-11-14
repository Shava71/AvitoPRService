#nullable disable
namespace AvitoPRService.Application;

public enum ErrorCodeDto
{

    [System.Runtime.Serialization.EnumMember(Value = @"TEAM_EXISTS")]
    TEAM_EXISTS = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"PR_EXISTS")]
    PR_EXISTS = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"PR_MERGED")]
    PR_MERGED = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"NOT_ASSIGNED")]
    NOT_ASSIGNED = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"NO_CANDIDATE")]
    NO_CANDIDATE = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"NOT_FOUND")]
    NOT_FOUND = 5,

}