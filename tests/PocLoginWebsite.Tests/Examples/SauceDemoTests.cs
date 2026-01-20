using NUnit.Framework;
using PocLoginWebsite.Core.Models;
using PocLoginWebsite.Core.Ports;
using PocLoginWebsite.Infrastructure.Adapters;
using PocLoginWebsite.Infrastructure.Configuration;
using PocLoginWebsite.Infrastructure.PageObjects;

namespace PocLoginWebsite.Tests.Examples;

/// <summary>
/// Comprehensive test suite for SauceDemo website automation with stealth features.
/// </summary>
[TestFixture]
public class SauceDemoTests
{
    private IBrowserPort? _browser;
    private IPagePort? _page;
    private IConfigurationPort? _config;

    [SetUp]
    public async Task SetUp()
    {
        _config = new TestConfiguration();
        _browser = new PlaywrightBrowserAdapter();
        
        // Launch with stealth options
        var stealthOptions = StealthOptions.Default;
        await _browser.LaunchAsync("chromium", headless: true, stealthOptions: stealthOptions);
        
        _page = await _browser.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
    }

    [Test]
    [Category("SauceDemo")]
    public async Task LoginWithValidCredentials_ShouldSucceed()
    {
        // Arrange
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();
        await loginPage.IsPageLoadedAsync();

        // Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        var loginSuccess = await loginPage.WaitForLoginSuccessAsync();

        // Assert
        Assert.That(loginSuccess, Is.True, "Login should succeed with valid credentials");
        
        var currentUrl = await _page!.GetUrlAsync();
        Assert.That(currentUrl, Does.Contain("inventory.html"), "Should navigate to inventory page");
    }

    [Test]
    [Category("SauceDemo")]
    public async Task LoginWithInvalidCredentials_ShouldShowError()
    {
        // Arrange
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();
        await loginPage.IsPageLoadedAsync();

        // Act
        await loginPage.LoginAsync("invalid_user", "wrong_password");
        await Task.Delay(1000); // Wait for error message

        var errorMessage = await loginPage.GetErrorMessageAsync();

        // Assert
        Assert.That(errorMessage, Is.Not.Null, "Should display error message");
        Assert.That(errorMessage, Does.Contain("do not match"), "Error message should indicate credential mismatch");
    }

    [Test]
    [Category("SauceDemo")]
    [Category("DataExtraction")]
    public async Task ExtractProductData_ShouldReturnProducts()
    {
        // Arrange - Login first
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await loginPage.WaitForLoginSuccessAsync();

        // Act
        var productsPage = new ProductsPageObject(_page!);
        await productsPage.IsPageLoadedAsync();

        var pageTitle = await productsPage.GetPageTitleAsync();
        var productCount = await productsPage.GetProductCountAsync();
        var productNames = await productsPage.GetProductNamesAsync();

        // Assert
        Assert.That(pageTitle, Is.EqualTo("Products"), "Page title should be 'Products'");
        Assert.That(productCount, Is.GreaterThan(0), "Should have products on the page");
        Assert.That(productNames, Is.Not.Empty, "Should extract product names");
        Assert.That(productNames.Length, Is.EqualTo(productCount), "Product names count should match product count");
    }

    [Test]
    [Category("SauceDemo")]
    [Category("JavaScript")]
    public async Task JavaScriptEvaluation_ShouldExtractData()
    {
        // Arrange - Login first
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await loginPage.WaitForLoginSuccessAsync();

        // Act
        var productsPage = new ProductsPageObject(_page!);
        await productsPage.IsPageLoadedAsync();

        var titleViaJS = await productsPage.GetPageTitleViaJavaScriptAsync();
        var titleViaElement = await productsPage.GetPageTitleAsync();

        // Assert
        Assert.That(titleViaJS, Is.EqualTo(titleViaElement), "JavaScript evaluation should return same result as element query");
        Assert.That(titleViaJS, Is.EqualTo("Products"), "JavaScript should extract correct title");
    }

    [Test]
    [Category("Stealth")]
    public async Task StealthFeatures_NavigatorWebdriverShouldBeUndefined()
    {
        // Arrange
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();

        // Act
        var webdriverValue = await _page!.EvaluateAsync<object>("navigator.webdriver");

        // Assert
        Assert.That(webdriverValue, Is.Null, "navigator.webdriver should be undefined (null) when stealth is active");
    }

    [Test]
    [Category("Stealth")]
    [Category("SauceDemo")]
    public async Task StealthFeatures_ShouldRemainActiveAfterLogin()
    {
        // Arrange
        var loginPage = new LoginPageObject(_page!, _config!);
        await loginPage.NavigateAsync();
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await loginPage.WaitForLoginSuccessAsync();

        // Act
        var productsPage = new ProductsPageObject(_page!);
        var isStealthActive = await productsPage.IsStealthActiveAsync();

        // Assert
        Assert.That(isStealthActive, Is.True, "Stealth features should remain active after login");
    }

    [Test]
    [Category("SauceDemo")]
    public async Task PageLoad_ShouldCompleteWithinTimeout()
    {
        // Arrange
        var loginPage = new LoginPageObject(_page!, _config!);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        await loginPage.NavigateAsync();
        var isLoaded = await loginPage.IsPageLoadedAsync();
        stopwatch.Stop();

        // Assert
        Assert.That(isLoaded, Is.True, "Page should load successfully");
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(10000), "Page should load within 10 seconds");
    }
}
