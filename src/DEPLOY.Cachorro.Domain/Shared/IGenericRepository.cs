using System.Linq.Expressions;

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
            TEntity entity,
            CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            Tid id,
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetByKeyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);
    }
}