using DEPLOY.Cachorro.Api.Extensions.Database;
using DEPLOY.Cachorro.Api.Extensions.Swagger;
using DEPLOY.Cachorro.Infra.CrossCutting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
            builder.Services.AddSwaggerExtension();
            builder.Services.AddDatabaseExtension(builder.Configuration);

            var app = builder.Build();

            //Use Extensions
            app.UseSwaggerExtension();

            app.UseHttpsRedirection();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}