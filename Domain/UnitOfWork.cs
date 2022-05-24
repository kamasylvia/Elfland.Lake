using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Elfland.Lake.Domain;

public abstract class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    protected readonly TDbContext _context;
    protected IDbContextTransaction? _currentTransaction;

    public UnitOfWork(TDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        _currentTransaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) >= 0;
    }

    public virtual async Task<bool> CommitTransactionAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        try
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch (System.Exception)
        {
            _currentTransaction?.Rollback();
            return false;
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

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
