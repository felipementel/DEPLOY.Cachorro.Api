using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DEPLOY.Cachorro.Api.Controllers.v2
{    
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CachorrosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
