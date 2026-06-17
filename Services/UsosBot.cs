using Microsoft.Extensions.Logging;
using UsosPotwierdzanieWnioskow.Playwright;

namespace UsosPotwierdzanieWnioskow.Services;

public sealed class UsosBot(
    BrowserFactory browserFactory,
    UsosLoginService loginService,
    ApplicationProcessor applicationProcessor,
    ILogger<UsosBot> logger)
{
    public async Task RunAsync()
    {
        logger.LogInformation("Launching browser...");

        await using var browser = await browserFactory.CreateAsync();
        
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await loginService.LoginIfNeededAsync(page);
        await applicationProcessor.ProcessAsync(page);
        
        logger.LogInformation("Finished successfully.");
        await page.PauseAsync();
    }
}