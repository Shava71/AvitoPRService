namespace AvitoPRService.Domain.Entities;

/// <summary>
/// Сущность команды
/// </summary>
public class Team
{
    public string TeamName {get; set;}
    
    public List<User> Members {get; set;} = new List<User>();
    
    public Team(){}

    public Team(string teamName)
    {
        TeamName = teamName;
    }
}