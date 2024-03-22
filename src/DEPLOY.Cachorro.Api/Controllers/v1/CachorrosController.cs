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
    public class CachorrosController : ControllerBase
    {
        public readonly ICachorroAppServices _cachorroAppService;

        public CachorrosController(ICachorroAppServices cachorroAppService)
        {
            _cachorroAppService = cachorroAppService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros")]
        public async Task<IActionResult> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetAllAsync(cancellationToken);

            return items.Any() ? Ok(items) : NoContent();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CachorroDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Obter Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para obter de cachorro por id")]
        public async Task<IActionResult> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByIdAsync(id, cancellationToken);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cadastar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para cadastrar cachorro")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CachorroCreateDto  cachorroCreateDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _cachorroAppService.InsertAsync(cachorroCreateDto, cancellationToken);

            if (item.Erros.Any())
                return BadRequest(item.Erros);

            return CreatedAtAction("GetById",
                new { id = item.Id, version = new ApiVersion(1, 0).ToString() },
                item);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para atualizar de cachorro")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            CachorroDto cachorroDto,
            CancellationToken cancellationToken = default)
        {
            if (id != cachorroDto.Id)
            {
                return BadRequest();
            }

            return await _cachorroAppService.UpdateAsync(id, cachorroDto, cancellationToken)
           ? NoContent()
           : NotFound();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Excluir Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para excluir de cachorro")]
        public async Task<IActionResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var item = await _cachorroAppService.DeleteAsync(id, cancellationToken);

            if (!item)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
