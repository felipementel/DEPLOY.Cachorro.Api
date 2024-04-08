using DEPLOY.Cachorro.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DEPLOY.Cachorro.Infra.Repository.Repositories.Base
{
    [ExcludeFromCodeCoverage]
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
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            //obj.GetType().GetProperty("Cadastro").SetValue(obj, new DateTime().ToLocalTime);

            await _cachorroDbContext
                .Set<TEntity>()
                .AddAsync(entity, cancellationToken);

            return entity;
        }

        public async virtual Task UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _cachorroDbContext
                    .Set<TEntity>()
                    .Attach(entity);

                _cachorroDbContext
                    .Entry(entity).State = EntityState.Modified;

            }, cancellationToken);
        }

        public async virtual Task<bool> DeleteAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            TEntity? existing = await _cachorroDbContext
                .Set<TEntity>()
                .FindAsync(new object[] { id }, cancellationToken);

            if (existing != null)
            {
                _cachorroDbContext
                    .Set<TEntity>()
                    .Remove(existing);

                return true;
            }

            return false;
        }

        public virtual async Task<List<TEntity>> GetByKeyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroDbContext
                .Set<TEntity>()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }
    }
}