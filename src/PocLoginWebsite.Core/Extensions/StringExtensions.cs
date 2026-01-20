namespace PocLoginWebsite.Core.Extensions;

/// <summary>
/// Extension methods for string operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Checks if a string is null or whitespace.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Checks if a string contains another string, ignoring case.
    /// </summary>
    public static bool ContainsIgnoreCase(this string source, string value)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (value == null) throw new ArgumentNullException(nameof(value));

        return source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if a string equals another string, ignoring case.
    /// </summary>
    public static bool EqualsIgnoreCase(this string source, string value)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (value == null) throw new ArgumentNullException(nameof(value));

        return source.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Truncates a string to a maximum length and adds ellipsis if needed.
    /// </summary>
    public static string Truncate(this string value, int maxLength, string ellipsis = "...")
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (maxLength < 0) throw new ArgumentException("Max length must be non-negative", nameof(maxLength));

        if (value.Length <= maxLength)
            return value;

        return value.Substring(0, Math.Max(0, maxLength - ellipsis.Length)) + ellipsis;
    }

    /// <summary>
    /// Converts a string to title case.
    /// </summary>
    public static string ToTitleCase(this string value)
    {
        if (value.IsNullOrWhiteSpace())
            return value;

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Ensures a string is not null, returning empty string if null.
    /// </summary>
    public static string OrEmpty(this string? value)
    {
        return value ?? string.Empty;
    }
}
