using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class TeamExistsException : DomainException
{
    public override ErrorCode Code => ErrorCode.TEAM_EXISTS;
    
    public TeamExistsException() : base("team_name already exists"){}
}