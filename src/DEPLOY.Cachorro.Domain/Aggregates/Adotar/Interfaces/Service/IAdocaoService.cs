namespace DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service
{
    public interface IAdocaoService
    {
        Task AdotarAsync(Guid cachorroId, long TutorId, CancellationToken cancellationToken = default);

        Task DevolverAdocaoAsync(Guid cachorroId, long TutorId, CancellationToken cancellationToken = default);
    }
}
