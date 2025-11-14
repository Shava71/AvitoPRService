namespace AvitoPRService.Domain.Exception;

public class PrMergedException : DomainException
{
    override public string Code => "PR_MERGED";
    
    public PrMergedException() : base("cannot reassign on merged PR"){}
}