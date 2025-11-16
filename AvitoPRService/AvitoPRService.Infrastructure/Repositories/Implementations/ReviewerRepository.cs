using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoPRService.Infrastructure.Repositories.Implementations;

public class ReviewerRepository : IReviewerRepository
{
    private readonly AppDbContext _dbContext;

    public ReviewerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Reviewer>> GetByPRIdReviewers(string pullRequestId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviewers
            .Where(r => r.PullRequestId == pullRequestId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Reviewer> reviewers, CancellationToken cancellationToken = default)
    {
        await _dbContext.Reviewers.AddRangeAsync(reviewers, cancellationToken);
    }

    public Task RemoveAsync(Reviewer reviewer, CancellationToken cancellationToken = default)
    {
        _dbContext.Reviewers.Remove(reviewer);
        return Task.CompletedTask;
    }
    
    public async Task<int> GetTotalReviewersCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviewers.CountAsync(cancellationToken);
    }

    public async Task<List<(string UserId, string Username, int AssignmentCount)>> GetTopReviewersAsync(int topCount, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviewers
            .GroupBy(r => new { r.UserId, r.User.Username })
            .Select(g => new { 
                g.Key.UserId, 
                g.Key.Username, 
                AssignmentCount = g.Count() 
            })
            .OrderByDescending(x => x.AssignmentCount)
            .Take(topCount)
            .Select(x => new ValueTuple<string, string, int>(
                x.UserId, 
                x.Username, 
                x.AssignmentCount
            ))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Reviewer>> GetByUserIdsAsync(List<string> userIds, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviewers
            .Where(r => userIds.Contains(r.UserId))
            .Include(r => r.PullRequest)
            .ToListAsync(cancellationToken);
    }
}