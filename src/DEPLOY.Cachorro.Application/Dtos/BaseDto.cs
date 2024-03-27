using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DEPLOY.Cachorro.Application.Dtos
{
    [ExcludeFromCodeCoverage]
    public record BaseDto
    {
        public BaseDto(IEnumerable<string> erros)
        {
            Erros = erros;
        }

        [JsonIgnore]
        public IEnumerable<string> Erros { get; set; }
    }
}
