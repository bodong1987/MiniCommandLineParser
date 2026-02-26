using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MiniCommandLineParser.Internals;

namespace MiniCommandLineParser;

/// <summary>
/// Contains metadata about a type's command-line option properties for parsing and formatting.
/// </summary>
public class TypeInfo
{
    /// <summary>
    /// The type being described.
    /// </summary>
#if !NETSTANDARD2_1
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
    public readonly Type Type;

    /// <summary>
    /// Internal list of reflected property information.
    /// </summary>
    private readonly List<ReflectedPropertyInfo> _propertiesCore = [];

    /// <summary>
    /// Gets all properties decorated with <see cref="OptionAttribute"/>.
    /// </summary>
    /// <value>An array of <see cref="ReflectedPropertyInfo"/> objects representing the command-line options.</value>
    public ReflectedPropertyInfo[] Properties => _propertiesCore.ToArray();

    /// <summary>
    /// Gets all positional (value) argument properties sorted by their index.
    /// </summary>
    /// <value>An array of <see cref="ReflectedPropertyInfo"/> objects representing positional arguments.</value>
    public ReflectedPropertyInfo[] PositionalProperties => 
        field ??= _propertiesCore
            .Where(x => x.Attribute.IsPositional)
            .OrderBy(x => x.Attribute.Index)
            .ToArray();

    /// <summary>
    /// Gets all named option properties (non-positional).
    /// </summary>
    /// <value>An array of <see cref="ReflectedPropertyInfo"/> objects representing named options.</value>
    public ReflectedPropertyInfo[] NamedProperties => 
        field ??= _propertiesCore
            .Where(x => !x.Attribute.IsPositional)
            .ToArray();

    /// <summary>
    /// Gets a value indicating whether the type has any properties with <see cref="OptionAttribute"/>.
    /// </summary>
    /// <value><c>true</c> if the type defines command-line options; otherwise, <c>false</c>.</value>
    public bool IsCommandLineObject => _propertiesCore.Count > 0;

    /// <summary>
    /// A default instance of the type, used for comparing values in simplify mode.
    /// </summary>
    public readonly object? DefaultObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeInfo"/> class.
    /// </summary>
    /// <param name="type">The type to analyze for command-line options.</param>
    public TypeInfo(
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        Type type)        
    {
        Type = type;

        Debug.Assert(!type.IsAbstract);

        try
        {
            DefaultObject = ObjectCreator.Create(type);
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"Failed create default for {type}, then this target will not support format command line in simplify mode.\n{ex.Message}");
        }

        ParseMeta();
    }

    /// <summary>
    /// Returns the full name of the described type.
    /// </summary>
    /// <returns>The full type name.</returns>
    public override string? ToString()
    {
        return Type.FullName;
    }

    private void ParseMeta()
    {
        var properties = Type.GetProperties();

        // sort by meta data token
        foreach(var property in properties)
        {
            if(property.IsDefined<OptionAttribute>())
            {
                var info = new ReflectedPropertyInfo(property, property.GetCustomAttribute<OptionAttribute>()!);
                _propertiesCore.Add(info);
            }
        }
    }

    /// <summary>
    /// Finds a property by its short option name.
    /// </summary>
    /// <param name="shortName">The short name to search for (e.g., "h" for "-h").</param>
    /// <param name="ignoreCase">If <c>true</c>, performs case-insensitive matching.</param>
    /// <returns>The matching <see cref="ReflectedPropertyInfo"/>, or <c>null</c> if not found.</returns>
    public ReflectedPropertyInfo? FindShortProperty(string shortName, bool ignoreCase)
    {
        return _propertiesCore.Find(x =>
        {
            if(x.Attribute.ShortName.IsNullOrEmpty())
            {
                return false;
            }

            return ignoreCase ? x.Attribute.ShortName.iEquals(shortName) : x.Attribute.ShortName!.Equals(shortName);
        });
    }

    /// <summary>
    /// Finds a property by its long option name.
    /// </summary>
    /// <param name="longName">The long name to search for (e.g., "help" for "--help").</param>
    /// <param name="ignoreCase">If <c>true</c>, performs case-insensitive matching.</param>
    /// <returns>The matching <see cref="ReflectedPropertyInfo"/>, or <c>null</c> if not found.</returns>
    public ReflectedPropertyInfo? FindLongProperty(string longName, bool ignoreCase)
    {
        return _propertiesCore.Find(x =>
        {
            if (x.Attribute.LongName.IsNullOrEmpty())
            {
                return false;
            }

            return ignoreCase ? x.Attribute.LongName.iEquals(longName) : x.Attribute.LongName!.Equals(longName);
        });
    }

    /// <summary>
    /// Finds a positional property by its index.
    /// </summary>
    /// <param name="index">The zero-based index position to search for.</param>
    /// <returns>The matching <see cref="ReflectedPropertyInfo"/>, or <c>null</c> if not found.</returns>
    public ReflectedPropertyInfo? FindPositionalProperty(int index)
    {
        return _propertiesCore.Find(x => x.Attribute.Index == index);
    }

    /// <summary>
    /// Finds the last positional property whose index is less than the given index
    /// and whose type is a collection (IsArray). This supports variadic positional arguments
    /// where a single collection-type positional property can consume multiple values.
    /// </summary>
    /// <param name="index">The current positional index that had no exact match.</param>
    /// <returns>The matching <see cref="ReflectedPropertyInfo"/>, or <c>null</c> if not found.</returns>
    public ReflectedPropertyInfo? FindLastPositionalProperty(int index)
    {
        ReflectedPropertyInfo? best = null;

        foreach (var prop in _propertiesCore)
        {
            if (prop.Attribute.IsPositional && prop.IsArray && prop.Attribute.Index < index)
            {
                if (best == null || prop.Attribute.Index > best.Attribute.Index)
                {
                    best = prop;
                }
            }
        }

        return best;
    }
}

#region ReflectedPropertyInfo
/// <summary>    
/// Caches property metadata and option attributes for efficient command-line parsing.
/// </summary>
public class ReflectedPropertyInfo
{
    /// <summary>
    /// The reflected property information from the options type.
    /// </summary>
    public readonly PropertyInfo Property;

    /// <summary>
    /// The option attribute associated with this property.
    /// </summary>
    public readonly OptionAttribute Attribute;

    /// <summary>
    /// Gets the property type.
    /// </summary>
    /// <value>The <see cref="System.Type"/> of the property.</value>
#if !NETSTANDARD2_1
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
    public Type Type { get; }

    /// <summary>
    /// Returns a string representation of the option in command-line format.
    /// </summary>
    /// <returns>A string like "-h --help" depending on the configured option names.</returns>
    public override string ToString()
    {
        return Attribute.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectedPropertyInfo"/> class.
    /// </summary>
    /// <param name="property">The property information.</param>
    /// <param name="attribute">The option attribute associated with the property.</param>
    [SuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "<Pending>")]
    public ReflectedPropertyInfo(PropertyInfo property, OptionAttribute attribute)
    {
        Property = property;
        Attribute = attribute;

        Type = Property.PropertyType;

        if(attribute.LongName.IsNullOrEmpty())
        {
            attribute.LongName = property.Name;
        }
    }

    /// <summary>
    /// Gets the property value from the target object.
    /// </summary>
    /// <param name="target">The object to get the value from.</param>
    /// <returns>The property value.</returns>
    public object? GetValue(object target)
    {
        return Property.GetValue(target);
    }

    /// <summary>
    /// Sets the property value on the target object.
    /// </summary>
    /// <param name="target">The object to set the value on.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(object? target, object? value)
    {
        Property.SetValue(target, value);
    }


    /// <summary>
    /// Gets a value indicating whether this property represents a list (array) of values.
    /// Supports List&lt;T&gt;, BindingList&lt;T&gt;, and other generic types implementing IList.
    /// </summary>
    /// <value><c>true</c> if the property is a generic collection implementing <see cref="System.Collections.IList"/>; otherwise, <c>false</c>.</value>
    public bool IsArray => Property.PropertyType.IsGenericType && 
                           typeof(System.Collections.IList).IsAssignableFrom(Property.PropertyType);

    /// <summary>
    /// Gets a value indicating whether this property represents a flags enumeration.
    /// </summary>
    /// <value><c>true</c> if the property is an enum decorated with <see cref="FlagsAttribute"/>; otherwise, <c>false</c>.</value>
    public bool IsFlags => Property.PropertyType.IsEnum && Property.PropertyType.IsDefined<FlagsAttribute>();

    /// <summary>
    /// Gets the element type for array properties.
    /// Supports List&lt;T&gt;, BindingList&lt;T&gt;, and other generic collection types.
    /// </summary>
    /// <returns>The element <see cref="System.Type"/> if this is an array property; otherwise, <c>null</c>.</returns>
    public Type? GetElementType()
    {
        if(!IsArray)
        {
            return null;
        }

        // For generic types like List<T>, BindingList<T>, etc., get the first generic argument
        return Property.PropertyType.IsGenericType ? Type.GetGenericArguments()[0] : Property.PropertyType.GetElementType();
    }
}
#endregion