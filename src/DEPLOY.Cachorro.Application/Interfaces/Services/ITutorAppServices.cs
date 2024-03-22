using DEPLOY.Cachorro.Application.Dtos;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface ITutorAppServices
    {
        public Task<TutorDto> InsertAsync(
            TutorDto tutorDto,
            CancellationToken cancellationToken = default);

        public Task<bool> UpdateAsync(
            long id, 
            TutorDto tutorDto,
            CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<TutorDto?>> GetAllAsync(
            CancellationToken cancellationToken = default);

        public Task<TutorDto?> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
