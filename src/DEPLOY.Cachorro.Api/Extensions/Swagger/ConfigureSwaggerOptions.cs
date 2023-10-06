using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DEPLOY.Cachorro.Api.Extensions.Swagger
{
    [ExcludeFromCodeCoverage]
    public class ConfigureSwaggerOptions
    : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(
                ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "Cachorro WebAPI .NET 7",
                Version = desc.ApiVersion.ToString(),
                Description = $"WebApi criada no canal D.E.P.L.O.Y. v{Assembly.GetExecutingAssembly().GetName().Version}",
                Contact = new OpenApiContact()
                {
                    Email = "admin@felipementel.dev.br",
                    Name = "Felipe Augusto"
                }
            };

            if (desc.IsDeprecated)
            {
                info.Description += "Endpoint depreciado. Pesquise e utilize a versão mais recente.";
            }

            return info;
        }
    }
}