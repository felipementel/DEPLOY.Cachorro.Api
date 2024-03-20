namespace DEPLOY.Cachorro.Domain.Shared
{
    public interface IGenericRepository<TEntity, Tid> where TEntity : BaseEntity<Tid>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Tid id, CancellationToken cancellationToken = default);
        Task<TEntity> InsertAsync(TEntity obj);
        Task UpdateAsync(TEntity obj);
        Task<bool> DeleteAsync(Tid id);
        //Task<List<TEntity>> GetByKey(Func<TEntity, bool> predicate);
    }
}