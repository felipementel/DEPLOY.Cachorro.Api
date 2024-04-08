using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

            return items?.Count() > 0 ? Ok(items) : NoContent();
        }

        [HttpGet("adotados")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Lista de cachorro adotados",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros adotados")]
        public async Task<IActionResult> ListAllCachorrosAdotadosAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByKeyAsync(
                c => c.Tutor != null,
                cancellationToken);

            return items?.Count() > 0 ? Ok(items) : NoContent();
        }

        [HttpGet("paraadotar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CachorroDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Lista de cachorro disponíveis para adoção",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorros disponíveis para adoção")]
        public async Task<IActionResult> ListAllCachorrosParaAdocaoAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _cachorroAppService.GetByKeyAsync(
                c => c.Tutor == null,
                cancellationToken);

            return items?.Count > 0 ? Ok(items) : NoContent();
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cadastar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para cadastrar cachorro")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CachorroCreateDto cachorroCreateDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _cachorroAppService.InsertAsync(
                cachorroCreateDto,
                cancellationToken);

            if (item?.Erros.Count() > 0)
                return UnprocessableEntity(item.Erros);

            return CreatedAtAction("GetById",
                new
                {
                    id = item?.Id,
                    version = new ApiVersion(
                        1,
                        0)
                    .ToString()
                },
                item);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para atualizar de cachorro")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] CachorroDto cachorroDto,
            CancellationToken cancellationToken = default)
        {
            if (id != cachorroDto.Id)
            {
                return UnprocessableEntity();
            }

            var item = await _cachorroAppService.UpdateAsync(
                id,
                cachorroDto,
                cancellationToken);

            return !item.Any()
           ? NoContent()
           : UnprocessableEntity(item);
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
            var item = await _cachorroAppService.DeleteAsync(
                id,
                cancellationToken);

            if (!item)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
