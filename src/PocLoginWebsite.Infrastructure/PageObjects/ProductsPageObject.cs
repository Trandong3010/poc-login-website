using PocLoginWebsite.Core.Common;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.PageObjects;

/// <summary>
/// Page object for the SauceDemo products page (post-login).
/// Provides methods to interact with and extract data from the products page.
/// </summary>
public class ProductsPageObject
{
    private readonly IPagePort _page;

    private const string PageTitleSelector = ".title";
    private const string InventoryItemSelector = ".inventory_item";
    private const string ProductNameSelector = ".inventory_item_name";

    public ProductsPageObject(IPagePort page)
    {
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public async Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _page.WaitForSelectorAsync(
                PageTitleSelector,
                30000,
                cancellationToken
            );
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the page title text (e.g., "Products").
    /// </summary>
    public async Task<string> GetPageTitleAsync(CancellationToken cancellationToken = default)
    {
        var titleElement = await _page.GetElementAsync(PageTitleSelector, cancellationToken);
        return await titleElement.GetTextAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the page title using JavaScript evaluation.
    /// Demonstrates the JavaScript evaluation capability.
    /// </summary>
    public async Task<string> GetPageTitleViaJavaScriptAsync(CancellationToken cancellationToken = default)
    {
        return await _page.EvaluateAsync<string>(
            $"document.querySelector('{PageTitleSelector}').textContent",
            cancellationToken
        );
    }

    /// <summary>
    /// Gets the count of products on the page.
    /// </summary>
    public async Task<int> GetProductCountAsync(CancellationToken cancellationToken = default)
    {
        return await _page.EvaluateAsync<int>(
            $"document.querySelectorAll('{InventoryItemSelector}').length",
            cancellationToken
        );
    }

    /// <summary>
    /// Extracts all product names from the page using JavaScript.
    /// </summary>
    public async Task<string[]> GetProductNamesAsync(CancellationToken cancellationToken = default)
    {
        return await _page.EvaluateAsync<string[]>(
            $@"Array.from(document.querySelectorAll('{ProductNameSelector}'))
                .map(el => el.textContent)",
            cancellationToken
        );
    }

    /// <summary>
    /// Verifies that the navigator.webdriver property is undefined (stealth check).
    /// </summary>
    public async Task<bool> IsStealthActiveAsync(CancellationToken cancellationToken = default)
    {
        var webdriverValue = await _page.EvaluateAsync<object>(
            "navigator.webdriver",
            cancellationToken
        );
        return webdriverValue == null;
    }
}
