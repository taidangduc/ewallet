using EWallet.Common.Core;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Common.EFCore;

public class DbContextRepository<TContext, TEntity> : DbContext, IRepository<TEntity>
    where TContext : DbContext, IUnitOfWork
    where TEntity : class
{
    private readonly TContext _dbContext;
    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
    public IUnitOfWork UnitOfWork => _dbContext;

    public DbContextRepository(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);
        return Task.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return query.ToListAsync();
    }
}