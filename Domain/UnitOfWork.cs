using Microsoft.EntityFrameworkCore;

namespace Elfland.Lake.Domain;

public abstract class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    protected readonly TDbContext _context;

    public UnitOfWork(TDbContext context)
    {
        _context = context;
    }

    public virtual async Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    ) => await _context.SaveChangesAsync() >= 0;

    #region Disposable
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
