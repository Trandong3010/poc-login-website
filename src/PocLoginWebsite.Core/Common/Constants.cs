namespace PocLoginWebsite.Core.Common;

/// <summary>
/// Common constants used throughout the application.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Default timeout values.
    /// </summary>
    public static class Timeouts
    {
        public static readonly TimeSpan Default = TimeSpan.FromSeconds(30);
        public static readonly TimeSpan Short = TimeSpan.FromSeconds(10);
        public static readonly TimeSpan Long = TimeSpan.FromSeconds(60);
        public static readonly TimeSpan PageLoad = TimeSpan.FromSeconds(30);
        public static readonly TimeSpan ElementWait = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Retry configuration.
    /// </summary>
    public static class RetryConfig
    {
        public const int DefaultMaxAttempts = 3;
        public const int DefaultDelayMs = 100;
    }

    /// <summary>
    /// Common CSS selectors.
    /// </summary>
    public static class Selectors
    {
        public const string Button = "button";
        public const string Input = "input";
        public const string Link = "a";
        public const string Form = "form";
    }

    /// <summary>
    /// Browser configuration.
    /// </summary>
    public static class Browser
    {
        public const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
        public const int DefaultViewportWidth = 1920;
        public const int DefaultViewportHeight = 1080;
    }
}
