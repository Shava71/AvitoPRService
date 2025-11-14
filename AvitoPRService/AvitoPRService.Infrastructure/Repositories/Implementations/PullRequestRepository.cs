using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoPRService.Infrastructure.Repositories.Implementations;

public class PullRequestRepository : IPullRequestRepository
{
    private readonly AppDbContext _dbContext;

    public PullRequestRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PullRequest?> GetByIdAsync(string pullRequestId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PullRequests
            .Include(pr => pr.Author)
            .Include(pr => pr.Reviewers)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(pr => pr.PullRequestId == pullRequestId, cancellationToken);
    }

    public async Task AddAsync(PullRequest pullRequest, CancellationToken cancellationToken = default)
    {
        await _dbContext.PullRequests.AddAsync(pullRequest, cancellationToken);
    }

    public Task UpdateAsync(PullRequest pullRequest, CancellationToken cancellationToken = default)
    {
        _dbContext.PullRequests.Update(pullRequest);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string pullRequestId, CancellationToken cancellationToken = default)
    {
        PullRequest? pullRequest = await _dbContext.PullRequests.FirstOrDefaultAsync(pr => pr.PullRequestId == pullRequestId, cancellationToken);
        if (pullRequest == null)
        {
            return;
        }
        _dbContext.PullRequests.Remove(pullRequest);
    }
}