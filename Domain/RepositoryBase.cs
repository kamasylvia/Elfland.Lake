using System.Linq.Expressions;
using Elfland.Lake.Attributes;
using Elfland.Lake.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Domain;

[ApplicationService(ServiceLifetime.Scoped)]
public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : EntityPixie, IAggregateRoot
    where TDbContext : DbContext
{
    protected TDbContext _context;
    protected DbSet<TEntity> _dbSet;

    public RepositoryBase(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> SearchAsync(
        int start = 0,
        int? end = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    )
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var filter in filters)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        foreach (
            var includeProperty in includeProperties.Split(
                new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries
            )
        )
        {
            query = query.Include(includeProperty);
        }

        return end.HasValue
            ? await query.Skip(start).Take(end.Value - start).ToListAsync()
            : await query.Skip(start).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> SearchByPaginationAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    ) =>
        await SearchAsync(
            start: pageIndex * pageSize,
            end: (pageIndex + 1) * pageSize,
            orderBy: orderBy,
            includeProperties: includeProperties,
            filters: filters
        );

    public virtual async Task<TEntity?> FindByIdAsync(params object[] keys) =>
        await _dbSet.FindAsync(keys);

    public virtual async Task<TEntity?> FindByIdAsync(IEnumerable<object> keys) =>
        await _dbSet.FindAsync(keys);

    public async Task<IEnumerable<TEntity>> FindRangeAsync(params object[] keys) => await keys.ToAsyncEnumerable().SelectAwait(async key => await FindByIdAsync(key) ?? throw new KeyNotFoundException()).ToListAsync();

    public Task<IEnumerable<TEntity>> FindRangeAsync(IEnumerable<object> keys) =>
        FindRangeAsync(keys.ToArray());

    public virtual async Task AddAsync(params TEntity[] entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task AddAsync(IEnumerable<TEntity> entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task<TEntity> InsertAsync(TEntity entity) =>
        (await _dbSet.AddAsync(entity)).Entity;

    public virtual void Delete(params TEntity[] entities) => _dbSet.RemoveRange(entities);

    public virtual void Delete(IEnumerable<TEntity> entities) =>
        _dbSet.RemoveRange(entities);

    public virtual async Task DeleteByIdAsync(params object[] keys) => Delete(await _dbSet.FindAsync(keys) ?? throw new KeyNotFoundException());

    public virtual async Task DeleteByIdAsync(IEnumerable<object> keys) => Delete(await _dbSet.FindAsync(keys) ?? throw new KeyNotFoundException());

    public async Task DeleteRangeByKeysAsync(params object[] keys) =>
        await keys.ToAsyncEnumerable().ForEachAsync(async key => await DeleteByIdAsync(key));

    public async Task DeleteRangeByKeysAsync(IEnumerable<object> keys) =>
        await keys.ToAsyncEnumerable().ForEachAsync(async key => await DeleteByIdAsync(key));

    public virtual void Update(params TEntity[] entities) => _dbSet.UpdateRange(entities);

    public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);
}
