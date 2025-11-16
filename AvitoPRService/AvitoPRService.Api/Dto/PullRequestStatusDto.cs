#nullable disable
namespace AvitoPRService.Application;

public enum PullRequestStatusDto
{

    [System.Runtime.Serialization.EnumMember(Value = @"OPEN")]
    OPEN = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"MERGED")]
    MERGED = 1,

}