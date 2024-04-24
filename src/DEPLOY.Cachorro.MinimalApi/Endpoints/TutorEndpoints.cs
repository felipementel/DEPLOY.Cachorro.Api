using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.OpenApi.Models;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints
{
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
                //.RequireAuthorization()
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
                //.RequireAuthorization()
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
                    //ApiVersion version,
                    TutorDto tutorDto,
                    ITutorAppServices tutorAppService,
                    CancellationToken cancellationToken) =>
                {
                    var item = await tutorAppService.InsertAsync(tutorDto, cancellationToken);

                    //if (item.Erros.Any())
                    //    return TypedResults.UnprocessableEntity(item.Erros);

                    //var scheme = request.Scheme;
                    //var host = request.Host;
                    //var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/v{version}/api/tutores/{item.Id}");

                    //return Results.Created(location, item);

                    //return TypedResults.Created(location, item);
                })
                .Accepts<TutorDto>("application/json")
                .Produces<TutorDto>(201, "application/json")
                //.RequireAuthorization()
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreateTutor",
                    Summary = "Cadastar Tutor",
                    Description = "Operação para cadastrar tutor",
                    Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
                });

            //app.MapPost("/tutores", async (
            //    HttpRequest request,
            //    ApiVersion version,
            //    TutorDto tutorDto,
            //    ITutorAppServices tutorAppService,
            //    CancellationToken cancellationToken) =>
            //{
            //    async Task<IResult> CreateTutorAsync()
            //    {
            //        var item = await tutorAppService.InsertAsync(tutorDto, cancellationToken);

            //        if (item.Erros.Any())
            //            return TypedResults.UnprocessableEntity(item.Erros);

            //        var scheme = request.Scheme;
            //        var host = request.Host;
            //        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/v{version}/api/cachorros/{item.Id}");

            //        return Results.Created(location, item);
            //    }

            //    //return await CreateTutorAsync(tutorDto, tutorAppService);
            //})
            //    .Accepts<TutorDto>("application/json")
            //    .Produces<TutorDto>(201, "application/json")
            //    //.RequireAuthorization()
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "CreateTutor3",
            //        Summary = "Create Tutor",
            //        Description = "Operação para criar tutor",
            //        Tags = new List<OpenApiTag> { new() { Name = "Tutores" } }
            //    })
            //    .WithApiVersionSet(apiVersionSetTutor);
        }
    }
}
