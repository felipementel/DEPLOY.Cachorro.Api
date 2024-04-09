namespace DEPLOY.Cachorro.Infra.Repository.Repositories.Base
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task<bool> CommitAndSaveChangesAsync(CancellationToken cancellationToken);
    }
}