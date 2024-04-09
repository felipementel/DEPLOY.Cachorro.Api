using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentValidation;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DEPLOY.Cachorro.Domain.Shared
{
    [ExcludeFromCodeCoverage]
    public abstract class GenericService<TEntity, Tid>
        : IGenericService<TEntity, Tid> where TEntity
        : BaseEntity<Tid>
    {
        private readonly IUnitOfWork _uow;

        private readonly IValidator<TEntity> _validator;

        private readonly IGenericRepository<TEntity, Tid> _genericRepository;        

        protected GenericService(
            IUnitOfWork uow,
            IValidator<TEntity> validator,
            IGenericRepository<TEntity, Tid> genericRepository)
        {
            _uow = uow;
            _validator = validator;
            _genericRepository = genericRepository;
        }

        public virtual async Task<TEntity> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var validated = await _validator.ValidateAsync(entity,
                options => options.IncludeRuleSets("CreateNew"));

            if (!validated.IsValid)
            {
                entity.Erros = validated.Errors.Select(x => x.ErrorMessage).ToList();
                return entity;
            }

            await _genericRepository.InsertAsync(entity, cancellationToken);

            await _uow.CommitAndSaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            var item = await _genericRepository.DeleteAsync(id, cancellationToken);

            if (!item)
                return false;

            return await _uow.CommitAndSaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<string>> UpdateAsync(
            Tid id, 
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var validated = await _validator.ValidateAsync(entity,
                options => options.IncludeRuleSets("Update"));

            if (!validated.IsValid)
            {
                return validated.Errors.Select(x => x.ErrorMessage).ToList();
            }

            var item = await _genericRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
                return entity.Erros.ToFrozenSet();

            await _genericRepository.UpdateAsync(entity, cancellationToken);

            await _uow.CommitAndSaveChangesAsync(cancellationToken);

            return entity.Erros;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _genericRepository.GetAllAsync(cancellationToken);

            return items?.Count() > 0 ? items : Enumerable.Empty<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            return await _genericRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<List<TEntity>> GetByKeyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return _genericRepository.GetByKeyAsync(predicate, cancellationToken);
        }
    }
}
