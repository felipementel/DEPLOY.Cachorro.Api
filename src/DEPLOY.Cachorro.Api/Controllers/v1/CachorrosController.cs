using DEPLOY.Cachorro.Repository;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            var items = _context.Cachorros.ToList();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult CadastrarCachorro(
            [FromBody] DEPLOY.Cachorro.Domain.Cachorro cachorro)
        {
            _context.Cachorros.Add(cachorro);

            return Ok();
        }
    }
}
