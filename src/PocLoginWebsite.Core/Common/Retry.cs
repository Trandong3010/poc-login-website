namespace PocLoginWebsite.Core.Common;

/// <summary>
/// Retry logic with exponential backoff.
/// </summary>
public static class Retry
{
    /// <summary>
    /// Executes an async operation with retry logic.
    /// </summary>
    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        int maxAttempts = 3,
        TimeSpan? initialDelay = null,
        Func<Exception, bool>? shouldRetry = null,
        CancellationToken cancellationToken = default)
    {
        var delay = initialDelay ?? TimeSpan.FromMilliseconds(100);
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                // Check if we should retry this exception
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    throw;
                }

                lastException = ex;
                await Task.Delay(delay, cancellationToken);
                delay = WithExponentialBackoff(delay, attempt);
            }
        }

        throw new InvalidOperationException(
            $"Operation failed after {maxAttempts} attempts",
            lastException);
    }

    /// <summary>
    /// Executes an async operation with retry logic (non-generic version).
    /// </summary>
    public static async Task ExecuteAsync(
        Func<Task> operation,
        int maxAttempts = 3,
        TimeSpan? initialDelay = null,
        Func<Exception, bool>? shouldRetry = null,
        CancellationToken cancellationToken = default)
    {
        var delay = initialDelay ?? TimeSpan.FromMilliseconds(100);
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await operation();
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    throw;
                }

                lastException = ex;
                await Task.Delay(delay, cancellationToken);
                delay = WithExponentialBackoff(delay, attempt);
            }
        }

        throw new InvalidOperationException(
            $"Operation failed after {maxAttempts} attempts",
            lastException);
    }

    /// <summary>
    /// Calculates exponential backoff delay.
    /// </summary>
    public static TimeSpan WithExponentialBackoff(TimeSpan currentDelay, int attempt)
    {
        var multiplier = Math.Pow(2, attempt - 1);
        var newDelay = TimeSpan.FromMilliseconds(currentDelay.TotalMilliseconds * multiplier);
        
        // Cap at 30 seconds
        return newDelay > TimeSpan.FromSeconds(30) 
            ? TimeSpan.FromSeconds(30) 
            : newDelay;
    }
}
