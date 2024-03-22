using DEPLOY.Cachorro.Application.Dtos;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface ICachorroAppServices
    {
        public Task<CachorroDto> InsertAsync(
            CachorroCreateDto cachorroDto, 
            CancellationToken cancellationToken = default);

        public Task<bool> UpdateAsync(
            Guid id,
            CachorroDto cachorroDto,
            CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<CachorroDto?>> GetAllAsync(
            CancellationToken cancellationToken = default);

        public Task<CachorroDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
