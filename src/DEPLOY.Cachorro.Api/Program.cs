using DEPLOY.Cachorro.Api.Extensions.Database;
using DEPLOY.Cachorro.Api.Extensions.Swagger;
using DEPLOY.Cachorro.Api.Extensions.Telemetria;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DEPLOY.Cachorro.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            builder.Services.AddEndpointsApiExplorer();

            //Extensions
            builder.Logging.AddLogExtension(builder.Configuration);
            builder.Services.AddDatabaseExtension(builder.Configuration);
            builder.Services.AddTelemetriaExtension(builder.Configuration);
            builder.Services.AddSwaggerExtension();

            var app = builder.Build();

            //Extensions
            app.UseSwaggerExtension();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}