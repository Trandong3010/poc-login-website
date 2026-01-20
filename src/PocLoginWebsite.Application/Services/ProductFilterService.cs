using PocLoginWebsite.Core.Extensions;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for filtering and searching products.
/// </summary>
public class ProductFilterService
{
    /// <summary>
    /// Searches products by name (case-insensitive).
    /// </summary>
    public IEnumerable<string> SearchProducts(IEnumerable<string> products, string searchTerm)
    {
        return products.Where(p => p.ContainsIgnoreCase(searchTerm));
    }

    /// <summary>
    /// Filters products that match exactly (case-insensitive).
    /// </summary>
    public IEnumerable<string> FilterExact(IEnumerable<string> products, string filter)
    {
        return products.Where(p => p.EqualsIgnoreCase(filter));
    }

    /// <summary>
    /// Gets unique product names (removes duplicates).
    /// </summary>
    public IEnumerable<string> GetUniqueProducts(IEnumerable<string> products)
    {
        return products.Distinct();
    }

    /// <summary>
    /// Removes null products from the list.
    /// </summary>
    public IEnumerable<string> RemoveNullProducts(IEnumerable<string?> products)
    {
        return products.WhereNotNull();
    }
}
