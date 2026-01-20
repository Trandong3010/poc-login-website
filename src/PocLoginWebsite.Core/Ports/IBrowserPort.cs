namespace PocLoginWebsite.Core.Ports;

/// <summary>
/// Port interface for browser lifecycle management.
/// Defines the contract for creating, managing, and disposing browser instances.
/// </summary>
public interface IBrowserPort : IAsyncDisposable
{
    /// <summary>
    /// Initializes and launches a new browser instance.
    /// </summary>
    /// <param name="browserType">Type of browser to launch (chromium, firefox, webkit).</param>
    /// <param name="headless">Whether to run the browser in headless mode.</param>
    /// <param name="stealthOptions">Optional stealth configuration for anti-detection.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task LaunchAsync(string browserType = "chromium", bool headless = true,
        Models.StealthOptions? stealthOptions = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new browser context with optional configuration.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new page port representing the browser context.</returns>
    Task<IPagePort> NewPageAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes all browser instances and contexts.
    /// </summary>
    Task CloseAsync(CancellationToken cancellationToken = default);
}
