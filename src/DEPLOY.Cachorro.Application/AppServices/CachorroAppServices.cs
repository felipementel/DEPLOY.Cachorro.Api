using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;

namespace DEPLOY.Cachorro.Application.AppServices
{
    public class CachorroAppServices : ICachorroAppServices
    {
        private readonly ICachorroService _cachorroService;

        public CachorroAppServices(ICachorroService cachorroService)
        {
            _cachorroService = cachorroService;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _cachorroService.DeleteAsync(id);
        }

        public async Task<IEnumerable<CachorroDto?>> GetAllAsync()
        {
            var retorno = await _cachorroService.GetAllAsync();

            return retorno.Select<Domain.Aggregates.Cachorro.Entities.Cachorro, CachorroDto>(x => x!).ToList();
        }

        public async Task<CachorroDto?> GetByIdAsync(Guid id)
        {
            return await _cachorroService.GetByIdAsync(id);
        }

        public async Task<CachorroDto?> InsertAsync(CachorroCreateDto cachorroDto)
        {
            return await _cachorroService.CreateAsync(cachorroDto);
        }

        public async Task<bool> UpdateAsync(Guid id, CachorroDto cachorroDto)
        {
            return await _cachorroService.UpdateAsync(id, cachorroDto);
        }
    }
}
