using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

#if !NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

namespace MiniCommandLineParser.Internals;

/// <summary>
/// Provides extension methods for <see cref="Type"/> and <see cref="MemberInfo"/> operations.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Gets the first custom attribute of the specified type from a member.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="memberInfo">The member to retrieve the attribute from.</param>
    /// <param name="inherit">If <c>true</c>, searches the inheritance chain for the attribute.</param>
    /// <returns>The first matching attribute, or <c>null</c> if not found.</returns>
    public static T? GetAnyCustomAttribute<T>(this MemberInfo memberInfo, bool inherit = true)
        where T : Attribute
    {
        var attrs = memberInfo.GetCustomAttributes(typeof(T), inherit);

        return attrs.Length > 0 ? attrs[0] as T : null;
    }

    /// <summary>
    /// Gets all custom attributes of the specified type from a member.
    /// </summary>
    /// <typeparam name="T">The type of attributes to retrieve.</typeparam>
    /// <param name="memberInfo">The member to retrieve the attributes from.</param>
    /// <param name="inherit">If <c>true</c>, searches the inheritance chain for attributes.</param>
    /// <returns>An array of matching attributes, or an empty array if none found.</returns>
    public static T[] GetCustomAttributes<T>(this MemberInfo memberInfo, bool inherit = true)
        where T : Attribute
    {
        var attrs = memberInfo.GetCustomAttributes(typeof(T), inherit);

        return attrs.Length > 0 ? attrs.Cast<T>().ToArray() : [];
    }

    /// <summary>
    /// Determines whether a specified attribute is defined on a member.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="memberInfo">The member to check.</param>
    /// <param name="inherit">If <c>true</c>, searches the inheritance chain for the attribute.</param>
    /// <returns><c>true</c> if the attribute is defined; otherwise, <c>false</c>.</returns>
    public static bool IsDefined<T>(this MemberInfo memberInfo, bool inherit = true)
        where T : Attribute
    {
        return memberInfo.IsDefined(typeof(T), inherit);
    }

    /// <summary>
    /// Gets the underlying type of a member (event handler type, field type, return type, or property type).
    /// </summary>
    /// <param name="memberInfo">The member to get the underlying type from.</param>
    /// <returns>The underlying <see cref="Type"/> of the member.</returns>
    /// <exception cref="ArgumentException">Thrown when the member is not an EventInfo, FieldInfo, MethodInfo, or PropertyInfo.</exception>
    public static Type GetUnderlyingType(this MemberInfo memberInfo)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Event:
                // ReSharper disable once RedundantSuppressNullableWarningExpression
                return ((EventInfo)memberInfo).EventHandlerType!;
            case MemberTypes.Field:
                return ((FieldInfo)memberInfo).FieldType;
            case MemberTypes.Method:
                return ((MethodInfo)memberInfo).ReturnType;
            case MemberTypes.Property:
                return ((PropertyInfo)memberInfo).PropertyType;
            default:
                throw new ArgumentException
                (
                    "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                );
        }
    }

    /// <summary>
    /// Gets the value of a field or property from a target object.
    /// </summary>
    /// <param name="memberInfo">The field or property member.</param>
    /// <param name="target">The object instance to retrieve the value from.</param>
    /// <returns>The value of the field or property.</returns>
    /// <exception cref="ArgumentException">Thrown when the member is not a FieldInfo or PropertyInfo.</exception>
    public static object? GetUnderlyingValue(this MemberInfo memberInfo, object target)
    {
        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)memberInfo).GetValue(target);
            case MemberTypes.Property:
                return ((PropertyInfo)memberInfo).GetValue(target, null);
            default:
                throw new ArgumentException
                (
                    "Input MemberInfo must be if type FieldInfo, or PropertyInfo"
                );
        }
    }

    /// <summary>
    /// Sets the value of a field or property on a target object.
    /// </summary>
    /// <param name="memberInfo">The field or property member.</param>
    /// <param name="target">The object instance to set the value on.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="ArgumentException">Thrown when the member is not a FieldInfo or PropertyInfo.</exception>
    public static void SetUnderlyingValue(this MemberInfo memberInfo, object? target, object? value)
    {
        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                ((FieldInfo)memberInfo).SetValue(target, value);
                break;
            case MemberTypes.Property:
                ((PropertyInfo)memberInfo).SetValue(target, value, null);
                break;
            default:
                throw new ArgumentException
                (
                    "Input MemberInfo must be if type FieldInfo, or PropertyInfo"
                );
        }
    }

    /// <summary>
    /// Determines whether the specified member has public accessibility.
    /// </summary>
    /// <param name="info">The member to check.</param>
    /// <returns><c>true</c> if the member is public; otherwise, <c>false</c>.</returns>
    public static bool IsPublic(this MemberInfo info)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (info.MemberType == MemberTypes.Field)
        {
            return (info as FieldInfo)!.IsPublic;
        }

        if (info.MemberType == MemberTypes.Property)
        {
            var getMethod = (info as PropertyInfo)!.GetGetMethod();
            var setMethod = (info as PropertyInfo)!.GetSetMethod();
            return getMethod != null && setMethod != null && getMethod.IsPublic;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified member is static.
    /// </summary>
    /// <param name="info">The member to check.</param>
    /// <returns><c>true</c> if the member is static; otherwise, <c>false</c>.</returns>
    public static bool IsStatic(this MemberInfo info)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (info.MemberType == MemberTypes.Field)
        {
            return (info as FieldInfo)!.IsStatic;
        }

        if (info.MemberType == MemberTypes.Property)
        {
            var getMethod = (info as PropertyInfo)!.GetGetMethod();
            var setMethod = (info as PropertyInfo)!.GetSetMethod();
            return getMethod != null && setMethod != null && getMethod.IsStatic;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified member is a read-only (init-only) field.
    /// </summary>
    /// <param name="info">The member to check.</param>
    /// <returns><c>true</c> if the member is a read-only field; otherwise, <c>false</c>.</returns>
    public static bool IsConstant(this MemberInfo info)
    {
        return info is FieldInfo { IsInitOnly: true };
    }

    /// <summary>
    /// Determines whether this type implements the specified interface.
    /// </summary>
    /// <typeparam name="T">The interface type to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type implements the interface; otherwise, <c>false</c>.</returns>
    public static bool IsImplementFrom<T>(
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif      
        this Type type)
        where T : class
    {
        Debug.Assert(typeof(T).IsInterface);
        return type.GetInterfaces().Contains(typeof(T));
    }

    /// <summary>
    /// Determines whether this type implements the specified interface.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="interfaceType">The interface type to check for.</param>
    /// <returns><c>true</c> if the type implements the interface; otherwise, <c>false</c>.</returns>
    public static bool IsImplementFrom(
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        this Type type, Type interfaceType)
    {
        Debug.Assert(interfaceType.IsInterface);

        return type.GetInterfaces().Contains(interfaceType);
    }

    /// <summary>
    /// Determines whether this type is derived from or is the same as the specified base type.
    /// </summary>
    /// <typeparam name="T">The base type to check against.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is assignable to the base type; otherwise, <c>false</c>.</returns>
    public static bool IsChildOf<T>(this Type type)
        where T : class
    {
        return typeof(T).IsAssignableFrom(type);
    }

    /// <summary>
    /// Determines whether this type is derived from or is the same as the specified base type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="baseType">The base type to check against.</param>
    /// <returns><c>true</c> if the type is assignable to the base type; otherwise, <c>false</c>.</returns>
    public static bool IsChildOf(this Type type, Type baseType)
    {
        return baseType.IsAssignableFrom(type);
    }

    /// <summary>
    /// Gets the <see cref="Type"/> of the type converter specified by the attribute.
    /// </summary>
    /// <param name="attribute">The <see cref="TypeConverterAttribute"/> to extract the converter type from.</param>
    /// <returns>The converter <see cref="Type"/>, or <c>null</c> if the type cannot be resolved.</returns>
    public static Type? GetConverterType(this TypeConverterAttribute attribute)
    {
        TryGetTypeByName(attribute.ConverterTypeName, out var result, AppDomain.CurrentDomain.GetAssemblies());
        return result;
    }

    /// <summary>
    /// Attempts to resolve a type by its name, including support for generic types.
    /// </summary>
    /// <param name="typeName">The full or assembly-qualified name of the type.</param>
    /// <param name="type">When this method returns, contains the resolved type if found; otherwise, <c>null</c>.</param>
    /// <param name="customAssemblies">Additional assemblies to search in.</param>
    /// <returns><c>true</c> if the type was found; otherwise, <c>false</c>.</returns>
#if !NETSTANDARD2_1
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL3050:RequiresDynamicCode")]        
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2057:Unrecognized value")]
#endif
    private static bool TryGetTypeByName(string typeName, out Type? type, params Assembly[] customAssemblies)
    {
        if (typeName.Contains("Version=")
            && !typeName.Contains('`'))
        {
            // remove full qualified assembly type name
            typeName = typeName[..typeName.IndexOf(',')];
        }

        type = Type.GetType(typeName) ?? GetTypeFromAssemblies(typeName, customAssemblies);

        // try get generic types
        if (type == null
            && typeName.Contains('`'))
        {
            var match = Regex.Match(typeName, @"(?<MainType>.+`(?<ParamCount>[0-9]+))\[(?<Types>.*)\]");

            if (match.Success)
            {
                var genericParameterCount = int.Parse(match.Groups["ParamCount"].Value);
                var genericDef = match.Groups["Types"].Value;
                var typeArgs = new List<string>(genericParameterCount);
                foreach (Match typeArgMatch in Regex.Matches(genericDef, @"\[(?<Type>.*?)\],?"))
                {
                    if (typeArgMatch.Success)
                    {
                        typeArgs.Add(typeArgMatch.Groups["Type"].Value.Trim());
                    }
                }

                var genericArgumentTypes = new Type?[typeArgs.Count];
                for (var genTypeIndex = 0; genTypeIndex < typeArgs.Count; genTypeIndex++)
                {
                    if (TryGetTypeByName(typeArgs[genTypeIndex], out var genericType, customAssemblies))
                    {
                        genericArgumentTypes[genTypeIndex] = genericType;
                    }
                    else
                    {
                        // cant find generic type
                        return false;
                    }
                }

                var genericTypeString = match.Groups["MainType"].Value;
                if (TryGetTypeByName(genericTypeString, out var genericMainType))
                {
                    // make generic type                        
                    // ReSharper disable once RedundantSuppressNullableWarningExpression
                    type = genericMainType?.MakeGenericType(genericArgumentTypes!);
                }
            }
        }

        return type != null;
    }

    /// <summary>
    /// Searches for a type by name in the specified assemblies and all loaded assemblies.
    /// </summary>
    /// <param name="typeName">The name of the type to find.</param>
    /// <param name="customAssemblies">Additional assemblies to search in first.</param>
    /// <returns>The resolved <see cref="Type"/>, or <c>null</c> if not found.</returns>
#if !NETSTANDARD2_1
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2057:Unrecognized value")]
#endif
    private static Type? GetTypeFromAssemblies(string typeName, params Assembly[] customAssemblies)
    {
        Type? type = null;

        if (customAssemblies.Length > 0)
        {
            foreach (var assembly in customAssemblies)
            {
                type = assembly.GetType(typeName);

                if (type != null)
                    return type;
            }
        }

        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in loadedAssemblies)
        {
            type = assembly.GetType(typeName);

            if (type != null)
                return type;
        }

        return type;
    }

    /// <summary>
    /// Gets a type by its name from the current AppDomain's loaded assemblies.
    /// </summary>
    /// <param name="typeName">The name of the type to find.</param>
    /// <returns>The resolved <see cref="Type"/>, or <c>null</c> if not found.</returns>
#if !NETSTANDARD2_1
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2057:Unrecognized value")]
#endif
    public static Type? GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null)
        {
            return type;
        }
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        return null;
    }

    /// <summary>
    /// Determines whether the specified type is a numeric type (integer, floating-point, or decimal).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is a numeric type; otherwise, <c>false</c>.</returns>
    public static bool IsNumericType(this Type type)
    {
        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Enumerates all individual flags that are set in a flags enumeration value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="flags">The flags enumeration value.</param>
    /// <returns>An enumerable of individual flag values that are set.</returns>
#if !NETSTANDARD2_1
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL3050:RequiresDynamicCode")]
#endif
    public static IEnumerable<T> GetUniqueFlags<T>(this T flags)
        where T : Enum
    {
        // aot warning
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (Enum value in Enum.GetValues(flags.GetType()))
        {
            if (flags.HasFlag(value))
            {
                yield return (T)value;
            }
        }
    }

    /// <summary>
    /// Gets the display name of a property from its <see cref="DisplayNameAttribute"/>, or the property name if not specified.
    /// </summary>
    /// <param name="propertyInfo">The property to get the display name from.</param>
    /// <returns>The display name or the property name.</returns>
    public static string GetDisplayName(this PropertyInfo propertyInfo)
    {
        var attr = propertyInfo.GetAnyCustomAttribute<DisplayNameAttribute>();

        return attr?.DisplayName ?? propertyInfo.Name;
    }

    /// <summary>
    /// Determines whether the property is marked as read-only via <see cref="ReadOnlyAttribute"/>.
    /// </summary>
    /// <param name="propertyInfo">The property to check.</param>
    /// <returns><c>true</c> if the property is marked as read-only; otherwise, <c>false</c>.</returns>
    public static bool IsReadOnly(this PropertyInfo propertyInfo)
    {
        var attr = propertyInfo.GetAnyCustomAttribute<ReadOnlyAttribute>();

        return attr is { IsReadOnly: true };
    }

    /// <summary>
    /// Determines whether the property is browsable (visible in a properties window).
    /// </summary>
    /// <param name="propertyInfo">The property to check.</param>
    /// <returns><c>true</c> if the property is browsable or has no <see cref="BrowsableAttribute"/>; otherwise, <c>false</c>.</returns>
    public static bool IsBrowsable(this PropertyInfo propertyInfo)
    {
        var attr = propertyInfo.GetAnyCustomAttribute<BrowsableAttribute>();

        return attr == null || attr.Browsable;
    }

    #region For PropertyDescriptor
    /// <summary>
    /// Determines whether a specified attribute is defined on a property descriptor.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="propertyDescriptor">The property descriptor to check.</param>
    /// <returns><c>true</c> if the attribute is defined; otherwise, <c>false</c>.</returns>
    public static bool IsDefined<T>(this PropertyDescriptor propertyDescriptor) where T : Attribute
    {
        return propertyDescriptor.Attributes.OfType<T>().Any();
    }

    /// <summary>
    /// Gets the first custom attribute of the specified type from a property descriptor.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="propertyDescriptor">The property descriptor to retrieve the attribute from.</param>
    /// <returns>The first matching attribute, or <c>null</c> if not found.</returns>
    public static T? GetCustomAttribute<T>(this PropertyDescriptor propertyDescriptor) where T : Attribute
    {
        foreach (var attr in propertyDescriptor.Attributes)
        {
            if (attr is T t)
            {
                return t;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets all custom attributes of the specified type from a property descriptor.
    /// </summary>
    /// <typeparam name="T">The type of attributes to retrieve.</typeparam>
    /// <param name="propertyDescriptor">The property descriptor to retrieve the attributes from.</param>
    /// <returns>An array of matching attributes.</returns>
    public static T[] GetCustomAttributes<T>(this PropertyDescriptor propertyDescriptor) where T : Attribute
    {
        var list = new List<T>();
        foreach (var attr in propertyDescriptor.Attributes)
        {
            if (attr is T t)
            {
                list.Add(t);
            }
        }

        return list.ToArray();
    }                
    #endregion

    /// <summary>
    /// Gets the return type of a method, or the declaring type if it is a constructor.
    /// </summary>
    /// <param name="mi">The method or constructor to get the return type from.</param>
    /// <returns>The return type of the method, or the declaring type for constructors.</returns>
    public static Type? GetReturnType(this MethodBase mi)
    {
        return mi.IsConstructor ? mi.DeclaringType : ((MethodInfo)mi).ReturnType;
    }
}