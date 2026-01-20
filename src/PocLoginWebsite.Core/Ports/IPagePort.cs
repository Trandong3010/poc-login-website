namespace PocLoginWebsite.Core.Ports;

/// <summary>
/// Port interface for page-level interactions.
/// Defines the contract for navigating and interacting with web pages.
/// </summary>
public interface IPagePort : IAsyncDisposable
{
    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task GotoAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current page title.
    /// </summary>
    Task<string> GetTitleAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current page URL.
    /// </summary>
    Task<string> GetUrlAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Waits for a selector to be present on the page.
    /// </summary>
    /// <param name="selector">CSS selector or text selector.</param>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task WaitForSelectorAsync(string selector, int timeoutMs = 30000, CancellationToken cancellationToken = default);

    /// <summary>
    /// Takes a screenshot of the current page.
    /// </summary>
    /// <param name="path">Path to save the screenshot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ScreenshotAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an element port for interacting with a specific element.
    /// </summary>
    /// <param name="selector">CSS selector for the element.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IElementPort> GetElementAsync(string selector, CancellationToken cancellationToken = default);

    /// <summary>
    /// Evaluates JavaScript in the page context and returns the result.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="script">JavaScript code to evaluate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the JavaScript evaluation.</returns>
    Task<T> EvaluateAsync<T>(string script, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the current page.
    /// </summary>
    Task CloseAsync(CancellationToken cancellationToken = default);
}
