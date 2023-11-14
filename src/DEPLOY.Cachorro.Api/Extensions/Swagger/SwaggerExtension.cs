using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Api.Extensions.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(
                    options =>
                    {
                        options.ReportApiVersions = true;

                    })
                .AddMvc()
                .AddApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";

                        options.SubstituteApiVersionInUrl = true;
                    });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    options.EnableAnnotations();
                    options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}-{e.ActionDescriptor.RouteValues["controller"]}-{e.HttpMethod}".ToLower());
                    options.OperationFilter<SwaggerDefaultValues>();

                    var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
                    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                    options.IncludeXmlComments(filePath);
                });
        }

        public static void UseSwaggerExtension(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    var descriptions = app.DescribeApiVersions();
                    options.RoutePrefix = string.Empty;

                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
        }
    }
}
