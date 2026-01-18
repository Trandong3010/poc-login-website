using Microsoft.Playwright;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Adapters;

/// <summary>
/// Playwright implementation of IElementPort.
/// Handles element-level interactions using Playwright locators.
/// </summary>
public class PlaywrightElementAdapter(ILocator locator) : IElementPort
{
    public async Task ClickAsync(CancellationToken cancellationToken = default)
    {
        await locator.ClickAsync();
    }

    public async Task FillAsync(string text, CancellationToken cancellationToken = default)
    {
        await locator.FillAsync(text);
    }

    public async Task<string> GetTextAsync(CancellationToken cancellationToken = default)
    {
        var text = await locator.TextContentAsync();
        return text ?? string.Empty;
    }

    public async Task<string?> GetAttributeAsync(
        string attributeName,
        CancellationToken cancellationToken = default
    )
    {
        return await locator.GetAttributeAsync(attributeName);
    }

    public async Task<bool> IsVisibleAsync(CancellationToken cancellationToken = default)
    {
        return await locator.IsVisibleAsync();
    }

    public async Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await locator.IsEnabledAsync();
    }

    public async Task WaitForVisibleAsync(
        int timeoutMs = 30000,
        CancellationToken cancellationToken = default
    )
    {
        await locator.WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = timeoutMs }
        );
    }
}
