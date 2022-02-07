using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Web.Config;
using Web.Data;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var seqConfig = Configuration.GetSection("Seq").Bind<SeqConfig>();
            var jaegerConfig = Configuration.GetSection("Jaeger").Bind<JaegerConfig>();

            var seqUrl = seqConfig.Url ?? "http://localhost:5341";
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(seqUrl);
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

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient<LottoService>();

            services.AddSingleton<LottoServiceConfig>(_ =>
            {
                var apiUrl = Configuration.GetServiceUri("LottoService")?.ToString() ?? Configuration.GetValue<string>("Api");
#if DEBUG
                apiUrl ??= "http://localhost:5002";
#endif
                var lottoServiceConfig = new LottoServiceConfig() { Url = apiUrl };
                return lottoServiceConfig;
            });
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<LottoService>();

            services.AddOpenTelemetryTracing(builder =>
            {
                var jaegerServiceName = jaegerConfig.ServiceName ?? "Web";
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(jaegerServiceName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(b =>
                    {
                        var jaegerHostname = jaegerConfig.HostName ?? "localhost";
                        Console.WriteLine($"Jaeger hostname: {jaegerHostname}");
                        b.AgentHost = jaegerHostname;
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // do not use https - networking assumptions are not the job of the program
                // app.UseHsts();
            }

            // do not use https - networking assumptions are not the job of the program
            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
