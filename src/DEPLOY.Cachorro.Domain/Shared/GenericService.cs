using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

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

        public virtual async Task<TEntity> CreateAsync(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var validated = await _validator.ValidateAsync(entity,
                options => options.IncludeRuleSets("CreateNew"));

            if (!validated.IsValid)
            {
                entity.Erros = validated.Errors.Select(x => x.ErrorMessage).ToList();
                return entity;
            }

            await _genericRepository.InsertAsync(entity);

            await _uow.CommitAndSaveChangesAsync();

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(
            Tid id,
            CancellationToken cancellationToken = default)
        {
            var item = await _genericRepository.DeleteAsync(id);

            if (!item)
                return false;

            return await _uow.CommitAndSaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync
            (Tid id, 
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var validated = await _validator.ValidateAsync(entity,
                options => options.IncludeRuleSets("Update"));

            if (!validated.IsValid)
            {
                entity.Erros = validated.Errors.Select(x => x.ErrorMessage).ToList();
                return false;
            }

            var item = await _genericRepository.GetByIdAsync(id);

            if (item == null)
                return false;

            await _genericRepository.UpdateAsync(entity);

            return await _uow.CommitAndSaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var item = await _genericRepository.GetAllAsync();

            return item.Any() ? item : Enumerable.Empty<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Tid id,
            CancellationToken cancellationToken = default)
        {
            return await _genericRepository.GetByIdAsync(id);
        }
    }
}
