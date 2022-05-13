namespace Elfland.Lake.Ddd;

public interface IUnitOfWork
{
    Task<bool> SaveAsync();
}
