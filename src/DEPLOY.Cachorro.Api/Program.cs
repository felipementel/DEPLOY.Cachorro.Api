using DEPLOY.Cachorro.Api.Extensions.Swagger;
using DEPLOY.Cachorro.Api.Extensions.Telemetria;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DEPLOY.Cachorro.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();            

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