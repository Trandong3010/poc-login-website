using Microsoft.Playwright;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Adapters;

/// <summary>
/// Playwright implementation of IPagePort.
/// Handles page-level interactions using Playwright.
/// </summary>
public class PlaywrightPageAdapter(IPage page) : IPagePort
{
    public async Task GotoAsync(string url, CancellationToken cancellationToken = default)
    {
        await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
    }

    public async Task<string> GetTitleAsync(CancellationToken cancellationToken = default)
    {
        return await page.TitleAsync();
    }

    public Task<string> GetUrlAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(page.Url);
    }

    public async Task WaitForSelectorAsync(
        string selector,
        int timeoutMs = 30000,
        CancellationToken cancellationToken = default
    )
    {
        await page.WaitForSelectorAsync(
            selector,
            new PageWaitForSelectorOptions
            {
                Timeout = timeoutMs,
                State = WaitForSelectorState.Visible
            }
        );
    }

    public async Task ScreenshotAsync(string path, CancellationToken cancellationToken = default)
    {
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
    }

    public Task<IElementPort> GetElementAsync(
        string selector,
        CancellationToken cancellationToken = default
    )
    {
        var locator = page.Locator(selector);
        return Task.FromResult<IElementPort>(new PlaywrightElementAdapter(locator));
    }

    public async Task<T> EvaluateAsync<T>(string script, CancellationToken cancellationToken = default)
    {
        return await page.EvaluateAsync<T>(script);
    }

    public async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        await page.CloseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await CloseAsync();
    }
}
