namespace AvitoPRService.Domain.Exception;

public class NoCandidateException : DomainException
{
    override public string Code => "NO_CANDIDATE";
    
    public NoCandidateException() : base("no active replacement candidate in team"){}
}