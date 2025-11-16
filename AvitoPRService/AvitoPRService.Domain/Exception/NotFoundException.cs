using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class NotFoundException : DomainException
{
    override public ErrorCode Code => ErrorCode.NOT_FOUND;
    
    public NotFoundException() : base("resource not found"){}
}