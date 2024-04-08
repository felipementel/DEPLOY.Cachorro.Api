using System.Threading;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface IAdocaoAppService
    {
        Task<IEnumerable<string>> AdotarAsync(
            Guid cachorroId,
            long tutorId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> DevolverAdocaoAsync(
            Guid cachorroId, 
            CancellationToken cancellationToken = default);
    }
}
