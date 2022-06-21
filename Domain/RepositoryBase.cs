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

    public virtual async Task<IEnumerable<TEntity>> GetListAsync(
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

    public virtual async Task<IEnumerable<TEntity>> GetPaginationAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    ) =>
        await GetListAsync(
            start: pageIndex * pageSize,
            end: (pageIndex + 1) * pageSize,
            orderBy: orderBy,
            includeProperties: includeProperties,
            filters: filters
        );

    public virtual async Task<TEntity?> FindByIdAsync(params object[] id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<TEntity?> FindByIdAsync(IEnumerable<object> id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> FindRangeAsync(params object[] keys)
    {
        var result = new List<TEntity>();
        foreach (var item in keys)
        {
            result.Add((await FindByIdAsync(item!))!);
        }
        return result;
    }

    public Task<IEnumerable<TEntity>> FindRangeAsync(IEnumerable<object> keys) =>
        FindRangeAsync(keys.ToArray());

    public virtual async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public virtual async Task AddRangeAsync(params TEntity[] entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task<TEntity> InsertAsync(TEntity entity) =>
        (await _dbSet.AddAsync(entity)).Entity;

    public virtual void DeleteByEntity(TEntity entityToDelete) => _dbSet.Remove(entityToDelete);

    public virtual async Task DeleteByIdAsync(object id) => DeleteByEntity(await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException());

    public virtual async Task DeleteByIdAsync(params object[] ids) => DeleteByEntity(await _dbSet.FindAsync(ids) ?? throw new KeyNotFoundException());

    public virtual async Task DeleteByIdAsync(IEnumerable<object> ids) => DeleteByEntity(await _dbSet.FindAsync(ids) ?? throw new KeyNotFoundException());

    public virtual void DeleteRange(params TEntity[] entityToDelete) =>
        _dbSet.RemoveRange(entityToDelete);

    public virtual void DeleteRange(IEnumerable<TEntity> entityToDelete) =>
        _dbSet.RemoveRange(entityToDelete);

    public async Task DeleteRangeByKeysAsync(params object[] keys) =>
        DeleteRange(await FindRangeAsync(keys));

    public async Task DeleteRangeByKeysAsync(IEnumerable<object> keys) =>
        DeleteRange(await FindRangeAsync(keys));

    public virtual void Update(TEntity entityToUpdate) => _dbSet.Update(entityToUpdate);

    public virtual void UpdateRange(params TEntity[] entities) => _dbSet.UpdateRange(entities);

    public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);
}
