using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.AppServices
{
    [ExcludeFromCodeCoverage]
    public class TutorAppServices : ITutorAppServices
    {
        private readonly ITutorService _tutorService;

        public TutorAppServices(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        public async Task<bool> DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.DeleteAsync(
                id,
                cancellationToken);
        }

        public async Task<IEnumerable<TutorDto?>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var retorno = await _tutorService.GetAllAsync(
                cancellationToken);

            return retorno
                .Select<Domain.Aggregates.Tutor.Entities.Tutor, TutorDto>(x => x!).ToList();
        }

        public async Task<TutorDto?> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.GetByIdAsync(id,
                cancellationToken);
        }

        public async Task<TutorDto> InsertAsync(
            TutorDto tutorDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _tutorService.CreateAsync(
                tutorDto, 
                cancellationToken);

            return item;
        }

        public async Task<IEnumerable<string>> UpdateAsync(
            long id, 
            TutorDto tutorDto,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.UpdateAsync(
                id,
                tutorDto, 
                cancellationToken);
        }
    }
}
