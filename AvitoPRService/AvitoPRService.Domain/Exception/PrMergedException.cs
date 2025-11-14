using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class PrMergedException : DomainException
{
    override public ErrorCode Code => ErrorCode.PR_MERGED;
    
    public PrMergedException() : base("cannot reassign on merged PR"){}
}