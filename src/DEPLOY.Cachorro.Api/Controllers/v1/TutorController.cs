using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly CachorroDbContext _cachorroDbContext;

        public TutorController(CachorroDbContext cachorroDbContext)
        {
            _cachorroDbContext = cachorroDbContext;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Domain.Tutor>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarTutores()
        {
            var items = await _cachorroDbContext.Tutors.ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Domain.Tutor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorIdAsync(int id)
        {
            var items = await _cachorroDbContext.Tutors.FindAsync(id);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Domain.Tutor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CadastrarTutorAsync(
            [FromBody] Domain.Tutor  tutor)
        {
            _cachorroDbContext.Tutors.Add(tutor);
            await _cachorroDbContext.SaveChangesAsync();

            return CreatedAtAction("ObterPorId",
                new { id = tutor.Id, version = new ApiVersion(1, 0).ToString() },
                tutor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTutorAsync(
            long id,
            Domain.Tutor tutor)
        {
            if (id != tutor.Id)
            {
                return BadRequest();
            }

            _cachorroDbContext.Entry(tutor).State = EntityState.Modified;

            await _cachorroDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirTutorAsync(int id)
        {
            var item = await _cachorroDbContext.Tutors.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _cachorroDbContext.Tutors.Remove(item);
            await _cachorroDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
