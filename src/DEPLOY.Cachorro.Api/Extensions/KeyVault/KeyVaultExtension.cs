using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Extensions.KeyVault
{
    [ExcludeFromCodeCoverage]
    public static class KeyVaultExtension
    {
        public static void AddKeyVaultExtension(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddSecretClient(
                                       configuration.GetSection("KeyVault"));

                clientBuilder.UseCredential(new DefaultAzureCredential());
            });
        }
    }
}
