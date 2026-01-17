using NUnit.Framework;
using PocLoginWebsite.Application.Services;
using PocLoginWebsite.Core.Ports;
using PocLoginWebsite.Infrastructure.Adapters;
using PocLoginWebsite.Infrastructure.Configuration;
using PocLoginWebsite.Infrastructure.PageObjects;

namespace PocLoginWebsite.Tests;

/// <summary>
/// Base class for all test fixtures.
/// Provides common setup and teardown for browser and dependency injection.
/// </summary>
public abstract class BaseTestFixture
{
    protected IBrowserPort BrowserPort { get; private set; } = null!;
    protected IConfigurationPort ConfigurationPort { get; private set; } = null!;
    protected TestOrchestrator TestOrchestrator { get; private set; } = null!;

    [OneTimeSetUp]
    public virtual async Task OneTimeSetUp()
    {
        // Initialize configuration
        ConfigurationPort = new TestConfiguration();

        // Initialize browser port
        BrowserPort = new PlaywrightBrowserAdapter();

        // Initialize test orchestrator
        TestOrchestrator = new TestOrchestrator(BrowserPort, ConfigurationPort);

        // Launch browser once for all tests in the fixture
        await TestOrchestrator.InitializeBrowserAsync();
    }

    [OneTimeTearDown]
    public virtual async Task OneTimeTearDown()
    {
        // Clean up browser resources
        await TestOrchestrator.CleanupAsync();
        
        // Dispose browser port
        if (BrowserPort != null)
        {
            await BrowserPort.DisposeAsync();
        }
    }

    /// <summary>
    /// Creates a new page for testing.
    /// </summary>
    protected async Task<IPagePort> CreatePageAsync()
    {
        return await TestOrchestrator.CreatePageAsync();
    }

    /// <summary>
    /// Creates a login page object.
    /// </summary>
    protected LoginPageObject CreateLoginPage(IPagePort page)
    {
        return new LoginPageObject(page, ConfigurationPort);
    }
}
