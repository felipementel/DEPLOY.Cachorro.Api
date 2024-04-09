using DEPLOY.Cachorro.Application.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Interfaces.Service;
using DEPLOY.Cachorro.Domain.Aggregates.Adotar.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Cachorro.Validations;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Repositories;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Interfaces.Services;
using DEPLOY.Cachorro.Domain.Aggregates.Tutor.Validations;
using DEPLOY.Cachorro.Infra.Repository.Repositories;
using DEPLOY.Cachorro.Infra.Repository.Repositories.Base;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;

namespace DEPLOY.Cachorro.Infra.CrossCutting
{
    [ExcludeFromCodeCoverage]
    public static class IoC
    {
        public static void AddRegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICachorroAppServices, Application.AppServices.CachorroAppServices>();
            services.AddScoped<ITutorAppServices, Application.AppServices.TutorAppServices>();
            services.AddScoped<IAdocaoAppService, Application.AppServices.AdocaoAppService>();

            services.AddScoped<IValidator<Domain.Aggregates.Cachorro.Entities.Cachorro>, CachorroValidator>();
            services.AddScoped<IValidator<Domain.Aggregates.Tutor.Entities.Tutor>, TutorValidator>();

            services.AddScoped<ICachorroService, Domain.Aggregates.Cachorro.Services.CachorroService>();
            services.AddScoped<ITutorService, Domain.Aggregates.Tutor.Services.TutorService>();
            services.AddScoped<IAdocaoService, AdocaoService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICachorroRepository, CachorroRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();
        }
    }
}
