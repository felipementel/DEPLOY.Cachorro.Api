using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace DEPLOY.Cachorro.Api.Extensions.Telemetria
{
    public static class AppInsightsExtensions
    {
        public static void AddTelemetria(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = configuration.GetSection("ConnectionsString:ApplicationInsights").Value;
            })
                .ConfigureTelemetryModule<QuickPulseTelemetryModule>(
                (module, o) =>
                module.AuthenticationApiKey = configuration.GetSection("ApplicationInsights:ApiKey").Value);
        }
    }
}
