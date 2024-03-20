using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AdocoesController : ControllerBase
    {
        private readonly IAdocaoAppService _adocaoAppService;

        public AdocoesController(IAdocaoAppService adocaoAppService)
        {
            _adocaoAppService = adocaoAppService;
        }

        [HttpPost("cachorro/{cachorroid}/tutor/{tutorid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Adotar um cachorro Cachorro",
            Tags = new[] { "Adocoes" },
            Description = "Operação para um tutor adotar um cachorro")]
        public async Task<IActionResult> AdotarAsync(
            [FromRoute] Guid cachorroid,
            [FromRoute] long tutorid,
            CancellationToken cancellationToken = default)
        {
            await _adocaoAppService.AdotarAsync(cachorroid, tutorid, cancellationToken);

            return Ok();
        }
    }
}
