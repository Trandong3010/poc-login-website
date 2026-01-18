using PocLoginWebsite.Core.Ports;
using PocLoginWebsite.Tests.Fixtures;

namespace PocLoginWebsite.Tests.Examples;

/// <summary>
/// Example test class demonstrating basic page navigation and interactions.
/// This test uses the Hexagonal Architecture pattern with Playwright adapters.
/// </summary>
[TestFixture]
public class ExampleTests : BaseTestFixture
{
    private IPagePort? _page;

    [SetUp]
    public async Task SetUp()
    {
        // Create a new page for each test
        _page = await CreatePageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        // Close the page after each test
        if (_page != null)
        {
            await _page.CloseAsync();
        }
    }

    [Test]
    public async Task NavigateToUrl_ShouldLoadPage()
    {
        // Arrange
        const string url = "https://www.example.com";

        // Act
        await _page!.GotoAsync(url);
        var title = await _page.GetTitleAsync();
        var currentUrl = await _page.GetUrlAsync();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(title, Is.Not.Empty, "Page title should not be empty");
            Assert.That(currentUrl, Does.Contain("example.com"), "URL should contain example.com");
        });
    }

    [Test]
    public async Task GetElement_ShouldFindElement()
    {
        // Arrange
        await _page!.GotoAsync("https://www.example.com");

        // Act
        var element = await _page.GetElementAsync("h1");
        var isVisible = await element.IsVisibleAsync();
        var text = await element.GetTextAsync();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(isVisible, Is.True, "H1 element should be visible");
            Assert.That(text, Is.Not.Empty, "H1 text should not be empty");
        });
    }

    [Test]
    public async Task TakeScreenshot_ShouldCreateFile()
    {
        // Arrange
        await _page!.GotoAsync("https://www.example.com");
        var screenshotPath = Path.Combine("screenshots", "example_test.png");
        Directory.CreateDirectory("screenshots");

        // Act
        await _page.ScreenshotAsync(screenshotPath);

        // Assert
        Assert.That(File.Exists(screenshotPath), Is.True, "Screenshot file should exist");

        // Cleanup
        if (File.Exists(screenshotPath))
        {
            File.Delete(screenshotPath);
        }
    }
}
