using System.Diagnostics;
using PocLoginWebsite.Application.Services;
using PocLoginWebsite.ConsoleApp.Services;
using PocLoginWebsite.Core.Common;
using PocLoginWebsite.Core.Extensions;
using PocLoginWebsite.Core.Models;
using PocLoginWebsite.Infrastructure.Adapters;
using PocLoginWebsite.Infrastructure.Configuration;
using PocLoginWebsite.Infrastructure.PageObjects;

namespace PocLoginWebsite.ConsoleApp.Services;

/// <summary>
/// Service that orchestrates the SauceDemo POC workflow with full utilization of helper functions.
/// </summary>
public class SauceDemoPocService
{
    private readonly ConsoleLogger _logger;
    private readonly AppConfiguration _config;
    private readonly BrowserStateService _stateService;
    private readonly Stopwatch _stopwatch;

    public SauceDemoPocService(ConsoleLogger logger, AppConfiguration config)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
        _config = Guard.Against.Null(config, nameof(config));
        _stateService = new BrowserStateService(_config.BrowserStatePath);
        _stopwatch = new Stopwatch();
    }

    public async Task<int> RunAsync()
    {
        _logger.LogHeader("SauceDemo Automated Login POC", "Playwright with Stealth Features");
        _stopwatch.Start();

        try
        {
            LogConfiguration();

            var browser = await LaunchBrowserWithRetryAsync();
            var page = await GetOrCreatePageWithStateAsync(browser);

            await NavigateAndVerifyStealthAsync(page);

            // Only login if we don't have saved state
            if (!_stateService.StateExists())
            {
                await LoginWithRetryAsync(page);
                await SaveBrowserStateAsync(browser);
            }
            else
            {
                _logger.LogSuccess("Skipped login (using saved state)");
                _logger.NewLine();
            }

            await ExtractDataAsync(page);
            await CleanupAsync(browser);

            _stopwatch.Stop();
            _logger.LogFooter($"POC completed successfully in {_stopwatch.ElapsedMilliseconds}ms");

            return 0;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            _logger.LogError(ex, _stopwatch.ElapsedMilliseconds);
            return 1;
        }
    }

    private void LogConfiguration()
    {
        _logger.LogStep(1, 7, "Configuration loaded");
        _logger.LogData("Target", _config.SauceDemoUrl);
        _logger.LogData("Headless", _config.Headless);
        _logger.LogData("Stealth", _config.StealthEnabled);
        _logger.LogData("Proxy", _config.ProxyEnabled ? _config.ProxyServer.OrEmpty() : "Disabled");
        _logger.LogData("State Persistence", _config.BrowserStateEnabled ? "Enabled" : "Disabled");

        if (_stateService.StateExists())
        {
            var (_, lastModified, sizeBytes) = _stateService.GetStateInfo();
            _logger.LogInfo($"      Found saved state: {lastModified} ({sizeBytes} bytes)");
        }

        _logger.NewLine();
    }

    private async Task<PlaywrightBrowserAdapter> LaunchBrowserWithRetryAsync()
    {
        _logger.LogStep(2, 7, "Launching browser with stealth configuration...");

        var stealthOptions = new StealthOptions
        {
            UserAgent = _config.StealthUserAgent,
            ViewportWidth = _config.StealthViewportWidth,
            ViewportHeight = _config.StealthViewportHeight,
            EnableStealth = _config.StealthEnabled,
            ProxyServer = _config.ProxyEnabled ? _config.ProxyServer : null,
            ProxyUsername = _config.ProxyUsername,
            ProxyPassword = _config.ProxyPassword
        };

        var browser = new PlaywrightBrowserAdapter();

        // Use Retry with timeout
        await Retry.ExecuteAsync(
            async () => await browser.LaunchAsync(
                _config.BrowserType,
                _config.Headless,
                stealthOptions
            ).WithTimeout(Constants.Timeouts.Long),
            maxAttempts: 3,
            initialDelay: TimeSpan.FromMilliseconds(500)
        );

        _logger.LogSuccess("Browser launched successfully");
        _logger.NewLine();

        return browser;
    }

    private async Task<Core.Ports.IPagePort> GetOrCreatePageWithStateAsync(PlaywrightBrowserAdapter browser)
    {
        if (_config.BrowserStateEnabled && _stateService.StateExists())
        {
            _logger.LogStep(3, 7, "Loading saved browser state...");
            var page = await browser.NewPageWithStateAsync(_stateService.GetStatePath());
            _logger.LogSuccess($"State loaded from {_stateService.GetStatePath()}");
            _logger.NewLine();
            return page;
        }

        return await browser.NewPageAsync();
    }

    private async Task NavigateAndVerifyStealthAsync(Core.Ports.IPagePort page)
    {
        Guard.Against.Null(page, nameof(page));

        _logger.LogStep(4, 7, "Navigating to SauceDemo...");

        var loginPage = new LoginPageObject(page, _config);

        // Navigate with retry and timeout
        await Retry.ExecuteAsync(
            async () => await loginPage.NavigateAsync()
                .WithTimeout(Constants.Timeouts.PageLoad),
            maxAttempts: 3
        );

        await loginPage.IsPageLoadedAsync();

        _logger.LogSuccess($"Navigated to {_config.SauceDemoUrl}");
        _logger.NewLine();

        // Verify stealth
        var webdriverValue = await page.EvaluateAsync<object>("navigator.webdriver");
        _logger.LogInfo($"[INFO] navigator.webdriver = {webdriverValue ?? "undefined"}");

        if (webdriverValue == null)
        {
            _logger.LogSuccess("Stealth mode active (navigator.webdriver is undefined)");
        }
        else
        {
            _logger.LogWarning("Warning: navigator.webdriver is still defined");
        }

        _logger.NewLine();
    }

    private async Task LoginWithRetryAsync(Core.Ports.IPagePort page)
    {
        Guard.Against.Null(page, nameof(page));
        Guard.Against.NullOrWhiteSpace(_config.SauceDemoUsername, nameof(_config.SauceDemoUsername));
        Guard.Against.NullOrWhiteSpace(_config.SauceDemoPassword, nameof(_config.SauceDemoPassword));

        _logger.LogStep(5, 7, $"Logging in as '{_config.SauceDemoUsername}'...");

        var loginPage = new LoginPageObject(page, _config);

        // Login with retry and timeout
        await Retry.ExecuteAsync(
            async () =>
            {
                await loginPage.LoginAsync(_config.SauceDemoUsername, _config.SauceDemoPassword)
                    .WithTimeout(Constants.Timeouts.Default);

                var loginSuccess = await loginPage.WaitForLoginSuccessAsync();
                Guard.Against.InvalidOperation(!loginSuccess, "Login failed");
            },
            maxAttempts: 3,
            initialDelay: TimeSpan.FromMilliseconds(500)
        );

        _logger.LogSuccess("Login successful");
        _logger.NewLine();
    }

    private async Task SaveBrowserStateAsync(PlaywrightBrowserAdapter browser)
    {
        if (!_config.BrowserStateEnabled)
            return;

        try
        {
            await browser.SaveStateAsync(_stateService.GetStatePath());
            _logger.LogSuccess($"Browser state saved to {_stateService.GetStatePath()}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed to save state: {ex.Message}");
        }
    }

    private async Task ExtractDataAsync(Core.Ports.IPagePort page)
    {
        Guard.Against.Null(page, nameof(page));

        _logger.LogStep(6, 7, "Extracting data from products page...");

        var productsPage = new ProductsPageObject(page);
        await productsPage.IsPageLoadedAsync();

        // Use string extensions
        var pageTitle = await productsPage.GetPageTitleViaJavaScriptAsync();
        _logger.LogData("Page Title", pageTitle.OrEmpty());

        var productCount = await productsPage.GetProductCountAsync();
        _logger.LogData("Product Count", productCount);

        var productNames = await productsPage.GetProductNamesAsync();

        // Use collection extensions
        if (!productNames.IsNullOrEmpty())
        {
            _logger.LogList("Products", productNames);

            // Demonstrate Batch extension
            var batches = productNames.Batch(3).ToList();
            _logger.LogInfo($"      Products in {batches.Count} batches of 3");
        }

        _logger.NewLine();

        var stealthActive = await productsPage.IsStealthActiveAsync();
        _logger.LogData("Stealth Status", stealthActive ? "Active ✓" : "Inactive ✗");
        _logger.NewLine();
    }

    private async Task CleanupAsync(PlaywrightBrowserAdapter browser)
    {
        Guard.Against.Null(browser, nameof(browser));

        _logger.LogStep(7, 7, "Cleanup...");
        await browser.CloseAsync();
        _logger.LogSuccess("Browser closed");
        _logger.NewLine();
    }
}
