using PocLoginWebsite.Core.Models;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for orchestrating test execution.
/// Provides centralized control over browser lifecycle and test flow.
/// </summary>
public class TestOrchestrator
{
    private readonly IBrowserPort _browserPort;
    private readonly IConfigurationPort _configurationPort;

    public TestOrchestrator(IBrowserPort browserPort, IConfigurationPort configurationPort)
    {
        _browserPort = browserPort;
        _configurationPort = configurationPort;
    }

    /// <summary>
    /// Initializes the browser for testing.
    /// </summary>
    public async Task InitializeBrowserAsync(CancellationToken cancellationToken = default)
    {
        await _browserPort.LaunchAsync(
            _configurationPort.BrowserType,
            _configurationPort.Headless,
            cancellationToken
        );
    }

    /// <summary>
    /// Creates a new page for testing.
    /// </summary>
    public async Task<IPagePort> CreatePageAsync(CancellationToken cancellationToken = default)
    {
        return await _browserPort.NewPageAsync(cancellationToken);
    }

    /// <summary>
    /// Executes a test action and returns the result.
    /// </summary>
    /// <param name="testAction">The test action to execute.</param>
    /// <param name="testName">Name of the test.</param>
    public async Task<TestResult> ExecuteTestAsync(
        Func<Task> testAction,
        string testName,
        CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            await testAction();
            var duration = DateTime.UtcNow - startTime;
            return TestResult.Passed($"Test '{testName}' completed successfully", duration);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            return TestResult.Failed($"Test '{testName}' failed: {ex.Message}", ex, duration);
        }
    }

    /// <summary>
    /// Cleans up browser resources.
    /// </summary>
    public async Task CleanupAsync(CancellationToken cancellationToken = default)
    {
        await _browserPort.CloseAsync(cancellationToken);
    }
}
