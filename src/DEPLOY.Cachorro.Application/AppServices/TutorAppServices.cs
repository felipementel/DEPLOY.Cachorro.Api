using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;

namespace DEPLOY.Cachorro.Application.AppServices
{
    public class TutorAppServices : ITutorAppServices
    {
        private readonly ITutorService _tutorService;

        public TutorAppServices(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        public async Task<bool> DeleteAsync(long id,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.DeleteAsync(id);
        }

        public async Task<IEnumerable<TutorDto?>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var retorno = await _tutorService.GetAllAsync();

            return retorno.Select<Domain.Aggregates.Tutor.Entities.Tutor, TutorDto>(x => x!).ToList();
        }

        public async Task<TutorDto?> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.GetByIdAsync(id);
        }

        public async Task<TutorDto?> InsertAsync(
            TutorDto TutorDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _tutorService.CreateAsync(TutorDto);

            return item;
        }

        public async Task<bool> UpdateAsync(
            long id, 
            TutorDto TutorDto,
            CancellationToken cancellationToken = default)
        {
            return await _tutorService.UpdateAsync(id, TutorDto);
        }
    }
}
