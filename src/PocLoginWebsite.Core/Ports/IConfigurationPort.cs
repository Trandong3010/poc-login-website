namespace PocLoginWebsite.Core.Ports;

/// <summary>
/// Port interface for accessing test configuration.
/// Defines the contract for retrieving configuration values.
/// </summary>
public interface IConfigurationPort
{
    /// <summary>
    /// Gets a configuration value by key.
    /// </summary>
    /// <param name="key">Configuration key.</param>
    /// <returns>Configuration value or null if not found.</returns>
    string? GetValue(string key);

    /// <summary>
    /// Gets a configuration value by key with a default value.
    /// </summary>
    /// <param name="key">Configuration key.</param>
    /// <param name="defaultValue">Default value if key not found.</param>
    T GetValue<T>(string key, T defaultValue);

    /// <summary>
    /// Gets the base URL for the application under test.
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Gets the default timeout in milliseconds.
    /// </summary>
    int DefaultTimeout { get; }

    /// <summary>
    /// Gets whether to run tests in headless mode.
    /// </summary>
    bool Headless { get; }

    /// <summary>
    /// Gets the browser type to use (chromium, firefox, webkit).
    /// </summary>
    string BrowserType { get; }
}
