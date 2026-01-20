namespace PocLoginWebsite.Core.Models;

/// <summary>
/// Configuration options for browser stealth features.
/// Used to configure anti-detection mechanisms.
/// </summary>
public class StealthOptions
{
    /// <summary>
    /// Custom User-Agent string to use.
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Viewport width in pixels.
    /// </summary>
    public int ViewportWidth { get; set; } = 1920;

    /// <summary>
    /// Viewport height in pixels.
    /// </summary>
    public int ViewportHeight { get; set; } = 1080;

    /// <summary>
    /// Whether to enable stealth mode (removes navigator.webdriver, etc.).
    /// </summary>
    public bool EnableStealth { get; set; } = true;

    /// <summary>
    /// Gets or sets the proxy server URL (e.g., "http://proxy:8080").
    /// </summary>
    public string? ProxyServer { get; set; }

    /// <summary>
    /// Gets or sets the proxy username for authentication.
    /// </summary>
    public string? ProxyUsername { get; set; }

    /// <summary>
    /// Gets or sets the proxy password for authentication.
    /// </summary>
    public string? ProxyPassword { get; set; }

    /// <summary>
    /// Gets the default stealth options with recommended settings.
    /// </summary>
    public static StealthOptions Default => new()
    {
        UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        ViewportWidth = 1920,
        ViewportHeight = 1080,
        EnableStealth = true,
        ProxyServer = null,
        ProxyUsername = null,
        ProxyPassword = null
    };
}
