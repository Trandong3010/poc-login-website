namespace PocLoginWebsite.Core.Models;

/// <summary>
/// Represents the result of a test execution.
/// </summary>
public class TestResult
{
    /// <summary>
    /// Gets or sets whether the test passed.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the test execution message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any exception that occurred during test execution.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets or sets the test execution duration.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Creates a successful test result.
    /// </summary>
    public static TestResult Passed(string message = "Test passed", TimeSpan? duration = null)
    {
        return new TestResult
        {
            Success = true,
            Message = message,
            Duration = duration ?? TimeSpan.Zero,
        };
    }

    /// <summary>
    /// Creates a failed test result.
    /// </summary>
    public static TestResult Failed(
        string message,
        Exception? exception = null,
        TimeSpan? duration = null
    )
    {
        return new TestResult
        {
            Success = false,
            Message = message,
            Exception = exception,
            Duration = duration ?? TimeSpan.Zero,
        };
    }
}
