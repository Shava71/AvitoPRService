using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class NotAssignedException : DomainException
{
    override public ErrorCode Code => ErrorCode.NOT_ASSIGNED;
    
    public NotAssignedException() : base("reviewer is not assigned to PR"){}
}