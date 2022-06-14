using System.Linq.Expressions;

namespace Elfland.Lake.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntityPixie, IAggregateRoot
{
    // Read
    Task<TEntity?> FindByIdAsync(params object[] id);
    Task<TEntity?> FindByIdAsync(IEnumerable<object> id);
    Task<IEnumerable<TEntity>> GetListAsync(
        int? count = null,
        Expression<Func<TEntity, bool>>? filter = null
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
}
