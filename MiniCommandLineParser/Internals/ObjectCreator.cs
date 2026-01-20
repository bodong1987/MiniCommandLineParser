#if !NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

namespace MiniCommandLineParser.Internals;

/// <summary>
/// Provides utility methods for creating object instances.
/// </summary>
internal static class ObjectCreator
{
    /// <summary>
    /// Creates an instance of the specified type using the provided constructor arguments.
    /// </summary>
    /// <typeparam name="T">The type of instance to create.</typeparam>
    /// <param name="args">The constructor arguments.</param>
    /// <returns>A new instance of type <typeparamref name="T"/>, or <c>null</c> if the type is abstract.</returns>
    public static T? Create<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
        T>(params object[] args)
    {
        return (T?)Create(typeof(T), args);
    }

    /// <summary>
    /// Creates an instance of the specified type using the provided constructor arguments.
    /// </summary>
    /// <param name="type">The type of instance to create.</param>
    /// <param name="args">The constructor arguments.</param>
    /// <returns>A new instance of the specified type; an empty string if the type is <see cref="string"/>; or <c>null</c> if the type is abstract.</returns>
    public static object? Create(
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
        Type type, params object[] args)
    {
        if (type == typeof(string))
        {
            return "";
        }

        return type.IsAbstract ? null : Activator.CreateInstance(type, args);
    }
}