namespace PocLoginWebsite.Core.Ports;

/// <summary>
/// Port interface for element-level interactions.
/// Defines the contract for interacting with web elements.
/// </summary>
public interface IElementPort
{
    /// <summary>
    /// Clicks on the element.
    /// </summary>
    Task ClickAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Fills the element with the specified text (for input elements).
    /// </summary>
    /// <param name="text">Text to fill.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task FillAsync(string text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the text content of the element.
    /// </summary>
    Task<string> GetTextAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the value of a specific attribute.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<string?> GetAttributeAsync(string attributeName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the element is visible.
    /// </summary>
    Task<bool> IsVisibleAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the element is enabled.
    /// </summary>
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Waits for the element to be visible.
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task WaitForVisibleAsync(int timeoutMs = 30000, CancellationToken cancellationToken = default);
}
