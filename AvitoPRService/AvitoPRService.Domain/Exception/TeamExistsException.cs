namespace AvitoPRService.Domain.Exception;

public class TeamExistsException : DomainException
{
    public override string Code => "TEAM_EXISTS";
    
    public TeamExistsException() : base("team_name already exists"){}
}