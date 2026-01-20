namespace PocLoginWebsite.Core.Extensions;

/// <summary>
/// Extension methods for Task operations.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Fire and forget pattern - runs task without waiting.
    /// Logs exceptions but doesn't throw them.
    /// </summary>
    public static async void FireAndForget(
        this Task task,
        Action<Exception>? onException = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            onException?.Invoke(ex);
            // In production, log this exception
            Console.WriteLine($"Fire and forget task failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts a task to a Result pattern (success/failure).
    /// </summary>
    public static async Task<(bool Success, T? Value, Exception? Error)> ToResult<T>(this Task<T> task)
    {
        try
        {
            var value = await task;
            return (true, value, null);
        }
        catch (Exception ex)
        {
            return (false, default, ex);
        }
    }

    /// <summary>
    /// Executes multiple tasks and returns all results, even if some fail.
    /// </summary>
    public static async Task<IEnumerable<(bool Success, T? Value, Exception? Error)>> WhenAllWithResults<T>(
        params Task<T>[] tasks)
    {
        var results = new List<(bool Success, T? Value, Exception? Error)>();

        foreach (var task in tasks)
        {
            results.Add(await task.ToResult());
        }

        return results;
    }
}
