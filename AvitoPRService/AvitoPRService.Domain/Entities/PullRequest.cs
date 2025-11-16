using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.ValueObject;

namespace AvitoPRService.Domain.Entities;

/// <summary>
/// Сущность PullRequest
/// </summary>
public class PullRequest
{
    public string PullRequestId { get; set; }
    public string PullRequestName {get; set;}
    public string AuthorId { get; set; }
    public User Author { get; set; }
    public PullRequestStatus Status {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? MergedAt {get; set;}
    public List<Reviewer> Reviewers { get; set; } = new List<Reviewer>();

    private const int MaxReviewers = 2;
    
    private PullRequest() { }

    public PullRequest(string id, string name, User author)
    {
        PullRequestId = id;
        PullRequestName = name ?? throw new ArgumentNullException(nameof(name));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        AuthorId = author.UserId;
        Status = PullRequestStatus.OPEN;
        CreatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Добавление двух кандидатов
    /// </summary>
    /// <param name="candidates">Список кандидатов</param>
    /// <param name="maxReviewers">Максимальное количество кандидатов, которые могут проводить ревью</param>
    /// <exception cref="PrMergedException"></exception>
    public void AssignReviewers(IEnumerable<User> candidates)
    {
        if (Status == PullRequestStatus.MERGED)
        {
            throw new PrMergedException();
        }
        
        Reviewers.Clear();
        foreach (User candidate in candidates.Take(MaxReviewers))
        {
            Reviewers.Add(new Reviewer(candidate, this));
        }
    }
    
    /// <summary>
    /// Замена участника ревью
    /// </summary>
    /// <param name="oldReviewer">Старый пользователь ревью</param>
    /// <param name="newReviewer">Новый пользователь, на которого заменяют</param>
    /// <exception cref="PrMergedException">PR_MERGED</exception>
    /// <exception cref="NotAssignedException">NOT_ASSIGNED</exception>
    public void ReplaceReviewer(User oldReviewer, User newReviewer)
    {
        if (Status == PullRequestStatus.MERGED)
        {
            throw new PrMergedException();
        }
        Reviewer? reviewer = Reviewers.Find(r => r.UserId == oldReviewer.UserId);
        if (reviewer == null)
        {
            throw new NotAssignedException();
        }
        Reviewers.Remove(reviewer);
        Reviewers.Add(new Reviewer(newReviewer, this));
        
    }
    
    /// <summary>
    /// Произвести merge для PullRequest
    /// </summary>
    public void Merge()
    {
        if (Status == PullRequestStatus.MERGED) return; // идемпотентность
        Status = PullRequestStatus.MERGED;
        MergedAt = DateTime.UtcNow;
    }
}