using Asp.Versioning;
using DEPLOY.Cachorro.Application.AppServices;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Entities;
using Microsoft.OpenApi.Models;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints.v1
{
    public static class AdocoesEndpoints
    {
        public static void MapAdocoesEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSetAdocoes = app
                .NewApiVersionSet("adocoes")
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var adocoes = app
                .MapGroup("/api/v{apiVersion:apiVersion}/adocoes")
                //.RequireAuthorization()
                .WithApiVersionSet(apiVersionSetAdocoes);

            adocoes
                .MapPost("/cachorro/{cachorroid}/tutor/{tutorid}", AdotarAsync)
                .Produces(201)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "AdotarAsync",
                    Summary = "Adotar um Cachorro",
                    Description = "Operação para um tutor adotar um cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Adocoes" } }
                });

            adocoes
                .MapPost("/cachorro/{cachorroid}", DevolverAsync)
                .Produces(201)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DevolverAsync",
                    Summary = "Devolver um cachorro que estava adotado",
                    Description = "Operação para um tutor devolver um cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Adocoes" } }
                });

            async Task<IResult> DevolverAsync(
                Guid cachorroid, 
                long tutorid, 
                HttpContext context,
                IAdocaoAppService adocaoAppService,
                CancellationToken cancellationToken = default)
            {
                var item = await adocaoAppService.AdotarAsync(cachorroid, tutorid, cancellationToken);

                if (item?.Count() > 0)
                {
                    return TypedResults.UnprocessableEntity(item);
                }

                return TypedResults.Ok();
            }

            async Task<IResult> AdotarAsync(
                Guid cachorroid, 
                HttpContext context,
                IAdocaoAppService adocaoAppService, 
                CancellationToken cancellationToken = default)
            {
                var item = await adocaoAppService.DevolverAdocaoAsync(
                cachorroid,
                cancellationToken);

                if (item?.Count() > 0)
                {
                    return TypedResults.UnprocessableEntity(item);
                }

                return TypedResults.Ok();
            }
        }        
    }
}
