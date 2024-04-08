using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.AppServices
{
    [ExcludeFromCodeCoverage]
    public class AdocaoAppService : IAdocaoAppService
    {
        private readonly IAdocaoService _adocaoService;

        public AdocaoAppService(IAdocaoService adocaoService)
        {
            _adocaoService = adocaoService;
        }

        public async Task<IEnumerable<string>> AdotarAsync(
            Guid cachorroId, 
            long tutorId, 
            CancellationToken cancellationToken = default)
        {
            return await _adocaoService.AdotarAsync(
                cachorroId, 
                tutorId, 
                cancellationToken);
        }

        public Task<IEnumerable<string>> DevolverAdocaoAsync(
            Guid cachorroId, 
            CancellationToken cancellationToken = default)
        {
            return _adocaoService.DevolverAdocaoAsync(
                cachorroId, 
                cancellationToken);
        }
    }
}
