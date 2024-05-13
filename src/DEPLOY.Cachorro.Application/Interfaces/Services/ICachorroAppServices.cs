using DEPLOY.Cachorro.Application.Dtos;
using System.Linq.Expressions;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface ICachorroAppServices
    {
        public Task<CachorroDto> InsertAsync(
            CachorroCreateDto cachorroDto,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<string>> UpdateAsync(
            Guid id,
            CachorroDto cachorroDto,
            CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<CachorroDto>> GetAllAsync(
            CancellationToken cancellationToken = default);

        public Task<CachorroDto> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<CachorroDto>> GetByKeyAsync(
            Expression<Func<CachorroDto, bool>> predicate,
            CancellationToken cancellationToken = default);

    }
}
