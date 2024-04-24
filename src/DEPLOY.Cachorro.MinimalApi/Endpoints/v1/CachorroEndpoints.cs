using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.MinimalApi.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class CachorroEndpoints
    {
        public static void MapCachorroEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSetCachorro = app
                .NewApiVersionSet("cachorros")
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var cachorros = app
                .MapGroup("/api/v{apiVersion:apiVersion}/cachorros")
                //.RequireAuthorization()
                .WithApiVersionSet(apiVersionSetCachorro);

            cachorros
                .MapGet("/", ListAllAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListAllCachorrosAsync",
                    Summary = "List Cachorro",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/{id:guid}", GetByIdAsync)
                .Produces<CachorroDto>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "GetByIdCachorrosAsync",
                    Summary = "List Cachorro",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapPost("/", CreateCachorroAsync)
                .Accepts<CachorroCreateDto>("application/json")
                .Produces<CachorroDto>(201, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "CreateCachorroAsync",
                    Summary = "Cadastar Cachorro",
                    Description = "Operação para cadastrar cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapPut("/{id:guid}", UpdateCachorroAsync)
                .Accepts<CachorroCreateDto>("application/json")
                .Produces(204)
                .Produces(422)
                .Produces(401)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "UpdateCachorroAsync",
                    Summary = "Atualizar Cachorro",
                    Description = "Operação para atualizar de cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapDelete("/{id:guid}", DeleteCachorroAsync)
                .Produces(204)
                .Produces(422)
                .Produces(401)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "DeleteCachorroAsync",
                    Summary = "Excluir Cachorro",
                    Description = "Operação para excluir cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/adotados", ListAllCachorrosAdotadosAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListAllCachorrosAdotadosAsync",
                    Summary = "List Cachorro adotados",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/para-adotar", ListAllCachorrosParaAdotarAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "ListAllCachorrosParaAdotarAsync",
                    Summary = "List Cachorro para adotar",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });


            //async Task<IResult> ListAllAsync(
            async Task<Results<Ok<IEnumerable<CachorroDto?>>, NoContent>> ListAllAsync(
                ICachorroAppServices cachorroAppService,
                CancellationToken cancellationToken = default)
            {
                var items = await cachorroAppService.GetAllAsync(cancellationToken);

                return items?.Count() > 0 ? TypedResults.Ok(items) : TypedResults.NoContent();
            }

            async Task<IResult> GetByIdAsync(
                Guid id,
                ICachorroAppServices cachorroAppService,
                CancellationToken cancellationToken = default)
            {
                var items = await cachorroAppService.GetByIdAsync(id, cancellationToken);

                if (items == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(items);
            }

            async Task<IResult> CreateCachorroAsync(
                ICachorroAppServices cachorroAppServices,
                HttpRequest request,
                CachorroCreateDto cachorroCreateDto,
                CancellationToken cancellationToken = default)
            {
                var item = await cachorroAppServices.InsertAsync(
                                cachorroCreateDto,
                                cancellationToken);

                if (item?.Erros.Count() > 0)
                    return TypedResults.UnprocessableEntity(item.Erros);

                var apiVersion = request.RouteValues["apiVersion"];
                var scheme = request.Scheme;
                var host = request.Host;
                var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/v{apiVersion}/cachorros/{item?.Id}");

                return Results.Created(location, item);
            }

            async Task<IResult> DeleteCachorroAsync(
                ICachorroAppServices cachorroAppServices,
                Guid id,
                CancellationToken cancellationToken = default)
            {
                var item = await cachorroAppServices.DeleteAsync(
                                id,
                                cancellationToken);

                if (item)
                    return TypedResults.NoContent();

                return TypedResults.NotFound();
            }

            async Task<IResult> UpdateCachorroAsync(
                ICachorroAppServices cachorroAppServices,
                Guid id,
                CachorroDto cachorroDto,
                CancellationToken cancellationToken = default)
            {
                if (id != cachorroDto.Id)
                {
                    return TypedResults.UnprocessableEntity();
                }

                var item = await cachorroAppServices.UpdateAsync(
                    id,
                    cachorroDto,
                    cancellationToken);

                return !item.Any() ? TypedResults.NoContent()
                          : TypedResults.UnprocessableEntity(item);
            }

            async Task<IResult> ListAllCachorrosAdotadosAsync(
                ICachorroAppServices cachorroAppService,
                CancellationToken cancellationToken = default)
            {
                var items = await cachorroAppService.GetByKeyAsync(
                    c => c.Adotado,
                    cancellationToken);

                return items?.Count() > 0 ? TypedResults.Ok(items) : TypedResults.NoContent();
            }

            async Task<IResult> ListAllCachorrosParaAdotarAsync(
                ICachorroAppServices cachorroAppService,
                CancellationToken cancellationToken = default)
            {
                var items = await cachorroAppService.GetByKeyAsync(
                    c => !c.Adotado,
                    cancellationToken);

                return items?.Count() > 0 ? TypedResults.Ok(items) : TypedResults.NoContent();
            }
        }
    }
}
