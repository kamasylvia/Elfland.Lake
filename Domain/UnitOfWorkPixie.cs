using Elfland.Lake.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Domain;

[ApplicationService(ServiceLifetime.Scoped)]
public abstract class UnitOfWorkPixie<TDbContext> : IUnitOfWorkPixie where TDbContext : DbContext
{
    protected readonly TDbContext _context;
    protected IDbContextTransaction? _currentTransaction;

    public UnitOfWorkPixie(TDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        _currentTransaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);

        _context.ChangeTracker
            .Entries()
            .Where(
                entry =>
                    entry.Entity is EntityPixie
                    && (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            )
            .ToList()
            .ForEach(entry =>
            {
                if (entry.State == EntityState.Added)
                {
                    (entry.Entity as EntityPixie)!.UpdatedTime = (entry.Entity as EntityPixie)!.CreatedTime;
                }
                else
                {
                    (entry.Entity as EntityPixie)!.UpdatedTime = DateTime.UtcNow;
                }
            });

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

    public virtual async Task<bool> SaveAndCommitAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    ) =>
        await SaveChangesAsync(cancellationToken)
        && await CommitTransactionAsync(cancellationToken);

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
