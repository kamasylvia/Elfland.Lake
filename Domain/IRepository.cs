namespace Elfland.Lake.Domain;

public interface IRepository<TEntity> where TEntity : class
{
    // Read
    Task<TEntity?> GetByIdAsync(params object[] id);
    Task<TEntity?> GetByIdAsync(IEnumerable<object> id);
    Task<IEnumerable<TEntity>> GetListAsync(int? pageSize = null);

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
