namespace PocLoginWebsite.ConsoleApp.Services;

/// <summary>
/// Console logging service for structured output.
/// </summary>
public class ConsoleLogger
{
    private const int SeparatorWidth = 60;

    public void LogHeader(string title, string subtitle = "")
    {
        Console.WriteLine("=".PadRight(SeparatorWidth, '='));
        Console.WriteLine(title);
        if (!string.IsNullOrEmpty(subtitle))
        {
            Console.WriteLine(subtitle);
        }
        Console.WriteLine("=".PadRight(SeparatorWidth, '='));
        Console.WriteLine();
    }

    public void LogStep(int step, int total, string message)
    {
        Console.WriteLine($"[{step}/{total}] {message}");
    }

    public void LogSuccess(string message, int indent = 6)
    {
        Console.WriteLine($"{new string(' ', indent)}✓ {message}");
    }

    public void LogInfo(string message, int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}{message}");
    }

    public void LogWarning(string message, int indent = 6)
    {
        Console.WriteLine($"{new string(' ', indent)}⚠ {message}");
    }

    public void LogData(string label, object value, int indent = 6)
    {
        Console.WriteLine($"{new string(' ', indent)}{label}: {value}");
    }

    public void LogList(string label, IEnumerable<string> items, int indent = 6)
    {
        Console.WriteLine($"{new string(' ', indent)}{label}:");
        foreach (var item in items)
        {
            Console.WriteLine($"{new string(' ', indent + 2)}- {item}");
        }
    }

    public void LogError(Exception ex, long elapsedMs)
    {
        Console.WriteLine();
        LogHeader("ERROR: POC Failed");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Type: {ex.GetType().Name}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner: {ex.InnerException.Message}");
        }
        Console.WriteLine($"Execution time: {elapsedMs}ms");
        Console.WriteLine();
    }

    public void LogFooter(string message)
    {
        Console.WriteLine("=".PadRight(SeparatorWidth, '='));
        Console.WriteLine(message);
        Console.WriteLine("=".PadRight(SeparatorWidth, '='));
    }

    public void NewLine() => Console.WriteLine();
}
