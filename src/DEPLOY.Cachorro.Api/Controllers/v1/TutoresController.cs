using Asp.Versioning;
using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TutoresController : ControllerBase
    {
        private readonly CachorroDbContext _cachorroDbContext;

        public TutoresController(CachorroDbContext cachorroDbContext)
        {
            _cachorroDbContext = cachorroDbContext;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Domain.Tutor>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar Tutor",
            Description = "Operação para listar do tutor",
            Tags = new[] { "Tutor", "Get" })]
        public async Task<IActionResult> ListarAsync()
        {
            var items = await _cachorroDbContext.Tutores.ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Domain.Tutor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Obter Tutor",
            Description = "Operação para obter  tutor por id",
            Tags = new[] { "Tutor", "Get" })]
        public async Task<IActionResult> ObterPorIdAsync(long id)
        {
            var items = await _cachorroDbContext.Tutores.FindAsync(id);

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
        [SwaggerOperation(
            Summary = "Cadastrar Tutor",
            Description = "Operação para cadastrar do tutor",
            Tags = new[] { "Tutor", "Post" })]
        public async Task<IActionResult> CadastrarTutorAsync(
            [FromBody] Domain.Tutor tutor)
        {
            _cachorroDbContext.Tutores.Add(tutor);
            await _cachorroDbContext.SaveChangesAsync();

            return CreatedAtAction("ObterPorId",
                new { id = tutor.Id, version = new ApiVersion(1, 0).ToString() },
                tutor);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar Tutor",
            Description = "Operação para atualizar do tutor",
            Tags = new[] { "Tutor", "Put" })]
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
        [SwaggerOperation(
            Summary = "Excluir Tutor",
            Description = "Operação para excluir do tutor por id",
            Tags = new[] { "Tutor", "Delete" })]
        public async Task<IActionResult> ExcluirTutorAsync(long id)
        {
            var item = await _cachorroDbContext.Tutores.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _cachorroDbContext.Tutores.Remove(item);
            await _cachorroDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
