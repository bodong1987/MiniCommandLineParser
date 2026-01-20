using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MiniCommandLineParser.Internals;

/// <summary>
/// Provides extension methods for string operations.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Gets the leftmost <paramref name="length"/> characters from a string.
    /// </summary>
    /// <param name="value">The string to retrieve the substring from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>The leftmost <paramref name="length"/> characters, or the entire string if it is shorter than <paramref name="length"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>
    public static string Left(this string value, int length)
    {
#pragma warning disable CA1510
        if (value == null)
#pragma warning restore CA1510
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length is less than zero");
        }

        return length <= value.Length ? value[..length] : value;
    }

    /// <summary>
    /// Gets the rightmost <paramref name="length"/> characters from a string.
    /// </summary>
    /// <param name="value">The string to retrieve the substring from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>The rightmost <paramref name="length"/> characters, or the entire string if it is shorter than <paramref name="length"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>
    public static string Right(/*[NotNull]*/ this string value, int length)
    {
#pragma warning disable CA1510
        if (value == null)
#pragma warning restore CA1510
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length is less than zero");
        }

        return length < value.Length ? value[^length..] : value;
    }


    /// <summary>
    /// Determines whether the specified string is not <c>null</c> or empty.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns><c>true</c> if the string is not <c>null</c> or empty; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether the specified string is <c>null</c> or empty.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns><c>true</c> if the string is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether two strings are equal, ignoring case.
    /// </summary>
    /// <param name="value">The first string to compare.</param>
    /// <param name="other">The second string to compare.</param>
    /// <returns><c>true</c> if the strings are equal ignoring case; otherwise, <c>false</c>.</returns>
    // ReSharper disable once InconsistentNaming
    public static bool iEquals(this string? value, string? other)
    {
        if (value == null && other == null)
        {
            return true;
        }

        if (value == null || other == null)
        {
            return false;
        }

        return value.Equals(other, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Determines whether the beginning of this string matches the specified string, ignoring case.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="other">The string to compare to the beginning of this string.</param>
    /// <returns><c>true</c> if this string starts with the specified string ignoring case; otherwise, <c>false</c>.</returns>
    // ReSharper disable once InconsistentNaming
    public static bool iStartsWith(this string? value, string? other)
    {
        if (value == null && other == null)
        {
            return true;
        }

        if (value == null || other == null)
        {
            return false;
        }

        return value.StartsWith(other, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Determines whether the end of this string matches the specified string, ignoring case.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="other">The string to compare to the end of this string.</param>
    /// <returns><c>true</c> if this string ends with the specified string ignoring case; otherwise, <c>false</c>.</returns>
    // ReSharper disable once InconsistentNaming
    public static bool iEndsWith(this string? value, string? other)
    {
        if (value == null && other == null)
        {
            return true;
        }

        if (value == null || other == null)
        {
            return false;
        }

        return value.EndsWith(other, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Replaces all occurrences of a specified string in the input string, using case-insensitive comparison.
    /// </summary>
    /// <param name="input">The string to search within.</param>
    /// <param name="search">The string to search for.</param>
    /// <param name="value">The replacement string.</param>
    /// <returns>A new string with all occurrences of <paramref name="search"/> replaced by <paramref name="value"/>.</returns>
    public static string ReplaceCaseInsensitive(this string input, string search, string value)
    {
        if (input.IsNotNullOrEmpty())
        {
            var result = System.Text.RegularExpressions.Regex.Replace(
                input,
                System.Text.RegularExpressions.Regex.Escape(search),
                value.Replace("$", "$$"),
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return result;
        }

        return input;
    }

    /// <summary>
    /// Computes a deterministic 32-bit hash code for the string.
    /// </summary>
    /// <param name="s">The string to compute the hash code for.</param>
    /// <param name="ignoreCase">If set to <c>true</c>, the hash is computed in a case-insensitive manner.</param>
    /// <returns>A deterministic 32-bit unsigned integer hash code.</returns>
    public static uint GetDeterministicHashCode(this string s, bool ignoreCase = false)
    {
        uint h = 0;
        var len = s.Length;
        if (len > 0)
        {
            var off = 0;

            for (var i = 0; i < len; i++)
            {
                var c = s[off++];
                if (ignoreCase && c is >= 'A' and <= 'Z')
                {
                    c += (char)('a' - 'A');
                }
                h = 31 * h + c;
            }
        }
        return h;
    }

    /// <summary>
    /// Computes a deterministic 64-bit hash code for the string.
    /// </summary>
    /// <param name="s">The string to compute the hash code for.</param>
    /// <param name="ignoreCase">If set to <c>true</c>, the hash is computed in a case-insensitive manner.</param>
    /// <returns>A deterministic 64-bit unsigned integer hash code.</returns>
    public static ulong GetDeterministicHashCode64(this string s, bool ignoreCase = false)
    {
        ulong h = 0;
        var len = s.Length;
        if (len > 0)
        {
            var off = 0;

            for (var i = 0; i < len; i++)
            {
                var c = s[off++];
                if (ignoreCase && c is >= 'A' and <= 'Z')
                {
                    c += (char)('a' - 'A');
                }
                h = 31 * h + c;
            }
        }
        return h;
    }

    /// <summary>
    /// Searches for a character that matches the specified predicate and returns the zero-based index of the last occurrence.
    /// </summary>
    /// <param name="s">The string to search.</param>
    /// <param name="predicate">The predicate that defines the conditions of the character to search for.</param>
    /// <returns>The zero-based index of the last matching character if found; otherwise, -1.</returns>
    public static int LastIndexOf(this string s, Predicate<char> predicate)
    {
        if (s.IsNullOrEmpty())
        {
            return -1;
        }

        for (var i = s.Length - 1; i >= 0; --i)
        {
            var c = s[i];
            if (predicate(c))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Searches for a character that matches the specified predicate and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <param name="s">The string to search.</param>
    /// <param name="predicate">The predicate that defines the conditions of the character to search for.</param>
    /// <returns>The zero-based index of the first matching character if found; otherwise, -1.</returns>
    public static int IndexOf(this string s, Predicate<char> predicate)
    {
        // ReSharper disable once InvokeAsExtensionMember
        return IndexOf(s, 0, predicate);
    }

    /// <summary>
    /// Searches for a character that matches the specified predicate starting at the specified index and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <param name="s">The string to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="predicate">The predicate that defines the conditions of the character to search for.</param>
    /// <returns>The zero-based index of the first matching character if found; otherwise, -1.</returns>
    public static int IndexOf(this string s, int startIndex, Predicate<char> predicate)
    {
        if (s.IsNullOrEmpty())
        {
            return -1;
        }

        for (var i = startIndex; i < s.Length; ++i)
        {
            var c = s[i];
            if (predicate(c))
            {
                return i;
            }
        }

        return -1;
    }
}