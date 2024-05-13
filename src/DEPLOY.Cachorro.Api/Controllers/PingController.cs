using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DEPLOY.Cachorro.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAsync()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
