using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using System.Collections.Frozen;

namespace DEPLOY.Cachorro.Domain.Aggregates.Adotar.Services
{
    public class AdocaoService : IAdocaoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICachorroRepository _cachorroRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IList<string> _validation;

        public AdocaoService(
            IUnitOfWork uow,
            ICachorroRepository cachorroRepository,
            ITutorRepository tutorRepository)
        {
            _uow = uow;
            _cachorroRepository = cachorroRepository;
            _tutorRepository = tutorRepository;
            _validation = new List<string>(3);
        }

        public async Task<IEnumerable<string>> AdotarAsync(
            Guid cachorroId,
            long tutorId,
            CancellationToken cancellationToken = default)
        {
            var existedCachorro = await _cachorroRepository.GetByIdAsync(cachorroId, cancellationToken);

            if (existedCachorro is null)
            {
                _validation.Add("Cachorro não encontrado");
            }
            else if (existedCachorro.Tutor is not null)
            {
                _validation.Add("Cachorro já adotado");
            }

            var existedTutor = await _tutorRepository.GetByIdAsync(tutorId, cancellationToken);

            if (existedTutor is null)
            {
                _validation.Add("Tutor não encontrado");
            }

            if (_validation.Count > 0)
            {
                return _validation;
            }

            existedCachorro?.Adotar(existedTutor);

            await _cachorroRepository.UpdateAsync(existedCachorro);

            await _uow.CommitAndSaveChangesAsync(cancellationToken);

            return _validation;
        }


        public async Task<IEnumerable<string>> DevolverAdocaoAsync(
            Guid cachorroId,
            CancellationToken cancellationToken = default)
        {
            var existedCachorro = await _cachorroRepository.GetByIdAsync(cachorroId, cancellationToken);

            if (existedCachorro is null)
            {
                _validation.Add("Cachorro não encontrado");
            }
            else if (existedCachorro.Tutor is null)
            {
                _validation.Add("Cachorro não consta como adotado no sistema");
            }

            if (_validation?.Count() > 0)
            {
                return _validation.ToFrozenSet();
            }

            existedCachorro.Devolver();

            await _cachorroRepository.UpdateAsync(existedCachorro, cancellationToken);

            await _uow.CommitAndSaveChangesAsync(cancellationToken);

            return _validation;
        }
    }
}
