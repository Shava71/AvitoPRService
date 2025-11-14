namespace AvitoPRService.Domain.Entities;

/// <summary>
/// Сущность участника ревью 
/// </summary>
public class Reviewer
{
    public string UserId { get; set; }
    public User User { get; set; }
    public string PullRequestId { get; set; }
    public PullRequest PullRequest { get; set; }
    
    public Reviewer(){}

    public Reviewer(User user, PullRequest pullRequest)
    {
        User = user;
        UserId = user.UserId;
        PullRequest = pullRequest;
        PullRequestId = pullRequest.PullRequestId;
    }
    
    /// <summary>
    /// Функция замены участника ревью на другого
    /// </summary>
    /// <param name="user">Пользователь, на которого заменяют участника ревью</param>
    public void ChangeReviewer(User user)
    {
        User = user;
        UserId = user.UserId;
    }
}