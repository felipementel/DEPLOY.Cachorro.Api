using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public static class TutorEndpoints
    {
        public static void MapTutorEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSetTutor = app
                .NewApiVersionSet("tutor")
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var tutores = app
                .MapGroup("/api/v{apiVersion:apiVersion}/tutores")
                .WithApiVersionSet(apiVersionSetTutor);

            tutores.MapGet("/", async (
                ITutorAppServices tutorAppService,
                CancellationToken cancellationToken) =>
            {
                async Task<IResult> ListAllAsync(
                    ITutorAppServices tutorAppService)
                {
                    var items = await tutorAppService.GetAllAsync(cancellationToken);

                    return items?.Count() > 0 ? TypedResults.Ok(items) : TypedResults.NoContent();
                }

                return await ListAllAsync(tutorAppService);
            })
                .Produces<IEnumerable<TutorDto>>(200, "application/json")
                .Produces(204)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "listall-tutores-get",
                    Summary = "Listar Tutores",
                    Description = "Operação para listar tutores",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores.MapGet("/{id:long}", async (
                [FromRoute] long id,
                ITutorAppServices tutorAppService,
                CancellationToken cancellationToken) =>
            {
                async Task<IResult> ListAllAsync(
                    ITutorAppServices tutorAppService)
                {
                    var items = await tutorAppService.GetByIdAsync(id, cancellationToken);

                    return items == null ? TypedResults.NoContent() : TypedResults.Ok(items);
                }

                return await ListAllAsync(tutorAppService);
            })
                .Produces<TutorDto>(200, "application/json")
                .Produces(404)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "getbyid-tutores-get",
                    Summary = "Obter Tutor",
                    Description = "Operação para obter tutor por id",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores
                .MapPost("/", async (
                    HttpRequest request,
                    TutorDto tutorDto,
                    ITutorAppServices tutorAppService,
                    CancellationToken cancellationToken) =>
                {
                    async Task<IResult> CreateTutorAsync()
                    {
                        var item = await tutorAppService.InsertAsync(tutorDto, cancellationToken);

                        if (item.Erros.Any())
                            return TypedResults.UnprocessableEntity(item.Erros);

                        var apiVersion = request.RouteValues["apiVersion"];

                        var scheme = request.Scheme;
                        var host = request.Host;
                        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/{apiVersion}/api/tutores/{item.Id}");

                        return Results.Created(location, item);
                    }

                    return await CreateTutorAsync();
                })
                .Accepts<TutorDto>("application/json")
                .Produces<TutorDto>(201, "application/json")
                .Produces(422)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "create-tutores-post",
                    Summary = "Cadastrar Tutor",
                    Description = "Operação para cadastrar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores.MapPut("/{id:long}", async (
                long id,
                TutorDto tutorDto,
                ITutorAppServices tutorAppService,
                CancellationToken cancellationToken) =>
            {
                async Task<IResult> UpdateTutorAsync()
                {
                    var item = await tutorAppService.UpdateAsync(id, tutorDto, cancellationToken);

                    if (item.Any())
                        return TypedResults.UnprocessableEntity(item);

                    return TypedResults.Ok(item);
                }

                return await UpdateTutorAsync();
            })
                .Accepts<TutorDto>("application/json")
                .Produces<TutorDto>(204, "application/json")
                .Produces(422)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "update-tutores-put",
                    Summary = "Atualizar Tutor",
                    Description = "Operação para atualizar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores.MapDelete("/{id:long}", async (
                long id,
                ITutorAppServices tutorAppService,
                CancellationToken cancellationToken) =>
            {
                async Task<IResult> DeleteTutorAsync()
                {
                    var item = await tutorAppService.DeleteAsync(id, cancellationToken);

                    return item ? TypedResults.NoContent() : TypedResults.NotFound();
                }

                return await DeleteTutorAsync();
            })
                .Produces(204)
                .Produces(404)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "delete-tutores-delete",
                    Summary = "Delete Tutor",
                    Description = "Operação para deletar tutor por id",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

        }
    }
}
