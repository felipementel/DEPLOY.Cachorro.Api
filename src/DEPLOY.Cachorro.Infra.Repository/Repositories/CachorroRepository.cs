using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DEPLOY.Cachorro.Infra.Repository.Repositories
{
    public class CachorroRepository
        : GenericRepository<Domain.Aggregates.Cachorro.Entities.Cachorro, Guid>
        , ICachorroRepository
    {
        private readonly CachorroDbContext _cachorroContext;

        public CachorroRepository(CachorroDbContext cachorroContext) : base(cachorroContext)
        {
            _cachorroContext = cachorroContext;
        }

        public override async Task<IEnumerable<Domain.Aggregates.Cachorro.Entities.Cachorro>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cachorroContext
                .Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .Include(x => x.Tutor)
                .ToListAsync();
        }

        public override async Task<Domain.Aggregates.Cachorro.Entities.Cachorro?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroContext
                .Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .Include(x => x.Tutor)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override async Task<List<Domain.Aggregates.Cachorro.Entities.Cachorro>> GetByKeyAsync(
                       Expression<Func<Domain.Aggregates.Cachorro.Entities.Cachorro, bool>> predicate,
                       CancellationToken cancellationToken = default)
        {
            return await _cachorroContext
                .Set<Domain.Aggregates.Cachorro.Entities.Cachorro>()
                .Include(x => x.Tutor)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }
    }
}
