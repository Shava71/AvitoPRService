namespace AvitoPRService.Domain.Exception;

public class PrExistsException : DomainException
{
    override public string Code => "PR_EXISTS";
    
    public PrExistsException() : base("PR already exists"){}
}