using DEPLOY.Cachorro.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
                options.UseInMemoryDatabase(databaseName: "Cachorro")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(Console.WriteLine, LogLevel.Debug)
                .EnableSensitiveDataLogging());
        }
    }
}