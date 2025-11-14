namespace AvitoPRService.Domain.Exception;

public class NotAssignedException : DomainException
{
    override public string Code => "NOT_ASSIGNED";
    
    public NotAssignedException() : base("reviewer is not assigned to PR"){}
}