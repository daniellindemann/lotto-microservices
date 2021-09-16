using System.Reflection;
using LottoService.Application.Common.Interfaces;
using LottoService.Application.Configuration;
using LottoService.Infrastructure;
using LottoService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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
                return s;
            });

            services.AddHttpClient<IRandomNumberService, RandomNumberService>();
            services.AddScoped<IRandomNumberService, RandomNumberService>();

            services.AddMediatR(typeof(Startup));

            return services;
        }
    }
}
