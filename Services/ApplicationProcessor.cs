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

        //await ProcessFirstAcceptanceApplications(page);
        await ProcessFinalAcceptanceApplications(page);
    }

    private async Task ProcessFirstAcceptanceApplications(IPage page)
    {
        await ConfigureFiltersForFirstAcceptAsync(page);

        var items =
            await page.GetByText(
                    "przyjmij do rozpatrzenia",
                    new() { Exact = true })
                .CountAsync();

        logger.LogInformation("Found {Count} applications.", items);
        logger.LogInformation("Processing applications...");
        
        while (items-- > 0)
        {
            await AcceptNextFirstAcceptApplicationAsync(page);
        }
    }
    
    private async Task ProcessFinalAcceptanceApplications(IPage page)
    {
        await ConfigureFiltersForFinalAcceptAsync(page);

        var items =
            await page.GetByText(
                    "rozpatrz pozytywnie",
                    new() { Exact = true })
                .CountAsync();

        logger.LogInformation("Found {Count} applications.", items);
        logger.LogInformation("Processing applications...");
        
        await AcceptFinalAcceptApplicationsAsync(page);
    }

    private async Task ConfigureFiltersForFirstAcceptAsync(IPage page)
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

        // Wait for fully loaded page
        // Temporary usage for not so much applications
        await page.WaitForTimeoutAsync(5000);
        
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    private async Task ConfigureFiltersForFinalAcceptAsync(IPage page)
    {
        logger.LogInformation("Configuring filters...");

        await page.Locator(Selectors.StanFilter)
            .ClickAsync();

        await page.WaitForTimeoutAsync(2000);

        await page.Locator(Selectors.ReadyToAccept)
            .ClickAsync();

        await page.WaitForTimeoutAsync(1000);
        
        await page.Locator(Selectors.ShowAllButton)
            .ClickAsync();

        // Wait for fully loaded page
        // Temporary usage for not so much applications
        await page.WaitForTimeoutAsync(8000);
        
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    private async Task AcceptNextFirstAcceptApplicationAsync(IPage page)
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
    
    private async Task AcceptFinalAcceptApplicationsAsync(IPage page)
    {
        var rows = await page.Locator(Selectors.ApplicationsRows).AllAsync();

        logger.LogInformation("Found ({RowsCount}) rows", rows.Count);

        var validItems = 0;
        foreach (var row in rows)
        {
            var isApplication =
                await row.GetByText("Gotowy do rozpatrzenia", new () { Exact = true })
                    .CountAsync() == 1;

            if (!isApplication) 
                continue;
            
            logger.LogInformation("Found valid application");
            validItems++;

            var userIndex = await row.Locator(Selectors.StudentIndex).InnerTextAsync();
            logger.LogInformation("Processing Student nr {Valid} / {All} with index: {Index}...", validItems, rows.Count, userIndex);
            
            await row.GetByText(
                "rozpatrz pozytywnie",
                new() { Exact = true })
            .First
            .ClickAsync();
            
            await page.WaitForTimeoutAsync(1000);
            
            // Just random text that loads and need to wait for table to be fully loaded
            await page.GetByText(
                "Dom studencki nr 1, Pokoje")
            .WaitForAsync();
            
            var dsRows = await page.Locator(Selectors.StudentsDormitoriesRows).AllAsync();

            logger.LogInformation("Found ds {RowsCount} rows", dsRows.Count);

            Students.IndexesDs.TryGetValue(userIndex, out var dormitory);
            foreach (var dsRow in dsRows)
            {
                if (dormitory == null || await dsRow.GetByText(dormitory).CountAsync() != 1) 
                    continue;
                
                await dsRow.Locator(Selectors.SelectDormitoryButton).ClickAsync();
                await page.WaitForTimeoutAsync(500);
                
                break;
            }
            
            await page.Locator(Selectors.AcceptApplicationButton)
                .ClickAsync();
            
            await page.WaitForTimeoutAsync(1000);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}