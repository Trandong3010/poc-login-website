namespace PocLoginWebsite.Core.Extensions;

/// <summary>
/// Extension methods for async operations.
/// </summary>
public static class AsyncExtensions
{
    /// <summary>
    /// Adds a timeout to a task.
    /// </summary>
    public static async Task<T> WithTimeout<T>(
        this Task<T> task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, linkedCts.Token));

        if (completedTask == task)
        {
            timeoutCts.Cancel(); // Cancel the delay task
            return await task;
        }

        throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
    }

    /// <summary>
    /// Adds a timeout to a task (non-generic version).
    /// </summary>
    public static async Task WithTimeout(
        this Task task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, linkedCts.Token));

        if (completedTask == task)
        {
            timeoutCts.Cancel();
            await task;
            return;
        }

        throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
    }

    /// <summary>
    /// Retries an async operation with exponential backoff.
    /// </summary>
    public static async Task<T> WithRetry<T>(
        this Func<Task<T>> operation,
        int maxAttempts = 3,
        TimeSpan? initialDelay = null,
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
                lastException = ex;
                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // Exponential backoff
            }
        }

        throw new InvalidOperationException(
            $"Operation failed after {maxAttempts} attempts",
            lastException);
    }
}
