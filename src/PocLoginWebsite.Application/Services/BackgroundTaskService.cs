using PocLoginWebsite.Core.Extensions;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for managing background tasks and async operations.
/// </summary>
public class BackgroundTaskService
{
    /// <summary>
    /// Executes multiple tasks and returns all results, even if some fail.
    /// </summary>
    public async Task<IEnumerable<(bool Success, T? Value, Exception? Error)>>
        ExecuteAllWithResults<T>(params Task<T>[] tasks)
    {
        return await Core.Extensions.TaskExtensions.WhenAllWithResults(tasks);
    }

    /// <summary>
    /// Executes a task and returns a result pattern.
    /// </summary>
    public async Task<(bool Success, T? Value, Exception? Error)>
        ExecuteWithResult<T>(Task<T> task)
    {
        return await task.ToResult();
    }

    /// <summary>
    /// Executes multiple operations in parallel and logs results.
    /// </summary>
    public async Task<Dictionary<string, object?>> ExecuteParallelOperations(
        Dictionary<string, Func<Task<object>>> operations)
    {
        var tasks = operations.Select(async kvp =>
        {
            var (success, value, error) = await ExecuteWithResult(kvp.Value());
            return new { kvp.Key, Success = success, Value = value, Error = error };
        });

        var results = await Task.WhenAll(tasks);

        return results.ToDictionary(
            r => r.Key,
            r => r.Success ? r.Value : null
        );
    }
}
