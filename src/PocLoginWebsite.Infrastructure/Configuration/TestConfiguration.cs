using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Configuration;

/// <summary>
/// Configuration provider for test settings.
/// Can be extended to read from appsettings.json or environment variables.
/// </summary>
public class TestConfiguration : IConfigurationPort
{
    private readonly Dictionary<string, string> _configuration;

    public TestConfiguration()
    {
        _configuration = new Dictionary<string, string>
        {
            { "BaseUrl", "https://example.com" },
            { "DefaultTimeout", "30000" },
            { "Headless", "true" },
            { "BrowserType", "chromium" }
        };
    }

    public string? GetValue(string key)
    {
        return _configuration.TryGetValue(key, out var value) ? value : null;
    }

    public T GetValue<T>(string key, T defaultValue)
    {
        if (!_configuration.TryGetValue(key, out var value))
            return defaultValue;

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    public string BaseUrl => GetValue("BaseUrl") ?? "https://example.com";

    public int DefaultTimeout => GetValue("DefaultTimeout", 30000);

    public bool Headless => GetValue("Headless", true);

    public string BrowserType => GetValue("BrowserType") ?? "chromium";

    /// <summary>
    /// Sets a configuration value (useful for testing or runtime configuration).
    /// </summary>
    public void SetValue(string key, string value)
    {
        _configuration[key] = value;
    }
}
