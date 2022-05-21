namespace Elfland.Lake.Domain;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
}
