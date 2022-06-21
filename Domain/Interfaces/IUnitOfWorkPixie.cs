namespace Elfland.Lake.Domain;

public interface IUnitOfWorkPixie : IDisposable
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> CommitTransactionAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    );
    Task<bool> SaveAndCommitAsync(CancellationToken cancellationToken = default(CancellationToken));
}
