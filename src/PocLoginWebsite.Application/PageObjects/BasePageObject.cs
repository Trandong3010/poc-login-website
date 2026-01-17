using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Application.PageObjects;

/// <summary>
/// Abstract base class for all page objects.
/// Provides common functionality for page interactions following the Page Object Model pattern.
/// </summary>
public abstract class BasePageObject
{
    protected readonly IPagePort Page;

    protected BasePageObject(IPagePort page)
    {
        Page = page;
    }

    /// <summary>
    /// Navigates to the page URL.
    /// </summary>
    public abstract Task NavigateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies that the page is loaded correctly.
    /// </summary>
    public abstract Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Takes a screenshot of the current page.
    /// </summary>
    /// <param name="fileName">Name of the screenshot file.</param>
    protected async Task TakeScreenshotAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var screenshotPath = Path.Combine("screenshots", $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
        Directory.CreateDirectory("screenshots");
        await Page.ScreenshotAsync(screenshotPath, cancellationToken);
    }

    /// <summary>
    /// Waits for the page to be ready.
    /// </summary>
    protected async Task WaitForPageReadyAsync(int timeoutMs = 30000, CancellationToken cancellationToken = default)
    {
        // Override in derived classes if needed
        await Task.CompletedTask;
    }
}
