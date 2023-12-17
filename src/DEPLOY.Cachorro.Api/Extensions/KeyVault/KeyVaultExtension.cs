using Azure.Identity;
using Microsoft.Extensions.Azure;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Extensions.KeyVault
{
    [ExcludeFromCodeCoverage]
    public static class KeyVaultExtension
    {
        public static IServiceCollection AddKeyVaultExtension(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddSecretClient(
                                       configuration.GetSection("KeyVault"));

                clientBuilder.UseCredential(new DefaultAzureCredential());
            });

            return services;
        }
    }
}
