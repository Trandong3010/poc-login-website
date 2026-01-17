using Microsoft.Playwright;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Adapters;

/// <summary>
/// Playwright implementation of IPagePort.
/// Handles page-level interactions using Playwright.
/// </summary>
public class PlaywrightPageAdapter : IPagePort
{
    private readonly IPage _page;

    public PlaywrightPageAdapter(IPage page)
    {
        _page = page;
    }

    public async Task GotoAsync(string url, CancellationToken cancellationToken = default)
    {
        await _page.GotoAsync(url);
    }

    public async Task<string> GetTitleAsync(CancellationToken cancellationToken = default)
    {
        return await _page.TitleAsync();
    }

    public async Task<string> GetUrlAsync(CancellationToken cancellationToken = default)
    {
        return _page.Url;
    }

    public async Task WaitForSelectorAsync(string selector, int timeoutMs = 30000, CancellationToken cancellationToken = default)
    {
        await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = timeoutMs });
    }

    public async Task ScreenshotAsync(string path, CancellationToken cancellationToken = default)
    {
        await _page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
    }

    public async Task<IElementPort> GetElementAsync(string selector, CancellationToken cancellationToken = default)
    {
        var locator = _page.Locator(selector);
        return new PlaywrightElementAdapter(locator);
    }

    public async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        await _page.CloseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await CloseAsync();
    }
}
