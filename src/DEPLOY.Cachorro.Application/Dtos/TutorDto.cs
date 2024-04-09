using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.Dtos
{
    [ExcludeFromCodeCoverage]
    public record TutorDto(
        long Id,
        string Nome,
        DateTime Cadastro,
        DateTime? Atualizacao,
        string CPF,
        [SwaggerSchema(ReadOnly = true)]
        IEnumerable<string> Erros = null) : BaseDto(Erros)
    {
        public static implicit operator Tutor(TutorDto dto) =>
            new Tutor(
                dto.Id,
                dto.Nome,
                dto.Cadastro,
                dto.Atualizacao,
                dto.CPF);

        public static implicit operator TutorDto?(Tutor? entity) =>
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
