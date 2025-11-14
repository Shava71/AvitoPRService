using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class PrExistsException : DomainException
{
    override public ErrorCode Code => ErrorCode.PR_EXISTS;
    
    public PrExistsException() : base("PR already exists"){}
}