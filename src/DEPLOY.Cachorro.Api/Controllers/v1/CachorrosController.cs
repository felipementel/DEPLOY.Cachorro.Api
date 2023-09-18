using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CachorrosController : ControllerBase
    {
        public readonly CachorroContext _context;

        public CachorrosController(CachorroContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DEPLOY.Cachorro.Domain.Cachorro>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        public async Task<IActionResult> ObterPorIdAsync(int id)
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
        public async Task<IActionResult> CadastrarCachorroAsync(
            [FromBody] DEPLOY.Cachorro.Domain.Cachorro cachorro)
        {
            _context.Cachorros.Add(cachorro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObterPorId",
                new { id = cachorro.Id, version = new ApiVersion(1, 0).ToString() },
                cachorro);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirCachorroAsync(int id)
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
