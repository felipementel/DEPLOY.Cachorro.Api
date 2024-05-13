using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Shared;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentValidation;

namespace DEPLOY.Cachorro.Domain.Aggregates.Tutor.Services
{
    public class TutorService : GenericService<Entities.Tutor, long>, ITutorService
    {
        public TutorService(
            IUnitOfWork unitOfWork,
            IValidator<Entities.Tutor> validator,
            ITutorRepository tutorRepository) : base(unitOfWork, validator, tutorRepository)
        {
        }
    }
}
