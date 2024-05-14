using Asp.Versioning;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.MinimalApi.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints
{
    public static class TestResourcesEndpoints
    {
        public static void MapTestResourcesEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSetTestResources = app
                .NewApiVersionSet("testresources")
                .HasApiVersion(new ApiVersion(1, 0))
                .Build();

            var testResources = app
                .MapGroup("/api/testresources")
                .ExcludeFromDescription()
                .WithApiVersionSet(apiVersionSetTestResources);

            testResources
                .MapGet("/keyvault/withoptions/{key}", async (
                    string key,
                    SecretClient secretClient,
                    IConfiguration configuration,
                    CancellationToken cancellationToken) =>
                {
                    async Task<IResult> GetKeyAsync(
                        string key)
                    {
                        SecretClientOptions options = new SecretClientOptions()
                        {
                            Retry =
                            {
                                Delay= TimeSpan.FromSeconds(2),
                                MaxDelay = TimeSpan.FromSeconds(3),
                                MaxRetries = 2,
                                Mode = RetryMode.Exponential
                            }
                        };

                        var client = new SecretClient(
                                                       new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
                                                       new DefaultAzureCredential(),
                                                       options);

                        KeyVaultSecret secret = await client.GetSecretAsync(name: key);

                        string secretValue = secret.Value;

                        return TypedResults.Ok(secretValue);
                    }

                    return await GetKeyAsync(key);
                })
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetAsync",
                    Summary = "Get Secret",
                    Description = "Get secret from KeyVault",
                    Tags = new List<OpenApiTag> { new() { Name = "KeyVault" } }
                });

            testResources
                .MapGet("/keyvault/{key}", async (
                    string key,
                    SecretClient secretClient,
                    CancellationToken cancellationToken) =>
                {
                    async Task<IResult> GetTest2Async(
                        string key
                        )
                    {
                        KeyVaultSecret secret = await secretClient.GetSecretAsync(name: key);

                        string secretValue = secret.Value;

                        return TypedResults.Ok(secretValue);
                    }

                    return await GetTest2Async(key);
                })
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetTest2Async",
                    Summary = "Get Secret",
                    Description = "Get secret from KeyVault",
                    Tags = new List<OpenApiTag> { new() { Name = "KeyVault" } }
                });


            testResources
                .MapGet("/featureflag", async (
                    IOptions<Settings> settings,
                    CancellationToken cancellationToken) =>
                {
                    async Task<Ok<string>> GetAsync()
                    {
                        return TypedResults.Ok(settings.Value.ValorDaMensagem);
                    }

                    return await GetAsync();
                })
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetAsync",
                    Summary = "Get Feature Flag",
                    Description = "Get feature flag value",
                    Tags = new List<OpenApiTag> { new() { Name = "FeatureFlag" } }
                });

            testResources
                .MapGet("/featureflag/{featureflag}", async (
                    string featureflag,
                    IFeatureManager featureManager,
                    CancellationToken cancellationToken) =>
                {
                    async Task<IResult> GetAsync(string featureflag)
                    {
                        var IsEnable = await featureManager.IsEnabledAsync(featureflag);

                        if (IsEnable)
                            return TypedResults.Ok("Sistema no ar.");
                        else
                            return TypedResults.BadRequest("Sistema fora do ar");
                    }

                    return await GetAsync(featureflag);
                })
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetAsync",
                    Summary = "Get Feature Flag",
                    Description = "Get feature flag value",
                    Tags = new List<OpenApiTag> { new() { Name = "FeatureFlag" } }
                });

        }
    }
}
