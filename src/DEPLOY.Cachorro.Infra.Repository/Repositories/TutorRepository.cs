using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;

namespace DEPLOY.Cachorro.Infra.Repository.Repositories
{
    public class TutorRepository : GenericRepository<Domain.Aggregates.Tutor.Entities.Tutor, long>, ITutorRepository
    {
        public TutorRepository(CachorroDbContext cachorroContext) : base(cachorroContext)
        {

        }
    }
}
