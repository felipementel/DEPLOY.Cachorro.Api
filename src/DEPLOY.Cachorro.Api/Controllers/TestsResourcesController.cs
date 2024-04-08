using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DEPLOY.Cachorro.Api.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class TestsResourcesController : ControllerBase
    {
        private readonly SecretClient _secretClient;
        private readonly Settings _settings;
        private readonly IFeatureManager _featureManager;

        private readonly IConfiguration _configuration;

        public TestsResourcesController(
            SecretClient secretClient,
            IConfiguration configuration,
            IOptions<Settings> settings,
            IFeatureManager featureManager)
        {
            _secretClient = secretClient;
            _configuration = configuration;
            _settings = settings.Value;
            _featureManager = featureManager;
        }

        [HttpGet("keyvault/withoptions/{key}")]
        public async Task<IActionResult> GetAsync(string key)
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(3),
                    MaxRetries = 2,
                    Mode = RetryMode.Exponential
                 }
            };

            var client = new SecretClient(
                new Uri(_configuration.GetSection("KeyVault:VaultUri").Value),
                new DefaultAzureCredential(),
                options);

            KeyVaultSecret secret = await client.GetSecretAsync(name: key);

            string secretValue = secret.Value;

            return Ok(secretValue);
        }

        [HttpGet("keyvault/{key}")]
        public async Task<IActionResult> GetTest2Async(string key)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(name: key);

            string secretValue = secret.Value;

            return Ok(secretValue);
        }

        [HttpGet("featureflag")]
        public IActionResult GetAsync()
        {
            return Ok(_settings.ValorDaMensagem);
        }

        [HttpGet("featureflag/{featureflag}")]
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
