using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.MinimalApi.Extensions.Auth
{
    [ExcludeFromCodeCoverage]
    public static class AuthExtension
    {
        public static void AddAuthExtension(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));
        }
    }
}
