using DEPLOY.Cachorro.Api.Extensions.AppConfiguration.Configs;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Extensions.AppConfiguration
{
    [ExcludeFromCodeCoverage]
    public static class AppConfigurationExtension
    { 
        public static void AddAppConfigurationExtension(
            this ConfigurationManager configurationManager,
            IServiceCollection services)
        {
            configurationManager.AddAzureAppConfiguration(options =>
            {
                options.Connect(configurationManager.GetSection("ConnectionStrings:AppConfiguration").Value)
                .Select(keyFilter: "CachorroApi:Settings:*", LabelFilter.Null)
                .ConfigureRefresh(refreshOptions =>
                {
                    refreshOptions
                    .Register("CachorroApi:Settings:*", refreshAll: true)
                    .SetCacheExpiration(TimeSpan.FromSeconds(5));
                });

                services.Configure<Settings>(configurationManager.GetSection("CachorroApi:Settings"));

                options.UseFeatureFlags(featureFlagOptions =>
                {
                    featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(5);
                });

                services.AddAzureAppConfiguration();

                services.AddFeatureManagement();
            });
        }
    }
}
