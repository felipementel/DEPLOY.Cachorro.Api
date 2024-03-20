using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.ValueObject;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Application.Dtos
{
    [ExcludeFromCodeCoverage]
    public record CachorroCreateDto(
               string Nome,
               DateTime Nascimento,
               PELAGEM Pelagem,
               float Peso)
    {
        public static implicit operator Domain.Aggregates.Cachorro.Entities.Cachorro(CachorroCreateDto dto) =>
            new Domain.Aggregates.Cachorro.Entities.Cachorro(
                dto.Nome,
                dto.Nascimento,
                dto.Pelagem,
                dto.Peso);
    }
}
