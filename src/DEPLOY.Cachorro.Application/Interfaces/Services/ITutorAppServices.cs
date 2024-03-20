using DEPLOY.Cachorro.Application.Dtos;

namespace DEPLOY.Cachorro.Application.Interfaces.Services
{
    public interface ITutorAppServices
    {
        public Task<TutorDto> InsertAsync(TutorDto tutorDto);

        public Task<bool> UpdateAsync(long id, TutorDto tutorDto);

        public Task<bool> DeleteAsync(long id);

        public Task<IEnumerable<TutorDto?>> GetAllAsync();

        public Task<TutorDto?> GetByIdAsync(long id);
    }
}
