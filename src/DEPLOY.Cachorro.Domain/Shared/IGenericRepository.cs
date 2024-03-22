namespace DEPLOY.Cachorro.Domain.Shared
{
    public interface IGenericRepository<TEntity, Tid> where TEntity : BaseEntity<Tid>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(
            Tid id, 
            CancellationToken cancellationToken = default);

        Task<TEntity> InsertAsync(
            TEntity obj,
            CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity obj,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Tid id,
            CancellationToken cancellationToken = default);

        //Task<List<TEntity>> GetByKey(Func<TEntity, bool> predicate);
    }
}