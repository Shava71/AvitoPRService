namespace AvitoPRService.Domain.Entities;

/// <summary>
/// Сущность пользователя
/// </summary>
public class User
{
    public string UserId { get; set; }
    public string Username {get; set;}
    public bool IsActive {get; set;}
    public string TeamName {get; set;}
    public Team Team {get; set;}
    public List<PullRequest> PullRequests {get; set;} = new List<PullRequest>();
    public List<Reviewer> Reviewers {get; set;} = new List<Reviewer>();
    
    public User() {}

    public User(string userId, string username, bool isActive, Team team)
    {
        UserId = userId;
        Username = username;
        IsActive = isActive;
        Team = team;
        TeamName = team.TeamName;
    }
    
    /// <summary>
    /// Функция установки активности
    /// </summary>
    /// <param name="active">булевая переменная активности</param>
    public void SetActive(bool active) => IsActive = active;
}