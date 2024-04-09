using DEPLOY.Cachorro.Domain.Shared;

namespace DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories
{
    public interface ICachorroRepository : IGenericRepository<Entities.Cachorro, Guid>
    {
        
    }
}
