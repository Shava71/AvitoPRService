using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Exception;

public class NoCandidateException : DomainException
{
    override public ErrorCode Code => ErrorCode.NO_CANDIDATE;
    
    public NoCandidateException() : base("no active replacement candidate in team"){}
}