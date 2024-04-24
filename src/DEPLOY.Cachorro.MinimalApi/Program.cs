using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DEPLOY.Cachorro.Infra.CrossCutting;
using DEPLOY.Cachorro.MinimalApi.Endpoints;
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

app.UseSwaggerExtension();


var versionSetPing = app.NewApiVersionSet("ping")
                    .Build();

app.MapGet("/ping", () => Assembly.GetExecutingAssembly().GetName().Version.ToString())
    .WithApiVersionSet(versionSetPing)
    .Produces<string>(200);

//=======================================================

app.MapCachorroEndpoints();

app.MapTutorEndpoints();

//var apiVersionSetCachorrov2 = app
//    .NewApiVersionSet("cachorrosv2")
//    .HasApiVersion(new ApiVersion(2))
//    .ReportApiVersions()
//    .Build();

//var cachorros = app
//    .MapGroup("/api/v{apiVersion:apiVersion}/cachorros")
//    .RequireAuthorization()
//    .WithApiVersionSet(apiVersionSetCachorrov2);

//cachorros
//    .MapGet("/", async(HttpContext context) => 
//    {
//        return TypedResults.Ok(new List<CachorroDto>());
//    })
//    .Produces<IEnumerable<CachorroDto>>(200, "application/json")
//    .WithOpenApi(operation => new(operation)
//    {
//        OperationId = "ListAllCachorros",
//        Summary = "List Cachorro",
//        Description = "Operação para listar de cachorros",
//        Tags = new List<OpenApiTag> { new() { Name = "Cachorros" } }
//    });


//cachorrosControllers
//    .MapPost("/", (
//        HttpRequest request,
//        ApiVersion version,
//        CachorroCreateDto cachorroCreateDto,
//        [FromServices] ICachorroAppServices cachorroAppServices,
//        CancellationToken cancelattionToken) =>
//    {
//        var item = cachorroAppServices.InsertAsync(cachorroCreateDto, cancelattionToken);

//        var scheme = request.Scheme;
//        var host = request.Host;
//        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/v{version}/api/cachorros/{item.Id}");

//        return Results.Created(location, item);
//    })
//        .Accepts<CachorroCreateDto>("application/json")
//        .Produces<CachorroCreateDto>(201)
//        .Produces(400)
//        .Produces(401)
//        .Produces(500);



//cachorrosControllers
//    .MapDelete("/{id}", DeleteCachorro)
//    .RequireAuthorization()
//    .WithApiVersionSet(versionSet)
//    .MapToApiVersion(1.0)
//    .WithOpenApi()
//    .IsApiVersionNeutral();





//var configKeyVaultControllers = app.MapGroup("/keyvault");

//configKeyVaultControllers
//    .MapGet("/withoptions/{key}", ListAllAsyncKeyVault)
//    .RequireAuthorization()
//    .WithOpenApi();

//configKeyVaultControllers
//    .MapGet("/{key}", ListAllAsyncKeyVaultInjected)
//    .RequireAuthorization()
//    .WithOpenApi();

app.Run();


async Task<IResult> ListAllAsyncKeyVault(
    IConfiguration configuration,
    string key)
{
    SecretClientOptions options = new SecretClientOptions()
    {
        Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(3),
            MaxRetries = 2,
            Mode = RetryMode.Exponential
        }
    };

    var client = new SecretClient(
        new Uri(configuration.GetSection("KeyVault:VaultUri").Value),
        new DefaultAzureCredential(),
        options);

    KeyVaultSecret secret = await client.GetSecretAsync(name: key);

    string secretValue = secret.Value;

    return TypedResults.Ok(secretValue);
}

async Task<IResult> ListAllAsyncKeyVaultInjected(
    SecretClient secretClient,
    string key)
{
    KeyVaultSecret secret = await secretClient.GetSecretAsync(name: key);

    string secretValue = secret.Value;

    return TypedResults.Ok(secretValue);
}