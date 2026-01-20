namespace PocLoginWebsite.Core.Common;

/// <summary>
/// Guard clauses for parameter validation.
/// </summary>
public static class Guard
{
    public static class Against
    {
        /// <summary>
        /// Throws ArgumentNullException if value is null.
        /// </summary>
        public static T Null<T>(T? value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);

            return value;
        }

        /// <summary>
        /// Throws ArgumentException if string is null or empty.
        /// </summary>
        public static string NullOrEmpty(string? value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty", parameterName);

            return value;
        }

        /// <summary>
        /// Throws ArgumentException if string is null or whitespace.
        /// </summary>
        public static string NullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace", parameterName);

            return value;
        }

        /// <summary>
        /// Throws ArgumentException if collection is null or empty.
        /// </summary>
        public static IEnumerable<T> NullOrEmpty<T>(IEnumerable<T>? collection, string parameterName)
        {
            if (collection == null || !collection.Any())
                throw new ArgumentException("Collection cannot be null or empty", parameterName);

            return collection;
        }

        /// <summary>
        /// Throws InvalidOperationException with the specified message.
        /// </summary>
        public static void InvalidOperation(bool condition, string message)
        {
            if (condition)
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Throws ArgumentOutOfRangeException if value is negative.
        /// </summary>
        public static int Negative(int value, string parameterName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(parameterName, "Value cannot be negative");

            return value;
        }

        /// <summary>
        /// Throws ArgumentOutOfRangeException if value is out of range.
        /// </summary>
        public static T OutOfRange<T>(T value, T min, T max, string parameterName) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(parameterName, $"Value must be between {min} and {max}");

            return value;
        }
    }
}
