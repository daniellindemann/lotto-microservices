using LottoService.Extensions;

using Web.Config;

var builder = WebApplication.CreateBuilder(args);

var dict = Environment.GetEnvironmentVariables().Cast<System.Collections.DictionaryEntry>()
     .ToDictionary(kvp => (string)kvp.Key, kvp => (string)kvp.Value).OrderBy(kvp => kvp.Key);
foreach (var kvp in dict)
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}


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

// do not use https - networking assumptions are not the job of the program
// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
