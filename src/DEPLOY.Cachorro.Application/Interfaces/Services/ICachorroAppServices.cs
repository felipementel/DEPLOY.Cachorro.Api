using DEPLOY.Cachorro.Application.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface ICachorroAppServices
    {
        public Task<CachorroDto> InsertAsync(CachorroCreateDto cachorroDto);

        public Task<bool> UpdateAsync(Guid id, CachorroDto cachorroDto);

        public Task<bool> DeleteAsync(Guid id);

        public Task<IEnumerable<CachorroDto?>> GetAllAsync();

        public Task<CachorroDto?> GetByIdAsync(Guid id);
    }
}
