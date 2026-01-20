using PocLoginWebsite.Core.Extensions;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for exporting and formatting data.
/// </summary>
public class DataExportService
{
    /// <summary>
    /// Exports data to CSV file asynchronously.
    /// </summary>
    public async Task ExportToCsvAsync(IEnumerable<string> data, string filePath)
    {
        // Clear file if exists
        if (File.Exists(filePath))
            File.Delete(filePath);

        await data.ForEachAsync(async item =>
        {
            var formatted = item.ToTitleCase();
            await File.AppendAllTextAsync(filePath, formatted + Environment.NewLine);
        });
    }

    /// <summary>
    /// Formats product name with title case and truncation.
    /// </summary>
    public string FormatProductName(string name, int maxLength = 30)
    {
        return name.ToTitleCase().Truncate(maxLength);
    }

    /// <summary>
    /// Formats a list of products for display.
    /// </summary>
    public IEnumerable<string> FormatProductList(IEnumerable<string> products, int maxLength = 40)
    {
        return products.Select(p => p.ToTitleCase().Truncate(maxLength));
    }
}
