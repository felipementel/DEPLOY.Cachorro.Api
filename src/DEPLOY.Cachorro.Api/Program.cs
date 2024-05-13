using DEPLOY.Cachorro.Api.Extensions.AppConfiguration;
using DEPLOY.Cachorro.Api.Extensions.Database;
using DEPLOY.Cachorro.Api.Extensions.KeyVault;
using DEPLOY.Cachorro.Api.Extensions.Swagger;
using DEPLOY.Cachorro.Api.Extensions.Telemetria;
using DEPLOY.Cachorro.Api.Extensions.Auth;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DEPLOY.Cachorro.Infra.CrossCutting;
namespace DEPLOY.Cachorro.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program() { }
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRegisterServices();

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.WriteIndented = true;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddRouting(opt =>
            {
                opt.LowercaseUrls = true;
                opt.LowercaseQueryStrings = true;
            });

            builder.Services.AddEndpointsApiExplorer();

            //Configure Extensions
            builder.Logging.AddLogExtension(builder.Configuration);
            builder.Services.AddAuthExtension(builder.Configuration);
            builder.Services.AddSwaggerExtension();
            builder.Services.AddDatabaseExtension(builder.Configuration);
            builder.Services.AddKeyVaultExtension(builder.Configuration);
            builder.Services.AddTelemetriaExtension(builder.Configuration);
            builder.Configuration.AddAppConfigurationExtension(builder.Services);

            var app = builder.Build();

            //Use Extensions
            app.UseSwaggerExtension();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseAzureAppConfiguration();

            await app.RunAsync();
        }
    }
}