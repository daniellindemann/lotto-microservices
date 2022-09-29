using LottoService.Extensions;

using Web.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions<LottoServiceConfig>()
    .Bind(builder.Configuration.GetSection("LottoService"))
    .PostConfigure((LottoServiceConfig lottoServiceConfig) =>
    {
        // check if type is enabled and get service url
        if (builder.Configuration.IsTye())
        {
            lottoServiceConfig.Url = builder.Configuration.GetServiceUri("lottoservice")?.ToString() ?? lottoServiceConfig.Url;
        }
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
