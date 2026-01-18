using PocLoginWebsite.Core.Ports;
using PocLoginWebsite.Infrastructure.PageObjects;
using PocLoginWebsite.Tests.Fixtures;

namespace PocLoginWebsite.Tests.Examples;

/// <summary>
/// Example test demonstrating the Page Object Model pattern.
/// This test shows how to use page objects for cleaner, more maintainable tests.
/// </summary>
[TestFixture]
public class LoginPageTests : BaseTestFixture
{
    private IPagePort? _page;
    private LoginPageObject? _loginPage;

    [SetUp]
    public async Task SetUp()
    {
        _page = await CreatePageAsync();
        _loginPage = CreateLoginPage(_page);
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }
    }

    [Test]
    public async Task NavigateToLoginPage_ShouldDisplayLoginForm()
    {
        // Act
        await _loginPage!.NavigateAsync();
        var isLoaded = await _loginPage.IsPageLoadedAsync();

        // Assert
        Assert.That(isLoaded, Is.True, "Login page should be loaded");
    }

    [Test]
    [Ignore("This is a demo test - requires actual login page")]
    public async Task LoginWithValidCredentials_ShouldSucceed()
    {
        // Arrange
        await _loginPage!.NavigateAsync();

        // Act
        await _loginPage.LoginAsync("testuser", "testpassword");

        // Wait for navigation or success indicator
        await Task.Delay(1000);

        var errorMessage = await _loginPage.GetErrorMessageAsync();

        // Assert
        Assert.That(errorMessage, Is.Null, "Should not display error message on successful login");
    }

    [Test]
    [Ignore("This is a demo test - requires actual login page")]
    public async Task LoginWithInvalidCredentials_ShouldShowError()
    {
        // Arrange
        await _loginPage!.NavigateAsync();

        // Act
        await _loginPage.LoginAsync("invalid", "invalid");

        // Wait for error message
        await Task.Delay(1000);

        var errorMessage = await _loginPage.GetErrorMessageAsync();

        // Assert
        Assert.That(errorMessage, Is.Not.Null, "Should display error message on failed login");
    }
}
