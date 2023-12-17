using Asp.Versioning;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class TestsKeyVaultController : ControllerBase
    {
        private readonly SecretClient _secretClient;

        private readonly IConfiguration _configuration;

        public TestsKeyVaultController(
            SecretClient secretClient,
            IConfiguration configuration)
        {
            _secretClient = secretClient;
            _configuration = configuration;
        }

        [HttpGet("{key}")]
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

        [HttpGet("test2/{key}")]
        public async Task<IActionResult> GetTest2Async(string key)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(name: key);

            string secretValue = secret.Value;

            return Ok(secretValue);
        }
    }
}
