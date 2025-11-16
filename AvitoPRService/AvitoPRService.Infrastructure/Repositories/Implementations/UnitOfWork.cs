using System.Data;
using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AvitoPRService.Infrastructure.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _dbcontext;
    private IDbContextTransaction _transaction;

    public UnitOfWork(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
       _transaction = await _dbcontext.Database.BeginTransactionAsync(cancellationToken); 
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbcontext.SaveChangesAsync(cancellationToken);
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbcontext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_dbcontext != null)
        {
            await _dbcontext.DisposeAsync();
        }

        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
    }
}