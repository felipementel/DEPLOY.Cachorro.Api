using System.Threading;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface IAdocaoAppService
    {
        Task AdotarAsync(Guid cachorroId, long TutorId, CancellationToken cancellationToken = default);

        Task DevolverAdocaoAsync(Guid cachorroId, long TutorId, CancellationToken cancellationToken = default);
    }
}
