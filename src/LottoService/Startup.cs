using System;
using LottoService.Config;
using LottoService.Infrastructure;
using LottoService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

namespace LottoService
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
            var redisConfig = Configuration.GetSection("Redis").Bind<RedisConfig>();

            var seqUrl = seqConfig.Url ?? "http://localhost:5341";
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(seqUrl);
            });

            services.AddControllers();

            ConnectionMultiplexer redisConnection = null;
            var redisHost = redisConfig.HostName ?? "localhost";
            try
            {
                redisConnection = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints = { redisHost }, ConnectTimeout = 250, ConnectRetry = 1
                });
            }
            catch (RedisConnectionException)
            {
                // catch connection exception and do nothing
            }

            services.AddSingleton<IRedisContext, RedisContext>(c =>
            {
                if (redisConnection != null)
                    return new RedisContext(redisConnection);

                return RedisContext.Empty;
            });

            var instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY") ?? Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");
            if(!string.IsNullOrEmpty(instrumentationKey))
            {
                services.AddApplicationInsightsTelemetry(instrumentationKey);
                services.AddApplicationInsightsKubernetesEnricher();
                Console.WriteLine("Setup instrumentation for application insights");
            }
            else
            {
                Console.WriteLine("Application insights instrumentation not configured");
            }

            services.AddLottoService(Configuration);

            services.AddOpenTelemetryTracing(builder =>
            {
                var jaegerServiceName = jaegerConfig.ServiceName ?? "LottoService";
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(jaegerServiceName))
                    .AddSource(nameof(RandomNumberService))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (redisConnection != null)
                {
                    builder.AddRedisInstrumentation(redisConnection);
                }

                builder.AddJaegerExporter(b =>
                {
                    var jaegerHostname = jaegerConfig.HostName ?? "localhost";
                    Console.WriteLine($"Jaeger hostname: {jaegerHostname}");
                    b.AgentHost = jaegerHostname;
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LottoService", Version = "v1" });
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LottoService v1"));

            // do not use https - networking assumptions are not the job of the program
            //app.UseHttpsRedirection();

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
