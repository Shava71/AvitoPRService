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
}