using Microsoft.Extensions.Configuration;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Configuration;

/// <summary>
/// Unified configuration service that reads from appsettings.json.
/// Implements IConfigurationPort for use across the application.
/// </summary>
public class AppConfiguration : IConfigurationPort
{
    private readonly IConfiguration _configuration;

    public AppConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppConfiguration() : this(BuildConfiguration())
    {
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public string? GetValue(string key) => _configuration[key];

    public T GetValue<T>(string key, T defaultValue)
    {
        var value = _configuration[key];
        if (string.IsNullOrEmpty(value))
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

    // SauceDemo Configuration
    public string SauceDemoUrl => GetValue("SauceDemo:BaseUrl") ?? "https://www.saucedemo.com/";
    public string SauceDemoUsername => GetValue("SauceDemo:Username") ?? "standard_user";
    public string SauceDemoPassword => GetValue("SauceDemo:Password") ?? "secret_sauce";

    // Browser Configuration
    public string BrowserType => GetValue("Browser:Type") ?? "chromium";
    public bool Headless => GetValue("Browser:Headless", true);
    public int DefaultTimeout => GetValue("Browser:DefaultTimeout", 30000);

    // Stealth Configuration
    public bool StealthEnabled => GetValue("Stealth:Enabled", true);

    public string StealthUserAgent => GetValue("Stealth:UserAgent") ??
                                      "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    public int StealthViewportWidth => GetValue("Stealth:ViewportWidth", 1920);
    public int StealthViewportHeight => GetValue("Stealth:ViewportHeight", 1080);

    // Proxy Configuration
    public bool ProxyEnabled => GetValue("Proxy:Enabled", false);
    public string? ProxyServer => GetValue("Proxy:Server");
    public string? ProxyUsername => GetValue("Proxy:Username");
    public string? ProxyPassword => GetValue("Proxy:Password");

    // Browser State Configuration
    public bool BrowserStateEnabled => GetValue("BrowserState:Enabled", false);
    public string BrowserStatePath => GetValue("BrowserState:StatePath") ?? "state.json";

    // IConfigurationPort implementation
    public string BaseUrl => SauceDemoUrl;
}
