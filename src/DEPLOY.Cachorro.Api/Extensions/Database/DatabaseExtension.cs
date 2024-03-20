using DEPLOY.Cachorro.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Extensions.Database
{
    [ExcludeFromCodeCoverage]
    public static class DatabaseExtension
    {
        public static void AddDatabaseExtension(
            this IServiceCollection service,
            ConfigurationManager configuration)
        {
            service.AddDbContext<CachorroDbContext>(options =>
                options.UseSqlServer(
                   configuration.GetSection("ConnectionStrings:DefaultConnection").Value,
                    p =>
                    {
                        p.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null);

                        p.MigrationsHistoryTable("_ControleMigracoes", "dbo");

                    })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(Console.WriteLine, LogLevel.Debug)
                .EnableSensitiveDataLogging());
        }
    }
}