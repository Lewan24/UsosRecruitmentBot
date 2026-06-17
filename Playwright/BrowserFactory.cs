using Microsoft.Playwright;

namespace UsosPotwierdzanieWnioskow.Playwright;

public sealed class BrowserFactory
{
    public async Task<IBrowser> CreateAsync(bool headless = false)
    {
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        return await playwright.Chromium.LaunchAsync(new()
        {
            Headless = headless
        });
    }
}