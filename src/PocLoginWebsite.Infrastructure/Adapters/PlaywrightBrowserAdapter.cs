using Microsoft.Playwright;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Adapters;

/// <summary>
/// Playwright implementation of IBrowserPort.
/// Manages browser lifecycle using Playwright.
/// </summary>
public class PlaywrightBrowserAdapter : IBrowserPort
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;

    public async Task LaunchAsync(
        string browserType = "chromium",
        bool headless = true,
        CancellationToken cancellationToken = default
    )
    {
        _playwright = await Playwright.CreateAsync();

        _browser = browserType.ToLower() switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
            "webkit" => await _playwright.Webkit.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
            _ => await _playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
        };

        _context = await _browser.NewContextAsync();
    }

    public async Task<IPagePort> NewPageAsync(CancellationToken cancellationToken = default)
    {
        if (_context == null)
        {
            throw new InvalidOperationException(
                "Browser context is not initialized. Call LaunchAsync first."
            );
        }

        var page = await _context.NewPageAsync();
        return new PlaywrightPageAdapter(page);
    }

    public async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (_context != null)
        {
            await _context.CloseAsync();
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CloseAsync();
    }
}
