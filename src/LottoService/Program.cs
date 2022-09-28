using LottoService.Application.Interfaces;
using LottoService.Application.Services;
using LottoService.Config;
using LottoService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("App"));
builder.Services.Configure<RandomNumberServiceConfig>(builder.Configuration.GetSection("RandomNumberService"));
builder.Services.AddHttpClient<IRandomNumberService, RandomNumberService>();
builder.Services.AddScoped<IRandomNumberService, RandomNumberService>();
builder.Services.AddScoped<ILottoNumberService, LottoNumberService>();

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
