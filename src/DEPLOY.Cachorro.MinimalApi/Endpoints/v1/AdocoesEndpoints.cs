using Asp.Versioning;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
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
                .RequireAuthorization()
                .WithApiVersionSet(apiVersionSetAdocoes);

            adocoes
                .MapPost("/cachorro/{cachorroid}/tutor/{tutorid}", AdotarAsync)
                .Produces(201)
                .Produces(401)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "adotar-adocoes-post",
                    Summary = "Adotar um Cachorro",
                    Description = "Operação para um tutor adotar um cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Adocoes" } }
                });

            adocoes
                .MapPost("/cachorro/{cachorroid}", DevolverAsync)
                .Produces(201)
                .Produces(422)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "devolver-adocoes-post",
                    Summary = "Devolver um cachorro que estava adotado",
                    Description = "Operação para um tutor devolver um cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Adocoes" } }
                });

            async Task<IResult> DevolverAsync(
                Guid cachorroid,
                IAdocaoAppService adocaoAppService,
                CancellationToken cancellationToken = default)
            {
                var item = await adocaoAppService.DevolverAdocaoAsync(
                    cachorroid,
                    cancellationToken);

                if (item?.Count() > 0)
                {
                    return Results.UnprocessableEntity(item);
                }

                return TypedResults.Ok();
            }

            async Task<IResult> AdotarAsync(
                Guid cachorroid,
                long tutorid,
                IAdocaoAppService adocaoAppService,
                CancellationToken cancellationToken = default)
            {
                var item = await adocaoAppService.AdotarAsync(
                    cachorroid,
                    tutorid,
                    cancellationToken);

                if (item?.Count() > 0)
                {
                    return Results.UnprocessableEntity(item);
                }

                return TypedResults.Ok();
            }
        }
    }
}
