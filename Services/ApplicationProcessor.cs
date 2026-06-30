using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using UsosPotwierdzanieWnioskow.Constants;

namespace UsosPotwierdzanieWnioskow.Services;

public sealed class ApplicationProcessor(
    ILogger<ApplicationProcessor> logger)
{
    public async Task ProcessAsync(IPage page)
    {
        await page.GotoAsync(UsosUrls.ApplicationsPage);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        await ConfigureFiltersAsync(page);

        var items =
            await page.GetByText(
                    "przyjmij do rozpatrzenia",
                    new() { Exact = true })
                .CountAsync();

        logger.LogInformation("Found {Count} applications.", items);
        logger.LogInformation("Processing applications...");
        
        while (items-- > 0)
        {
            await AcceptNextApplicationAsync(page);
        }
    }
    
    private async Task ConfigureFiltersAsync(IPage page)
    {
        logger.LogInformation("Configuring filters...");

        await page.Locator(Selectors.StanFilter)
            .ClickAsync();

        await page.WaitForTimeoutAsync(2000);

        await page.Locator(Selectors.ZlozonyFilter)
            .ClickAsync();

        await page.WaitForTimeoutAsync(1000);
        
        await page.Locator(Selectors.ShowAllButton)
            .ClickAsync();

        await page.WaitForTimeoutAsync(5000);
        
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    private async Task AcceptNextApplicationAsync(IPage page)
    {
        await page.GetByText(
                "przyjmij do rozpatrzenia",
                new() { Exact = true })
            .First
            .ClickAsync();

        await page.WaitForTimeoutAsync(1000);
        
        await page.GetByText(
                "Powiadom wnioskodawcę wiadomością e-mail")
            .WaitForAsync();

        await page.Locator(Selectors.AcceptApplicationButton)
            .ClickAsync();
        
        await page.WaitForTimeoutAsync(1000);
        
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}