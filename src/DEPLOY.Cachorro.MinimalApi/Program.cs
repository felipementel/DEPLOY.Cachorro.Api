using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DEPLOY.Cachorro.Infra.CrossCutting;
using DEPLOY.Cachorro.MinimalApi.Endpoints;
using DEPLOY.Cachorro.MinimalApi.Endpoints.v1;
using DEPLOY.Cachorro.MinimalApi.Extensions.AppConfiguration;
using DEPLOY.Cachorro.MinimalApi.Extensions.Auth;
using DEPLOY.Cachorro.MinimalApi.Extensions.Database;
using DEPLOY.Cachorro.MinimalApi.Extensions.KeyVault;
using DEPLOY.Cachorro.MinimalApi.Extensions.Swagger;
using DEPLOY.Cachorro.MinimalApi.Extensions.Telemetria;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRegisterServices();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opt.JsonSerializerOptions.WriteIndented = true;
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddEndpointsApiExplorer();

//Configure Extensions
builder.Logging.AddLogExtension(builder.Configuration);
builder.Services.AddAuthExtension(builder.Configuration);
builder.Services.AddKeyVaultExtension(builder.Configuration);
builder.Services.AddDatabaseExtension(builder.Configuration);
builder.Services.AddTelemetriaExtension(builder.Configuration);
builder.Services.AddSwaggerExtension();
builder.Configuration.AddAppConfigurationExtension(builder.Services);

// Configure the HTTP request pipeline.
var app = builder.Build();

//=======================================================

var versionSetPing = app.NewApiVersionSet("ping")
                    .Build();

app
    .MapGet("/ping", async () =>
{
    return TypedResults.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
})
    .WithApiVersionSet(versionSetPing)
    .Produces<string>(200);

//=======================================================

app.MapCachorroEndpoints();

app.MapTutorEndpoints();

app.MapAdocoesEndpoints();

app.MapTestResourcesEndpoints();

app.UseSwaggerExtension();

app.Run();