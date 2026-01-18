using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for orchestrating test execution.
/// Provides centralized control over browser lifecycle and test flow.
/// </summary>
public class TestOrchestrator(IBrowserPort browserPort, IConfigurationPort configurationPort)
{
    /// <summary>
    /// Initializes the browser for testing.
    /// </summary>
    public async Task InitializeBrowserAsync(CancellationToken cancellationToken = default)
    {
        await browserPort.LaunchAsync(
            configurationPort.BrowserType,
            configurationPort.Headless,
            cancellationToken
        );
    }

    /// <summary>
    /// Creates a new page for testing.
    /// </summary>
    public async Task<IPagePort> CreatePageAsync(CancellationToken cancellationToken = default)
    {
        return await browserPort.NewPageAsync(cancellationToken);
    }

    /// <summary>
    /// Cleans up browser resources.
    /// </summary>
    public async Task CleanupAsync(CancellationToken cancellationToken = default)
    {
        await browserPort.CloseAsync(cancellationToken);
    }
}
