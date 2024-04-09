namespace DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service
{
    public interface IAdocaoService
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
