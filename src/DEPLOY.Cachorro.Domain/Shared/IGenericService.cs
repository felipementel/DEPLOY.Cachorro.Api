namespace DEPLOY.Cachorro.Domain.Shared
{
    public interface IGenericService<TEntity, Tid> where TEntity : BaseEntity<Tid>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(Tid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Tid id);
        Task<bool> UpdateAsync(Tid id, TEntity entity);
    }
}