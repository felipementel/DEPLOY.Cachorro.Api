using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities;

namespace DEPLOY.Cachorro.Application.Dtos
{
    public record TutorDto(
        long Id,
        string Nome,
        DateTime Cadastro,
        DateTime? Atualizacao,
        long CPF,
        IEnumerable<String> Erros)
    {
        public static implicit operator Tutor(TutorDto dto) =>
            new Domain.Aggregates.Tutor.Entities.Tutor (
                dto.Id,
                dto.Nome,
                dto.Cadastro,
                dto.Atualizacao,
                dto.CPF);

        public static implicit operator TutorDto?(Tutor entity) =>
            entity == null ? null :
            new TutorDto(
                entity.Id,
                entity.Nome,
                entity.Cadastro,
                entity.Atualizacao,
                entity.CPF,
                entity.Erros);
    }
}
