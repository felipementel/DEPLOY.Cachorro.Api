using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.ValueObject;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.Dtos
{
    [ExcludeFromCodeCoverage]
    public record CachorroDto(
        Guid Id,
        string Nome,
        DateTime Cadastro,
        DateTime? Atualizacao,
        DateTime Nascimento,
        bool Adotado,
        PELAGEM Pelagem,
        float Peso,
        TutorDto? Tutor,
        [SwaggerSchema(ReadOnly = true)]
        IEnumerable<string> Erros) : BaseDto(Erros)
    {
        public static implicit operator Domain.Aggregates.Cachorro.Entities.Cachorro(CachorroDto dto) =>
            new Domain.Aggregates.Cachorro.Entities.Cachorro(
                dto.Id,
                dto.Nome,
                dto.Cadastro,
                dto.Atualizacao,
                dto.Nascimento,
                dto.Adotado,
                dto.Pelagem,
                dto.Peso);

        public static implicit operator CachorroDto?(Domain.Aggregates.Cachorro.Entities.Cachorro? entity) =>
            entity == null ? null :
            new CachorroDto(
                entity.Id,
                entity.Nome,
                entity.Cadastro,
                entity.Atualizacao,
                entity.Nascimento,
                entity.Adotado,
                entity.Pelagem,
                entity.Peso,
                entity.Tutor,
                entity.Erros);
    }
}
