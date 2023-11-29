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
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestsKeyVaultController : ControllerBase
    {
        private readonly SecretClient _secretClient;

        public TestsKeyVaultController(SecretClient secretClient)
        {
            _secretClient = secretClient;
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
                new Uri("https://kv-canaldeploy-dev.vault.azure.net/"),
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
