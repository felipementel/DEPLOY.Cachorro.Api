using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Shared;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentValidation;

namespace DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Services
{
    public class CachorroService : GenericService<Entities.Cachorro, Guid>, ICachorroService
    {
        public CachorroService(
            IUnitOfWork unitOfWork,
            IValidator<Entities.Cachorro> validator,
            ICachorroRepository cachorroRepository) : base(unitOfWork, validator, cachorroRepository)
        {
        }
    }
}
