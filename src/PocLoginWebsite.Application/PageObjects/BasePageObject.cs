using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Application.PageObjects;

/// <summary>
/// Abstract base class for all page objects.
/// Provides common functionality for page interactions following the Page Object Model pattern.
/// </summary>
public abstract class BasePageObject(IPagePort page)
{
    protected readonly IPagePort Page = page;

    /// <summary>
    /// Verifies that the page is loaded correctly.
    /// </summary>
    public abstract Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Takes a screenshot of the current page.
    /// </summary>
    /// <param name="fileName">Base name for the screenshot file (without extension).</param>
    /// <param name="screenshotFolder">Folder to save screenshots (default: "screenshots").</param>
    /// <param name="cancellationToken"></param>
    ///  <returns>The full path to the saved screenshot.</returns>
    protected async Task<string> TakeScreenshotAsync(
        string fileName,
        string screenshotFolder = "screenshots",
        CancellationToken cancellationToken = default
    )
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        var screenshotFileName = $"{fileName}_{timestamp}.png";
        var screenshotPath = Path.Combine(screenshotFolder, screenshotFileName);

        // Create directory if it doesn't exist
        Directory.CreateDirectory(screenshotFolder);

        await Page.ScreenshotAsync(screenshotPath, cancellationToken);

        return screenshotPath;
    }

    /// <summary>
    /// Takes a screenshot of the current page with a custom subfolder.
    /// Useful for organizing screenshots by test suite or test run.
    /// </summary>
    /// <param name="fileName">Base name for the screenshot file.</param>
    /// <param name="subfolder">Subfolder within the screenshots directory.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The full path to the saved screenshot.</returns>
    protected async Task<string> TakeScreenshotInSubfolderAsync(
        string fileName,
        string subfolder,
        CancellationToken cancellationToken = default
    )
    {
        var screenshotFolder = Path.Combine("screenshots", subfolder);
        return await TakeScreenshotAsync(fileName, screenshotFolder, cancellationToken);
    }

    /// <summary>
    /// Takes a screenshot when a test fails.
    /// Automatically includes "FAILED" prefix and stores in a "failures" subfolder.
    /// </summary>
    /// <param name="testName">Name of the failed test.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The full path to the saved screenshot.</returns>
    protected async Task<string> TakeFailureScreenshotAsync(
        string testName,
        CancellationToken cancellationToken = default
    )
    {
        var fileName = $"FAILED_{testName}";
        return await TakeScreenshotInSubfolderAsync(fileName, "failures", cancellationToken);
    }

    /// <summary>
    /// Gets the current page title.
    /// Useful for verification and debugging.
    /// </summary>
    protected async Task<string> GetPageTitleAsync(CancellationToken cancellationToken = default)
    {
        return await Page.GetTitleAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the current page URL.
    /// Useful for verification and debugging.
    /// </summary>
    protected async Task<string> GetPageUrlAsync(CancellationToken cancellationToken = default)
    {
        return await Page.GetUrlAsync(cancellationToken);
    }

    /// <summary>
    /// Waits for the page to be ready.
    /// </summary>
    protected async Task WaitForPageReadyAsync(
        int timeoutMs = 30000,
        CancellationToken cancellationToken = default
    )
    {
        // Override in derived classes if needed
        await Task.CompletedTask;
    }
}
