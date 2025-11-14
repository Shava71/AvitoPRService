namespace AvitoPRService.Domain.Exception;

public class NotFoundException : DomainException
{
    override public string Code => "NOT_FOUND";
    
    public NotFoundException() : base("resource not found"){}
}