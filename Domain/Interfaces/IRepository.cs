using System.Linq.Expressions;

namespace Elfland.Lake.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityPixie, IAggregateRoot
{
    // Read
    Task<TEntity?> FindByIdAsync(params object[] keyValues);
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
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entities);

    // Update
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    // Delete
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
    Task DeleteByIdAsync(params object[] keyValues);
    Task DeleteRangeByKeysAsync(IEnumerable<object> keys);
}
