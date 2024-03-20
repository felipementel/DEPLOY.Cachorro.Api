using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.RateLimiting;

namespace DEPLOY.Cachorro.Api.Extensions.HttpClient
{
    public static class HttpClientExtension
    {
        public static void AddHttpClient(this IServiceCollection services)
        {
            services
                .AddHttpClient("Pokemon", client =>
                {
                    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddStandardResilienceHandler(options =>
                {
                    // Customize retry
                    options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<TimeoutRejectedException>()
                        .Handle<HttpRequestException>()
                        .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError);
                    options.Retry.MaxRetryAttempts = 5;

                    // Customize attempt timeout
                    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2);
                });
        }
    }
}
