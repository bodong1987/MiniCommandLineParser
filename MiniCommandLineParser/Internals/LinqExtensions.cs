using System.Collections;

namespace MiniCommandLineParser.Internals;

/// <summary>
/// Provides LINQ extension methods.
/// </summary>
internal static class LinqExtensions
{
    /// <summary>
    /// Searches for an element that matches the specified predicate and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence to search.</param>
    /// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first matching element if found; otherwise, -1.</returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
    {
        var i = 0;

        foreach (var element in source)
        {
            if (predicate(element))
                return i;

            ++i;
        }

        return -1;
    }

    /// <summary>
    /// Searches for an element that matches the specified predicate starting at the specified index and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first matching element if found; otherwise, -1.</returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, int startIndex, Predicate<TSource> predicate)
    {
        var i = 0;
        var c = 0;

        foreach (var element in source)
        {
            if (c < startIndex)
            {
                ++c;
                ++i;
                continue;
            }

            if (predicate(element))
                return i;

            ++i;
        }

        return -1;
    }

    /// <summary>
    /// Returns the element with the maximum projected value in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="list">The sequence to get the maximum element from.</param>
    /// <param name="projection">A converter that transforms each element into a comparable integer value.</param>
    /// <returns>The element with the maximum projected value; or the default value of the type if the sequence is empty.</returns>
    public static T? MaxElement<T>(this IEnumerable<T> list, Converter<T, int> projection)
    {
        var maxValue = int.MinValue;
        var result = default(T);

        foreach (var item in list)
        {
            var value = projection(item);
            if (value > maxValue)
            {
                maxValue = value;
                result = item;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether the sequence contains an element that matches the specified condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="list">The source sequence to search.</param>
    /// <param name="match">The predicate that defines the conditions of the element to search for.</param>
    /// <returns><c>true</c> if the sequence contains an element that matches the condition; otherwise, <c>false</c>.</returns>
    public static bool Contains<T>(this IEnumerable<T> list, Predicate<T> match)
    {
        return list.Any(v => match(v));
    }

    /// <summary>
    /// Removes the first element that matches the specified condition from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to remove the element from.</param>
    /// <param name="match">The predicate that defines the conditions of the element to remove.</param>
    /// <returns><c>true</c> if an element was successfully removed; <c>false</c> if no matching element was found.</returns>
    public static bool Remove<T>(this IList<T> list, Predicate<T> match)
    {
        var index = list.IndexOf(match);

        if (index == -1)
        {
            return false;
        }
            
        list.RemoveAt(index);
            
        return true;

    }

    /// <summary>
    /// Projects each element of a non-generic <see cref="IEnumerable"/> sequence into a new form.
    /// </summary>
    /// <typeparam name="TRet">The type of the value returned by the selector.</typeparam>
    /// <param name="enumerable">The non-generic sequence to transform.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>An <see cref="IEnumerable{TRet}"/> whose elements are the result of invoking the transform function on each element of the source sequence.</returns>
    public static IEnumerable<TRet> Select<TRet>(this IEnumerable enumerable, Func<object, TRet> selector)
    {
        // Note: Do NOT use LINQ query syntax (from...select) here!
        // It would cause infinite recursion because the query syntax compiles to a Select() call,
        // which may resolve back to this extension method instead of the standard Enumerable.Select().
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in enumerable)
        {
            yield return selector(item!);
        }
    }
}