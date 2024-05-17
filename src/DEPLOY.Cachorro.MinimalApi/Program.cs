using DEPLOY.Cachorro.Infra.CrossCutting;
using DEPLOY.Cachorro.MinimalApi.Endpoints.v1;
using DEPLOY.Cachorro.MinimalApi.Endpoints.v2;
using DEPLOY.Cachorro.MinimalApi.Extensions.Database;
using DEPLOY.Cachorro.MinimalApi.Extensions.Swagger;
using System.Reflection;
using System.Text.Json.Serialization;
using DEPLOY.Cachorro.MinimalApi.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRegisterServices();

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.SerializerOptions.WriteIndented = true;
    opt.SerializerOptions.PropertyNameCaseInsensitive = true;
    opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddEndpointsApiExplorer();

//Configure Extensions
builder.Services.AddSwaggerExtension();
builder.Services.AddDatabaseExtension(builder.Configuration);

// Configure the HTTP request pipeline.
var app = builder.Build();

// without version
var versionSetPing = app.NewApiVersionSet("Ping")
                    .Build();

app
    .MapGet("/ping", () =>
{
    return TypedResults.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
}).WithOpenApi(operation => new(operation)
{
    OperationId = "get-ping-get"
})
.WithApiVersionSet(versionSetPing)
.Produces<string>(200);

//v1
app.MapCachorroEndpoints();

app.MapTutorEndpoints();

app.MapAdocoesEndpoints();

//v2 
app.MapCachorroEndpointsV2();

//Use Extensions
app.UseSwaggerExtension();

app.UseHttpsRedirection();

await app.RunAsync();