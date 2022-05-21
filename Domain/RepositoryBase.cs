using Elfland.Lake.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Domain;

[ApplicationService(ServiceLifetime.Scoped)]
public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : class
    where TDbContext : DbContext
{
    protected TDbContext _context;
    protected DbSet<TEntity> _dbSet;

    public RepositoryBase(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetListAsync(int? length = null) =>
        length.HasValue
            ? await _dbSet.Take(length.Value).ToListAsync()
            : await _dbSet.ToListAsync();

    public virtual async Task<TEntity?> FindByIdAsync(params object[] id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<TEntity?> FindByIdAsync(IEnumerable<object> id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public virtual async Task AddRangeAsync(params TEntity[] entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await _dbSet.AddRangeAsync(entities);

    public virtual async Task<TEntity> InsertAsync(TEntity entity) =>
        (await _dbSet.AddAsync(entity)).Entity;

    public virtual void Delete(TEntity entityToDelete) => _dbSet.Remove(entityToDelete);

    public virtual async Task DeleteByIdAsync(object id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete is not null)
        {
            Delete(entityToDelete);
        }
    }

    public virtual async Task DeleteByIdAsync(params object[] ids)
    {
        foreach (var item in ids)
        {
            await DeleteByIdAsync(item);
        }
    }

    public virtual async Task DeleteByIdAsync(IEnumerable<object> ids)
    {
        foreach (var item in ids)
        {
            await DeleteByIdAsync(item);
        }
    }

    public virtual void DeleteRange(params TEntity[] entityToDelete) =>
        _dbSet.RemoveRange(entityToDelete);

    public virtual void DeleteRange(IEnumerable<TEntity> entityToDelete) =>
        _dbSet.RemoveRange(entityToDelete);

    public virtual void Update(TEntity entityToUpdate) => _dbSet.Update(entityToUpdate);

    public virtual void UpdateRange(params TEntity[] entities) => _dbSet.UpdateRange(entities);

    public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);
}
