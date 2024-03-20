namespace DEPLOY.Cachorro.Infra.Repository.Repositories.Base
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> CommitAndSaveChangesAsync();
    }
}