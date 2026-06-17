using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UsosPotwierdzanieWnioskow.Configuration;
using UsosPotwierdzanieWnioskow.Playwright;
using UsosPotwierdzanieWnioskow.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOptions<UsosCredentials>()
    .BindConfiguration("USOS")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.AddConsole();
    loggerBuilder.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddSingleton<BrowserFactory>();

builder.Services.AddTransient<UsosLoginService>();
builder.Services.AddTransient<ApplicationProcessor>();
builder.Services.AddTransient<UsosBot>();

using var host = builder.Build();
var bot = host.Services.GetRequiredService<UsosBot>();

await bot.RunAsync();