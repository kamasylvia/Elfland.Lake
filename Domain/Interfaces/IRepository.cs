using System.Linq.Expressions;

namespace Elfland.Lake.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityPixie, IAggregateRoot
{
    // Read
    Task<TEntity?> FindByIdAsync(params object[] keys);
    Task<TEntity?> FindByIdAsync(IEnumerable<object> keys);
    Task<IEnumerable<TEntity>> FindRangeAsync(params object[] keys);
    Task<IEnumerable<TEntity>> FindRangeAsync(IEnumerable<object> keys);
    Task<IEnumerable<TEntity>> SearchAsync(
        int start = 0,
        int? end = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    );
    Task<IEnumerable<TEntity>> SearchByPaginationAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        params Expression<Func<TEntity, bool>>[] filters
    );

    // Create
    Task AddAsync(params TEntity[] entities);
    Task AddAsync(IEnumerable<TEntity> entities);
    Task<TEntity> InsertAsync(TEntity entity);

    // Update
    void Update(params TEntity[] entities);
    void Update(IEnumerable<TEntity> entities);

    // Delete
    void Delete(params TEntity[] entities);
    void Delete(IEnumerable<TEntity> entities);
    Task DeleteByIdAsync(params object[] keys);
    Task DeleteByIdAsync(IEnumerable<object> keys);
    Task DeleteRangeByKeysAsync(params object[] keys);
    Task DeleteRangeByKeysAsync(IEnumerable<object> keys);
}
