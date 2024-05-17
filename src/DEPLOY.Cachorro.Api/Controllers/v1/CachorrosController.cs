using Asp.Versioning;
using DEPLOY.Cachorro.Application.AppServices;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CachorrosController : ControllerBase
    {
        public readonly ICachorroAppServices _cachorroAppService;

        public CachorrosController(ICachorroAppServices cachorroAppService)
        {
            _cachorroAppService = cachorroAppService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar Cachorros",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros")]
        public async Task<IActionResult> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetAllAsync(cancellationToken);

            return items.Any() ? Ok(items) : NoContent();
        }

        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CachorroDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Obter Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para obter cachorro por id")]
        public async Task<IActionResult> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByIdAsync(
                id,
                cancellationToken);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CachorroDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cadastrar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para cadastrar cachorro")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CachorroCreateDto cachorroCreateDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _cachorroAppService.InsertAsync(
                cachorroCreateDto,
                cancellationToken);

            if (item.Erros.Any())
                return UnprocessableEntity(item.Erros);

            return CreatedAtAction("GetById",
                new
                {
                    id = item.Id,
                    version = new ApiVersion(
                        1,
                        0)
                    .ToString()
                },
                item);
        }

        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Atualizar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para atualizar de cachorro")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] Guid id,
            [FromBody] CachorroDto cachorroDto,
            CancellationToken cancellationToken = default)
        {
            if (id != cachorroDto.Id)
            {
                return UnprocessableEntity();
            }

            var retorned = await _cachorroAppService.UpdateAsync(
                id,
                cachorroDto,
                cancellationToken);

            return !retorned.Any() ? NoContent()
             : UnprocessableEntity(retorned);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Excluir Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para excluir cachorro")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var item = await _cachorroAppService.DeleteAsync(
                id,
                cancellationToken);

            if (!item)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("adotados")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Lista de cachorro adotados",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros adotados")]
        public async Task<IActionResult> ListAllCachorrosAdotadosAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByKeyAsync(
                c => c.Adotado,
                cancellationToken);

            return items.Any() ? Ok(items) : NoContent();
        }

        [HttpGet("para-adotar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Lista de cachorro disponíveis para adoção",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros disponíveis para adoção")]
        public async Task<IActionResult> ListAllCachorrosParaAdocaoAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByKeyAsync(
                c => !c.Adotado,
                cancellationToken);

            return items.Count > 0 ? Ok(items) : NoContent();
        }
    }
}
