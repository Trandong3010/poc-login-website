namespace PocLoginWebsite.Core.Extensions;

/// <summary>
/// Extension methods for collections and IEnumerable.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Checks if a collection is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
    {
        return collection == null || !collection.Any();
    }

    /// <summary>
    /// Executes an async action for each item in the collection.
    /// </summary>
    public static async Task ForEachAsync<T>(
        this IEnumerable<T> collection,
        Func<T, Task> action,
        CancellationToken cancellationToken = default)
    {
        foreach (var item in collection)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action(item);
        }
    }

    /// <summary>
    /// Filters out null values from the collection.
    /// </summary>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> collection) where T : class
    {
        return collection.Where(item => item != null)!;
    }

    /// <summary>
    /// Splits the collection into batches of specified size.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
    {
        if (batchSize <= 0)
            throw new ArgumentException("Batch size must be greater than 0", nameof(batchSize));

        var batch = new List<T>(batchSize);
        foreach (var item in collection)
        {
            batch.Add(item);
            if (batch.Count == batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }
}
