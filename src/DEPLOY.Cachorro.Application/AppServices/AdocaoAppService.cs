using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service;

namespace DEPLOY.Cachorro.Application.AppServices
{
    public class AdocaoAppService : IAdocaoAppService
    {
        private readonly IAdocaoService _adocaoService;

        public AdocaoAppService(IAdocaoService adocaoService)
        {
            _adocaoService = adocaoService;
        }

        //https://github.com/altmann/FluentResults
        public async Task AdotarAsync(Guid cachorroId, long tutorId, CancellationToken cancellationToken = default)
        {
            await _adocaoService.AdotarAsync(cachorroId, tutorId, cancellationToken);
        }

        public Task DevolverAdocaoAsync(Guid cachorroId, long TutorId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
