using DEPLOY.Cachorro.Api.Extensions.Swagger;
using DEPLOY.Cachorro.Api.Extensions.Telemetria;
using DEPLOY.Cachorro.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DEPLOY.Cachorro.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<CachorroContext>(options =>
            {
                options.UseInMemoryDatabase("Cachorros");
            });

            //Extensions
            builder.Logging.AddLogExtensions(builder.Configuration);
            builder.Services.AddTelemetria(builder.Configuration);
            builder.Services.AddSwagger();

            var app = builder.Build();

            //Extensions
            app.UseSwaggerDEPLOY();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}