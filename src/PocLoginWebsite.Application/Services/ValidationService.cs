using PocLoginWebsite.Core.Common;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for advanced validation.
/// </summary>
public class ValidationService
{
    /// <summary>
    /// Validates that a price is not negative.
    /// </summary>
    public decimal ValidatePrice(decimal price)
    {
        Guard.Against.Negative((int)price, nameof(price));
        return price;
    }

    /// <summary>
    /// Validates that a quantity is within acceptable range.
    /// </summary>
    public int ValidateQuantity(int quantity, int min = 0, int max = 1000)
    {
        return Guard.Against.OutOfRange(quantity, min, max, nameof(quantity));
    }

    /// <summary>
    /// Validates viewport dimensions.
    /// </summary>
    public (int Width, int Height) ValidateViewport(int width, int height)
    {
        var validWidth = Guard.Against.OutOfRange(width, 800, 3840, nameof(width));
        var validHeight = Guard.Against.OutOfRange(height, 600, 2160, nameof(height));
        return (validWidth, validHeight);
    }

    /// <summary>
    /// Validates timeout value.
    /// </summary>
    public int ValidateTimeout(int timeoutMs, int minMs = 1000, int maxMs = 60000)
    {
        return Guard.Against.OutOfRange(timeoutMs, minMs, maxMs, nameof(timeoutMs));
    }
}
