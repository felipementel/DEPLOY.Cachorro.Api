using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

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
                .RequireAuthorization()
                .WithApiVersionSet(apiVersionSetTutor);

            tutores.MapGet("/{id}", async (
                int id,
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
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListByIdTutores",
                    Summary = "List Tutores",
                    Description = "Operação para listar de tutores",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });


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
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListByIdTutores2",
                    Summary = "List Tutores",
                    Description = "Operação para listar de tutores",
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
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreateTutor",
                    Summary = "Cadastar Tutor",
                    Description = "Operação para cadastrar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores.MapDelete("/{id}", async (
                int id,
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
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DeleteTutor",
                    Summary = "Delete Tutor",
                    Description = "Operação para deletar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            tutores.MapPut("/{id}", async (
                int id,
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
                .Produces<TutorDto>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "UpdateTutor",
                    Summary = "Update Tutor",
                    Description = "Operação para atualizar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

        }
    }
}
