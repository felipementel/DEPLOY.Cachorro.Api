using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
                .WithApiVersionSet(apiVersionSetCachorro);

            cachorros
                .MapGet("/", ListAllAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "listall-cachorros-get",
                    Summary = "Listar Cachorros",
                    Description = "Operação para listar de cachorros",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/{id:guid}", GetByIdAsync)
                .Produces<CachorroDto>(200, "application/json")
                .Produces(404)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "getbyid-cachorros-get",
                    Summary = "Obter Cachorro",
                    Description = "Operação para obter cachorro por id",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapPost("/", CreateCachorroAsync)
                .Accepts<CachorroCreateDto>("application/json") //não tem na controller
                .Produces<CachorroDto>(201, "application/json")
                .Produces(422)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "create-cachorros-post",
                    Summary = "Cadastrar Cachorro",
                    Description = "Operação para cadastrar cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapPut("/{id:guid}", UpdateCachorroAsync)
                .Accepts<CachorroCreateDto>("application/json")
                .Produces(204)
                .Produces(422)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "update-cachorros-put",
                    Summary = "Atualizar Cachorro",
                    Description = "Operação para atualizar de cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapDelete("/{id:guid}", DeleteCachorroAsync)
                .Produces(204)
                .Produces(404)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "delete-cachorros-delete",
                    Summary = "Excluir Cachorro",
                    Description = "Operação para excluir cachorro",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/adotados", ListAllCachorrosAdotadosAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .Produces(204)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "listallcachorrosadotados-cachorros-get",
                    Summary = "Lista de cachorro adotados",
                    Description = "Operação para listar de cachorros adotados",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            cachorros
                .MapGet("/para-adotar", ListAllCachorrosParaAdotarAsync)
                .Produces<IEnumerable<CachorroDto>>(200, "application/json")
                .Produces(204)
                .Produces(401)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "listallcachorrosparaadocao-cachorros-get",
                    Summary = "Lista de cachorro disponíveis para adoção",
                    Description = "Operação para listar de cachorros disponíveis para adoção",
                    Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
                });

            //async Task<IResult> ListAllAsync(
            async Task<Results<Ok<IEnumerable<CachorroDto>>, NoContent>> ListAllAsync(
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
