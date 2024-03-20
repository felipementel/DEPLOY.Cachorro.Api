using Asp.Versioning;
using DEPLOY.Cachorro.Api.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AppConfigurationController : ControllerBase
    {
        public readonly Settings _settings;

        private readonly IFeatureManager _featureManager;

        public AppConfigurationController(
            IOptions<Settings> settings,
            IFeatureManager featureManager)
        {
            _settings = settings.Value;
            _featureManager = featureManager;
        }

        [HttpGet]
        public IActionResult GetAsync()
        {
            return Ok(_settings.ValorDaMensagem);
        }

        [HttpGet("/featureflag/{featureflag}")]
        public async Task<IActionResult> Get2FeatureFlagAsync(string featureflag)
        {
            var IsEnable = await _featureManager.IsEnabledAsync(featureflag);

            if (IsEnable)
                return Ok("Sistema no ar.");
            else
                return BadRequest("Sistema fora do ar");
        }
    }
}
