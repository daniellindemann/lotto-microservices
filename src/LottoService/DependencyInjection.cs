using System;
using System.Net;
using System.Reflection;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.Configuration;
using LottoService.Infrastructure;
using LottoService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace LottoService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLottoService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<RandomNumberServiceSettings>(provider =>
            {
                var s = new RandomNumberServiceSettings();
                configuration.GetSection("RandomNumberService").Bind(s);

                s.Url = configuration.GetServiceUri("RandomNumberService")?.ToString() ?? s.Url;

                return s;
            });

            services.AddHttpClient<IRandomNumberService, RandomNumberService>()
                // retry policy
                .AddPolicyHandler(_ =>
                {
                    var jitterer = new Random();
                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(5, retryAttempt =>
                                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + // exponential back-off: 2, 4, 8 etc
                                TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)) // plus some jitter: up to 1 second
                        );
                })
                // circuit breaker
                .AddPolicyHandler(_ =>
                {
                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
                });
            services.AddScoped<IRandomNumberService, RandomNumberService>();

            services.AddMediatR(typeof(Startup));

            return services;
        }
    }
}
