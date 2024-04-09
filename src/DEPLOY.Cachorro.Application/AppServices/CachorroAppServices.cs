using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DEPLOY.Cachorro.Application.AppServices
{
    [ExcludeFromCodeCoverage]
    public class CachorroAppServices : ICachorroAppServices
    {
        private readonly ICachorroService _cachorroService;

        public CachorroAppServices(ICachorroService cachorroService)
        {
            _cachorroService = cachorroService;
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroService.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<CachorroDto?>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var retorno = await _cachorroService.GetAllAsync(
                cancellationToken);

            return retorno
                .Select<Domain.Aggregates.Cachorro.Entities.Cachorro, CachorroDto>(x => x!).ToList();
        }

        public async Task<CachorroDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroService.GetByIdAsync(
                id,
                cancellationToken);
        }


        public async Task<List<CachorroDto>> GetByKeyAsync(
            Expression<Func<Domain.Aggregates.Cachorro.Entities.Cachorro, bool>> predicate,
            CancellationToken cancellationToken = default)
        { 
            var item = await _cachorroService.GetByKeyAsync(predicate, cancellationToken);

            return item.Select(x => (CachorroDto)x!).ToList();
        }

        public async Task<CachorroDto> InsertAsync(
            CachorroCreateDto cachorroDto,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroService.CreateAsync(
                cachorroDto,
                cancellationToken);
        }

        public async Task<IEnumerable<string>> UpdateAsync(
            Guid id,
            CachorroDto cachorroDto,
            CancellationToken cancellationToken = default)
        {
            return await _cachorroService.UpdateAsync(
                id,
                cachorroDto, 
                cancellationToken);
        }
    }
}
