using Microsoft.Playwright;
using PocLoginWebsite.Core.Ports;

namespace PocLoginWebsite.Infrastructure.Adapters;

/// <summary>
/// Playwright implementation of IBrowserPort.
/// Manages browser lifecycle using Playwright.
/// </summary>
public class PlaywrightBrowserAdapter : IBrowserPort
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;

    public async Task LaunchAsync(
        string browserType = "chromium",
        bool headless = true,
        Core.Models.StealthOptions? stealthOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        _playwright = await Playwright.CreateAsync();

        // Use default stealth options if none provided
        stealthOptions ??= Core.Models.StealthOptions.Default;

        _browser = browserType.ToLower() switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
            "webkit" => await _playwright.Webkit.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
            _ => await _playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = headless }
            ),
        };

        // Create browser context with stealth options
        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize
            {
                Width = stealthOptions.ViewportWidth,
                Height = stealthOptions.ViewportHeight
            }
        };

        // Set custom user agent if provided
        if (!string.IsNullOrEmpty(stealthOptions.UserAgent))
        {
            contextOptions.UserAgent = stealthOptions.UserAgent;
        }

        // Configure proxy if provided
        if (!string.IsNullOrEmpty(stealthOptions.ProxyServer))
        {
            contextOptions.Proxy = new Proxy
            {
                Server = stealthOptions.ProxyServer,
                Username = stealthOptions.ProxyUsername,
                Password = stealthOptions.ProxyPassword
            };
        }

        _context = await _browser.NewContextAsync(contextOptions);

        // Apply stealth scripts if enabled
        if (stealthOptions.EnableStealth)
        {
            await _context.AddInitScriptAsync(@"
                // Remove navigator.webdriver property
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => undefined
                });

                // Spoof Chrome runtime
                window.chrome = {
                    runtime: {}
                };

                // Spoof permissions
                const originalQuery = window.navigator.permissions.query;
                window.navigator.permissions.query = (parameters) => (
                    parameters.name === 'notifications' ?
                        Promise.resolve({ state: Notification.permission }) :
                        originalQuery(parameters)
                );

                // Spoof plugins
                Object.defineProperty(navigator, 'plugins', {
                    get: () => [1, 2, 3, 4, 5]
                });

                // Spoof languages
                Object.defineProperty(navigator, 'languages', {
                    get: () => ['en-US', 'en']
                });

                // Canvas fingerprint protection (Bài 4 - Cloudflare bypass)
                const originalToDataURL = HTMLCanvasElement.prototype.toDataURL;
                HTMLCanvasElement.prototype.toDataURL = function(type) {
                    const context = this.getContext('2d');
                    if (context) {
                        const imageData = context.getImageData(0, 0, this.width, this.height);
                        for (let i = 0; i < imageData.data.length; i += 4) {
                            imageData.data[i] = imageData.data[i] ^ 1;
                        }
                        context.putImageData(imageData, 0, 0);
                    }
                    return originalToDataURL.apply(this, arguments);
                };

                // WebGL fingerprint protection
                const getParameter = WebGLRenderingContext.prototype.getParameter;
                WebGLRenderingContext.prototype.getParameter = function(parameter) {
                    if (parameter === 37445) {
                        return 'Intel Inc.';
                    }
                    if (parameter === 37446) {
                        return 'Intel Iris OpenGL Engine';
                    }
                    return getParameter.apply(this, arguments);
                };

                // Audio context fingerprint protection
                const AudioContext = window.AudioContext || window.webkitAudioContext;
                if (AudioContext) {
                    const originalCreateAnalyser = AudioContext.prototype.createAnalyser;
                    AudioContext.prototype.createAnalyser = function() {
                        const analyser = originalCreateAnalyser.apply(this, arguments);
                        const originalGetFloatFrequencyData = analyser.getFloatFrequencyData;
                        analyser.getFloatFrequencyData = function(array) {
                            originalGetFloatFrequencyData.apply(this, arguments);
                            for (let i = 0; i < array.length; i++) {
                                array[i] = array[i] + Math.random() * 0.0001;
                            }
                        };
                        return analyser;
                    };
                }
            ");
        }
    }

    public async Task<IPagePort> NewPageAsync(CancellationToken cancellationToken = default)
    {
        if (_context == null)
        {
            throw new InvalidOperationException(
                "Browser context is not initialized. Call LaunchAsync first."
            );
        }

        var page = await _context.NewPageAsync();
        return new PlaywrightPageAdapter(page);
    }

    public async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (_context != null)
        {
            await _context.CloseAsync();
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    /// <summary>
    /// Saves the current browser state (cookies, localStorage, etc.) to a file.
    /// </summary>
    public async Task SaveStateAsync(string path, CancellationToken cancellationToken = default)
    {
        if (_context == null)
        {
            throw new InvalidOperationException("Browser context is not initialized.");
        }

        await _context.StorageStateAsync(new()
        {
            Path = path
        });
    }

    /// <summary>
    /// Creates a new page with previously saved browser state.
    /// </summary>
    public async Task<IPagePort> NewPageWithStateAsync(
        string statePath,
        CancellationToken cancellationToken = default)
    {
        if (_browser == null)
        {
            throw new InvalidOperationException("Browser is not launched.");
        }

        if (!File.Exists(statePath))
        {
            throw new FileNotFoundException($"State file not found: {statePath}");
        }

        // Create new context with saved state
        var context = await _browser.NewContextAsync(new()
        {
            StorageStatePath = statePath
        });

        var page = await context.NewPageAsync();
        return new PlaywrightPageAdapter(page);
    }

    /// <summary>
    /// Gets the current browser context (for advanced scenarios).
    /// </summary>
    public IBrowserContext? GetContext() => _context;

    public async ValueTask DisposeAsync()
    {
        await CloseAsync();
    }
}
