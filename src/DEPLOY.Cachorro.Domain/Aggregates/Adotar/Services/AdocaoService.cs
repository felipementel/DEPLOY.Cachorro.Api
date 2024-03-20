using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;

namespace DEPLOY.Cachorro.Domain.Aggregates.Adotar.Services
{
    public class AdocaoService : IAdocaoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICachorroRepository _cachorroRepository;
        private readonly ITutorRepository _tutorRepository;

        public AdocaoService(
            IUnitOfWork uow,
            ICachorroRepository cachorroRepository,
            ITutorRepository tutorRepository)
        {
            _uow = uow;
            _cachorroRepository = cachorroRepository;
            _tutorRepository = tutorRepository;
        }

        //https://github.com/altmann/FluentResults
        public async Task AdotarAsync(
            Guid cachorroId,
            long tutorId,
            CancellationToken cancellationToken = default)
        {
            var existedCachorro = await _cachorroRepository.GetByIdAsync(cachorroId, cancellationToken);

            var existedTutor = await _tutorRepository.GetByIdAsync(tutorId, cancellationToken);

            existedCachorro.Adotar(existedTutor);

            await _cachorroRepository.UpdateAsync(existedCachorro);

            await _uow.CommitAndSaveChangesAsync();
        }

        public Task DevolverAdocaoAsync(
            Guid cachorroId,
            long TutorId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
