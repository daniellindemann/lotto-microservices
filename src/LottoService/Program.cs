using LottoService.Application.Interfaces;
using LottoService.Application.Services;
using LottoService.Config;
using LottoService.Infrastructure.Services;

using System.Linq;

using Microsoft.Extensions.Options;
using System.Collections;
using LottoService.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("App"));
builder.Services.AddOptions<RandomNumberServiceConfig>()
    .Bind(builder.Configuration.GetSection("RandomNumberService"))
    .PostConfigure((RandomNumberServiceConfig randomNumberServiceConfig) =>
    {
        // check if type is enabled and get service url
        if (builder.Configuration.IsTye())
        {
            randomNumberServiceConfig.Url = builder.Configuration.GetServiceUri("randomnumberservice")?.ToString() ?? randomNumberServiceConfig.Url;
        }
    });

// add dapr if enabled
var daprConfig = new DaprConfig();
builder.Configuration.GetSection("Dapr").Bind(daprConfig);
if (daprConfig.Enabled)
{
    builder.Services.AddDaprClient();
    builder.Services.AddScoped<IRandomNumberService, DaprRandomNumberService>();
}
else
{
    builder.Services.AddHttpClient<IRandomNumberService, RandomNumberService>();
    builder.Services.AddScoped<IRandomNumberService, RandomNumberService>();
}

// configure lotto number service
var redisConfig = new RedisConfig();
builder.Configuration.GetSection("Redis").Bind(redisConfig);
if (redisConfig.Enabled)
{
    builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("Redis"));

    if (daprConfig.Enabled)
    {
        builder.Services.AddHttpClient<DaprRandomNumberService>();
        builder.Services.AddScoped<ILottoNumberService, DaprCachedLottoNumberService>();
    }
    else
    {
        builder.Services.AddStackExchangeRedisCache((options) =>
        {
            options.Configuration = builder.Configuration.IsTye() ?
                builder.Configuration.GetConnectionString("redis") :
                builder.Configuration.GetConnectionString(RedisConfig.ConnectionStringName);
            options.InstanceName = redisConfig.Instance;
        });
        builder.Services.AddSingleton(typeof(ICacheService<>), typeof(RedisCacheService<>));
        builder.Services.AddScoped<ILottoNumberService, CachedLottoNumberService>();
    }
}
else
{
    builder.Services.AddScoped<ILottoNumberService, LottoNumberService>();
}

// add dapr if enabled
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add health checks
builder.Services.AddHealthChecks();

// add cors config
// allow everything
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// do not use https - networking assumptions are not the job of the program
//app.UseHttpsRedirection();

app.UseCors();  // use cors

app.UseAuthorization();

// add health checks
app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
