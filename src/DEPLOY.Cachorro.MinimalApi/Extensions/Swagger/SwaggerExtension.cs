﻿using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace DEPLOY.Cachorro.MinimalApi.Extensions.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(
                    options =>
                    {
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                               new HeaderApiVersionReader("x-api-version"),
                                                               new MediaTypeApiVersionReader("x-api-version"));
                    })
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
                    options.DisplayRequestDuration();
                    var descriptions = app.DescribeApiVersions();
                    options.RoutePrefix = string.Empty;

                    foreach (var groupName in descriptions.Select(description => description.GroupName))
                    {
                        var url = $"/swagger/{groupName}/swagger.json";
                        var name = groupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
        }
    }
}