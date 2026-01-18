using PocLoginWebsite.Application.PageObjects;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.PageObjects;

/// <summary>
/// Example page object for a login page.
/// Demonstrates how to implement concrete page objects using the base class.
/// </summary>
public class LoginPageObject(IPagePort page, IConfigurationPort configuration)
    : BasePageObject(page)
{
    // Selectors for SauceDemo website
    private const string UsernameInputSelector = "#user-name";
    private const string PasswordInputSelector = "#password";
    private const string LoginButtonSelector = "#login-button";
    private const string ErrorMessageSelector = "[data-test='error']";

    public virtual async Task NavigateAsync(CancellationToken cancellationToken = default)
    {
        await Page.GotoAsync(configuration.BaseUrl, cancellationToken);
    }

    public override async Task<bool> IsPageLoadedAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await Page.WaitForSelectorAsync(
                UsernameInputSelector,
                configuration.DefaultTimeout,
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
        var usernameInput = await Page.GetElementAsync(UsernameInputSelector, cancellationToken);
        await usernameInput.FillAsync(username, cancellationToken);

        var passwordInput = await Page.GetElementAsync(PasswordInputSelector, cancellationToken);
        await passwordInput.FillAsync(password, cancellationToken);

        var loginButton = await Page.GetElementAsync(LoginButtonSelector, cancellationToken);
        await loginButton.ClickAsync(cancellationToken);
    }

    public async Task<string?> GetErrorMessageAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var errorElement = await Page.GetElementAsync(ErrorMessageSelector, cancellationToken);
            return await errorElement.GetTextAsync(cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}
