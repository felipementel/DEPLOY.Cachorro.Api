using Asp.Versioning;
using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CachorrosController : ControllerBase
    {
        public readonly CachorroDbContext _context;

        public CachorrosController(CachorroDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DEPLOY.Cachorro.Domain.Cachorro>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para listar de cachorro")]
        public async Task<IActionResult> ListarAsync()
        {
            var items = await _context.Cachorros.ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DEPLOY.Cachorro.Domain.Cachorro), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Obter Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para obter de cachorro por id")]
        public async Task<IActionResult> ObterPorIdAsync(Guid id)
        {
            var items = await _context.Cachorros.FindAsync(id);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DEPLOY.Cachorro.Domain.Cachorro), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cadastar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para cadastrar de cachorro")]
        public async Task<IActionResult> CadastrarCachorroAsync(
            [FromBody] DEPLOY.Cachorro.Domain.Cachorro cachorro)
        {
            _context.Cachorros.Add(cachorro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObterPorId",
                new { id = cachorro.Id, version = new ApiVersion(1, 0).ToString() },
                cachorro);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar Cachorro",
            Tags = new[] { "Cachorros" },
            Description = "Operação para atualizar de cachorro")]
        public async Task<IActionResult> PutCachorroAsync(
            Guid id,
            Domain.Cachorro cachorro)
        {
            if (id != cachorro.Id)
            {
                return BadRequest();
            }

            _context.Entry(cachorro).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
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
        public async Task<IActionResult> ExcluirCachorroAsync(Guid id)
        {
            var item = await _context.Cachorros.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Cachorros.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
