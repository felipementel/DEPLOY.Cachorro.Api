using DEPLOY.Cachorro.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DEPLOY.Cachorro.Infra.Repository.Repositories.Base
{
    public abstract class GenericRepository<TEntity, Tid>
        : IGenericRepository<TEntity, Tid> where TEntity
        : BaseEntity<Tid>
    {
        public readonly CachorroDbContext _cachorroDbContext;

        protected GenericRepository(CachorroDbContext cachorroContext)
        {
            _cachorroDbContext = cachorroContext;
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _cachorroDbContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken);
        }

        public async virtual Task<TEntity?> GetByIdAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroDbContext
                .Set<TEntity>()
                .FindAsync(new object[] { id }, cancellationToken);
        }

        public async virtual Task<TEntity> InsertAsync(
            TEntity obj,
            CancellationToken cancellationToken = default)
        {
            //obj.GetType().GetProperty("Cadastro").SetValue(obj, new DateTime().ToLocalTime);

            await _cachorroDbContext
                .Set<TEntity>()
                .AddAsync(obj, cancellationToken);

            return obj;
        }

        public async virtual Task UpdateAsync(
            TEntity obj,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _cachorroDbContext
                    .Set<TEntity>()
                    .Attach(obj);

                _cachorroDbContext
                    .Entry(obj).State = EntityState.Modified;
            });
        }

        public async virtual Task<bool> DeleteAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            TEntity? existing = await _cachorroDbContext.Set<TEntity>().FindAsync(new object[] {id}, cancellationToken);

            if (existing != null)
            {
                _cachorroDbContext
                    .Set<TEntity>()
                    .Remove(existing);

                return true;
            }

            return false;
        }
    }
}