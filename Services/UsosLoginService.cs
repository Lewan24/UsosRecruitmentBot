using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using UsosPotwierdzanieWnioskow.Configuration;
using UsosPotwierdzanieWnioskow.Constants;

namespace UsosPotwierdzanieWnioskow.Services;

public sealed class UsosLoginService(
    IOptions<UsosCredentials> credentials,
    ILogger<UsosLoginService> logger)
{
    public async Task LoginIfNeededAsync(IPage page)
    {
        await page.GotoAsync(UsosUrls.ApplicationsPage);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        var loginLink = page.GetByText("zaloguj się");

        if (await loginLink.CountAsync() == 0)
            return;

        logger.LogInformation("User not authenticated. Logging in...");

        await page.Locator(Selectors.LoginPageButton)
            .ClickAsync();

        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        await page.Locator(Selectors.LoginUsername)
            .FillAsync(credentials.Value.Username);

        await page.Locator(Selectors.LoginPassword)
            .FillAsync(credentials.Value.Password);

        await page.Locator(Selectors.LogInButton)
            .ClickAsync();

        await page.GetByText("Zalogowany użytkownik")
            .WaitForAsync(new()
            {
                Timeout = 0
            });
    }
}