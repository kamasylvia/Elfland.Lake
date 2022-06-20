using System.Linq.Expressions;

namespace Elfland.Lake.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityPixie, IAggregateRoot
{
    // Read
    Task<TEntity?> FindByIdAsync(params object[] id);
    Task<TEntity?> FindByIdAsync(IEnumerable<object> id);
    Task<IEnumerable<TEntity>> FindRangeAsync(params object[] keys);
    Task<IEnumerable<TEntity>> FindRangeAsync(IEnumerable<object> keys);
    Task<IEnumerable<TEntity>> GetListAsync(
        int start = 0,
        int? end = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    );
    Task<IEnumerable<TEntity>> GetPaginationAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    );

    // Create
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(params TEntity[] entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity> InsertAsync(TEntity entity);

    // Update
    void Update(TEntity entity);
    void UpdateRange(params TEntity[] entities);
    void UpdateRange(IEnumerable<TEntity> entities);

    // Delete
    void Delete(TEntity entityToDelete);
    Task DeleteByIdAsync(object id);
    Task DeleteByIdAsync(params object[] ids);
    Task DeleteByIdAsync(IEnumerable<object> ids);
    void DeleteRange(params TEntity[] entityToDelete);
    void DeleteRange(IEnumerable<TEntity> entityToDelete);
}
