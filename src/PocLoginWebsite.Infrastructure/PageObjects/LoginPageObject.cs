using PocLoginWebsite.Application.PageObjects;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.PageObjects;

/// <summary>
/// Example page object for a login page.
/// Demonstrates how to implement concrete page objects using the base class.
/// </summary>
public class LoginPageObject : BasePageObject
{
    private readonly IConfigurationPort _configuration;

    // Selectors
    private const string UsernameInputSelector = "#username";
    private const string PasswordInputSelector = "#password";
    private const string LoginButtonSelector = "button[type='submit']";
    private const string ErrorMessageSelector = ".error-message";

    public LoginPageObject(IPagePort page, IConfigurationPort configuration) : base(page)
    {
        _configuration = configuration;
    }

    public override async Task NavigateAsync(CancellationToken cancellationToken = default)
    {
        await Page.GotoAsync($"{_configuration.BaseUrl}/login", cancellationToken);
    }

    public override async Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await Page.WaitForSelectorAsync(UsernameInputSelector, _configuration.DefaultTimeout, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task LoginAsync(string username, string password, CancellationToken cancellationToken = default)
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

    public async Task<bool> IsUsernameInputVisibleAsync(CancellationToken cancellationToken = default)
    {
        var element = await Page.GetElementAsync(UsernameInputSelector, cancellationToken);
        return await element.IsVisibleAsync(cancellationToken);
    }
}
