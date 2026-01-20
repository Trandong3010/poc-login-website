using PocLoginWebsite.Core.Common;
using PocLoginWebsite.Core.Extensions;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.PageObjects;

/// <summary>
/// Page object for SauceDemo login page.
/// Provides methods to interact with the login form.
/// </summary>
public class LoginPageObject
{
    private readonly IPagePort _page;
    private readonly IConfigurationPort _configuration;

    // Selectors for SauceDemo website
    private const string UsernameInputSelector = "#user-name";
    private const string PasswordInputSelector = "#password";
    private const string LoginButtonSelector = "#login-button";
    private const string ErrorMessageSelector = "[data-test='error']";

    public LoginPageObject(IPagePort page, IConfigurationPort configuration)
    {
        _page = page ?? throw new ArgumentNullException(nameof(page));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public virtual async Task NavigateAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_configuration.BaseUrl))
        {
            throw new InvalidOperationException("BaseUrl is not configured");
        }

        await _page.GotoAsync(_configuration.BaseUrl, cancellationToken);
    }

    public async Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _page.WaitForSelectorAsync(
                UsernameInputSelector,
                _configuration.DefaultTimeout,
                cancellationToken
            );
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        var usernameInput = await _page.GetElementAsync(UsernameInputSelector, cancellationToken);
        await usernameInput.FillAsync(username, cancellationToken);

        var passwordInput = await _page.GetElementAsync(PasswordInputSelector, cancellationToken);
        await passwordInput.FillAsync(password, cancellationToken);

        var loginButton = await _page.GetElementAsync(LoginButtonSelector, cancellationToken);
        await loginButton.ClickAsync(cancellationToken);
    }

    public async Task<string?> GetErrorMessageAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var errorElement = await _page.GetElementAsync(ErrorMessageSelector, cancellationToken);
            return await errorElement.GetTextAsync(cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Waits for successful login by checking for navigation to the products page.
    /// </summary>
    public async Task<bool> WaitForLoginSuccessAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Wait for URL to change to inventory page
            await Task.Delay(1000, cancellationToken); // Brief delay for navigation
            var currentUrl = await _page.GetUrlAsync(cancellationToken);
            return currentUrl.Contains("inventory.html");
        }
        catch
        {
            return false;
        }
    }
}
