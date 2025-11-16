using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public abstract class DomainException : System.Exception
{
    public abstract ErrorCode Code { get; }
    
    protected DomainException(string message) : base(message) { }
    
}