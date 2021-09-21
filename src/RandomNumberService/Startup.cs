using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RandomNumberService.Application;
using RandomNumberService.Application.Common.Interfaces;
using RandomNumberService.Config;

namespace RandomNumberService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var seqConfig = Configuration.GetSection("Seq").Bind<SeqConfig>();
            var jaegerConfig = Configuration.GetSection("Jaeger").Bind<JaegerConfig>();

            services.AddSingleton<AppConfig>(_ =>
            {
                var appConfig = new AppConfig()
                {
                    ThrowOnModulo = Configuration.GetValue<int>("ThrowOnModulo")
                };

                return appConfig;
            });

            var seqUrl = seqConfig.Url ?? "http://localhost:5341";
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(seqUrl);
            });

            services.AddControllers();
            services.AddSingleton<IRandomNumberService, Infrastructure.RandomNumberService>();

            services.AddOpenTelemetryTracing(builder =>
            {
                var serviceName = jaegerConfig.ServiceName ?? "RandomNumberService";
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(b =>
                    {
                        var jaegerHostname = jaegerConfig.HostName ?? "localhost";
                        Console.WriteLine($"Jaeger hostname: {jaegerHostname}");
                        b.AgentHost = jaegerHostname;
                    });

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RandomNumberService", Version = "v1" });
            });
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // enable swagger, because it's a demo app
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RandomNumberService v1"));

            // do not use https - networking assumptions are not the job of the program
            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            // add health checks
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
