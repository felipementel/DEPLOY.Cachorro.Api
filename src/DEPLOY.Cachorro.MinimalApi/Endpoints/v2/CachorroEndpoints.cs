using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints.v2
{
    [ExcludeFromCodeCoverage]
    public static class CachorroEndpoints
    {
        public static void MapCachorroEndpointsV2(this IEndpointRouteBuilder app)
        {
            var apiVersionSetCachorro = app
                .NewApiVersionSet("cachorrosv2")
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();

            var cachorros = app
                .MapGroup("/api/v{apiVersion:apiVersion}/cachorros")
                //.RequireAuthorization()
                .WithApiVersionSet(apiVersionSetCachorro);

            cachorros
                .MapGet("/", () =>
                {
                    TypedResults.Ok();
                })
                .Produces<string>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListAllCachorrosAsync",
                    Summary = "List Cachorro",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });
        }
    }
}