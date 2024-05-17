using Asp.Versioning;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AdocoesController : ControllerBase
    {
        private readonly IAdocaoAppService _adocaoAppService;

        public AdocoesController(
            IAdocaoAppService adocaoAppService)
        {
            _adocaoAppService = adocaoAppService;
        }

        [HttpPost("cachorro/{cachorroid}/tutor/{tutorid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Adotar um Cachorro",
            Tags = new[] { "Adocoes" },
            Description = "Operação para um tutor adotar um cachorro")]
        public async Task<IActionResult> AdotarAsync(
            [FromRoute] Guid cachorroid,
            [FromRoute] long tutorid,
            CancellationToken cancellationToken = default)
        {
            var item = await _adocaoAppService.AdotarAsync(
                cachorroid,
                tutorid,
                cancellationToken);

            if (item.Any())
            {
                return UnprocessableEntity(item);
            }

            return Ok();
        }

        [HttpPost("cachorro/{cachorroid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Devolver um cachorro que estava adotado",
            Tags = new[] { "Adocoes" },
            Description = "Operação para um tutor devolver um cachorro")]
        public async Task<IActionResult> DevolverAsync(
            [FromRoute] Guid cachorroid,
            CancellationToken cancellationToken = default)
        {
            var item = await _adocaoAppService.DevolverAdocaoAsync(
                cachorroid,
                cancellationToken);

            if (item.Any())
            {
                return UnprocessableEntity(item);
            }

            return Ok();
        }
    }
}
