namespace AvitoPRService.Domain.Exception;

public abstract class DomainException : System.Exception
{
    public abstract string Code { get; }
    
    protected DomainException(string message) : base(message) { }
    
}