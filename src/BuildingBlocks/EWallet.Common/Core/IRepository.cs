namespace EWallet.Common.Core;

public interface IRepository<TEntity> where TEntity : class
{
    IUnitOfWork UnitOfWork { get; }
    IQueryable<TEntity> GetQueryable();
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
    Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query);
    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}