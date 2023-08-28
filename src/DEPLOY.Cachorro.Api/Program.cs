
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace DEPLOY.Cachorro.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                {
                    config.ConnectionString = builder.Configuration.GetSection("ConnectionsString:ApplicationInsights").Value;
                },
                configureApplicationInsightsLoggerOptions: (options) => { });

            builder.Services.AddApplicationInsightsTelemetry(options => 
            {
                options.ConnectionString = builder.Configuration.GetSection("ConnectionsString:ApplicationInsights").Value; 
            })
                .ConfigureTelemetryModule<QuickPulseTelemetryModule>(
                (module, o) => 
                module.AuthenticationApiKey = builder.Configuration.GetSection("ApplicationInsights:Api-Key").Value);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}