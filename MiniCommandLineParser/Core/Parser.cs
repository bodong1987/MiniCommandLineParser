using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using MiniCommandLineParser.Internals;
using System.Diagnostics;


#if !NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

namespace MiniCommandLineParser;

/// <summary>
/// Specifies how the command line should be formatted when converting an options object back to arguments.
/// </summary>
[Flags]
public enum CommandLineFormatMethod
{
    /// <summary>
    /// No formatting is applied.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Outputs all options regardless of whether their values match the defaults.
    /// </summary>
    Complete = 1 << 0,

    /// <summary>
    /// Outputs only options whose values differ from their default values.
    /// </summary>
    Simplify = 1 << 1,
    
    /// <summary>
    /// Outputs options using the equal sign style.
    /// </summary>
    EqualSignStyle = 1 << 2
}

/// <summary>
/// Provides methods for parsing command-line arguments into strongly-typed options objects
/// and formatting options objects back into command-line strings.
/// </summary>
public class Parser
{
    /// <summary>
    /// The configuration settings for this parser instance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public readonly ParserSettings Settings;

    private static readonly ConcurrentDictionary<Type, TypeInfo> TypeInfos = new();

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Parser"/> class.
    /// </summary>
    public Parser()
    {
        Settings = new ParserSettings();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parser"/> class with the specified settings.
    /// </summary>
    /// <param name="settings">The parser configuration settings.</param>
    public Parser(ParserSettings settings)
    {
        Settings = settings;
    }
    #endregion

    #region Parse Target
    /// <summary>
    /// Gets or creates cached type information for the specified type.
    /// </summary>
    /// <param name="type">The type to get information for.</param>
    /// <returns>The <see cref="TypeInfo"/> containing metadata about the type's command-line options.</returns>
    public static TypeInfo GetTypeInfo(
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        Type type)
    {
        return TypeInfos.GetOrAdd(type, static t => new TypeInfo(t));
    }

    /// <summary>
    /// Gets or creates cached type information for the specified type.
    /// </summary>
    /// <typeparam name="T">The type to get information for.</typeparam>
    /// <returns>The <see cref="TypeInfo"/> containing metadata about the type's command-line options.</returns>
    public static TypeInfo GetTypeInfo<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        T>()
    {
        return GetTypeInfo(typeof(T));
    }

    /// <summary>
    /// Gets or creates cached type information for the specified object's type.
    /// </summary>
    /// <param name="target">The object to get type information for.</param>
    /// <returns>The <see cref="TypeInfo"/> containing metadata about the object's type.</returns>
    // ReSharper disable once MemberCanBePrivate.Global
    public static TypeInfo GetTypeInfo(object target)
    {
        return GetTypeInfo(target.GetType());
    }

    /// <summary>
    /// Parses a command-line string into a new instance of the options type.
    /// </summary>
    /// <typeparam name="T">The options type to parse into.</typeparam>
    /// <param name="arguments">The command-line string to parse.</param>
    /// <returns>A <see cref="ParserResult{T}"/> containing the parsed options or error information.</returns>
    public ParserResult<T> Parse<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        T>(string arguments) where T : class, new ()
    {
        return Parse<T>(Splitter.SplitCommandLineIntoArguments(arguments));
    }

    /// <summary>
    /// Parses a collection of command-line arguments into a new instance of the options type.
    /// </summary>
    /// <typeparam name="T">The options type to parse into.</typeparam>
    /// <param name="arguments">The collection of arguments to parse.</param>
    /// <returns>A <see cref="ParserResult{T}"/> containing the parsed options or error information.</returns>
    public ParserResult<T> Parse<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        T>(IEnumerable<string> arguments) where T : class, new ()
    {
        return Parse(arguments, new T());
    }

    /// <summary>
    /// Parses a command-line string into an existing options instance.
    /// </summary>
    /// <typeparam name="T">The options type to parse into.</typeparam>
    /// <param name="arguments">The command-line string to parse.</param>
    /// <param name="value">The existing options instance to populate.</param>
    /// <returns>A <see cref="ParserResult{T}"/> containing the parsed options or error information.</returns>
    public ParserResult<T> Parse<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        T>(string arguments, T value) where T : class
    {
        return Parse(Splitter.SplitCommandLineIntoArguments(arguments), value);
    }

    /// <summary>
    /// Parses a collection of command-line arguments into an existing options instance.
    /// </summary>
    /// <typeparam name="T">The options type to parse into.</typeparam>
    /// <param name="arguments">The collection of arguments to parse.</param>
    /// <param name="value">The existing options instance to populate.</param>
    /// <returns>A <see cref="ParserResult{T}"/> containing the parsed options or error information.</returns>
    public ParserResult<T> Parse<
#if !NETSTANDARD2_1
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        T>(IEnumerable<string> arguments, T value) where T : class
    {
        var result = new ParserResult<T>(GetTypeInfo(value))
        {
            Result = ParserResultType.NotParsed,
            Value = value
        };

        IParserResult r = result;
        Parse(arguments, value, result.Type, ref r);

        return result;
    }

    #region Prase Details
    private void Parse(IEnumerable<string> arguments, object value, TypeInfo? typeInfo, ref IParserResult result)
    {
        var startPos = 0;
        var positionalIndex = 0;
        var setProperties = new HashSet<string>(); // Track which properties were set

        var enumerable = arguments.ToList();
        while(startPos < enumerable.Count)
        {
            var arg = enumerable[startPos];

            // Check if this is a named option (starts with - or --)
            if (arg.StartsWith('-'))
            {
                // Parse named option
                if (!ParseNamedOption(enumerable, ref startPos, value, typeInfo, ref result, setProperties))
                {
                    result.Result = ParserResultType.NotParsed;
                    return;
                }
            }
            else
            {
                // Parse positional argument
                if (!ProcessPositionalValue(value, typeInfo, positionalIndex, arg, ref result, setProperties))
                {
                    result.Result = ParserResultType.NotParsed;
                    return;
                }
                positionalIndex++;
                startPos++;
            }
        }

        // Apply environment variable values for unset properties
        ApplyEnvironmentVariables(value, typeInfo, setProperties);

        // Validate required options before marking as parsed
        if (!ValidateRequiredOptions(value, typeInfo, setProperties, ref result))
        {
            result.Result = ParserResultType.NotParsed;
            return;
        }

        result.Result = ParserResultType.Parsed;
    }

    /// <summary>
    /// Applies values from environment variables for properties that were not set from command line.
    /// </summary>
    /// <param name="value">The options object to populate.</param>
    /// <param name="typeInfo">The type information for the options class.</param>
    /// <param name="setProperties">Set of property names that were already set from command line.</param>
    private void ApplyEnvironmentVariables(object value, TypeInfo? typeInfo, HashSet<string> setProperties)
    {
        if (typeInfo == null)
        {
            return;
        }

        foreach (var property in typeInfo.Properties)
        {
            // Skip if already set from command line or no environment variable configured
            if (setProperties.Contains(property.Property.Name) || 
                string.IsNullOrEmpty(property.Attribute.EnvironmentVariable))
            {
                continue;
            }

            var envValue = Environment.GetEnvironmentVariable(property.Attribute.EnvironmentVariable);
            if (string.IsNullOrEmpty(envValue))
            {
                continue;
            }

            try
            {
                // Handle array types
                if (property.IsArray)
                {
                    if (Activator.CreateInstance(property.Type) is IList list)
                    {
                        // Split by comma for environment variable arrays
                        var parts = envValue.Split(',');
                        foreach (var part in parts)
                        {
                            var trimmed = part.Trim();
                            if (trimmed.Length > 0)
                            {
                                var elementValue = GetValue(property.GetElementType()!, trimmed);
                                if (elementValue != null)
                                {
                                    list.Add(elementValue);
                                }
                            }
                        }
                        property.SetValue(value, list);
                        setProperties.Add(property.Property.Name);
                    }
                }
                else
                {
                    var convertedValue = GetValue(property.Type, envValue);
                    if (convertedValue != null)
                    {
                        property.SetValue(value, convertedValue);
                        setProperties.Add(property.Property.Name);
                    }
                }
            }
            catch
            {
                // Ignore conversion errors from environment variables
            }
        }
    }

    /// <summary>
    /// Validates that all required options have been provided with values.
    /// </summary>
    /// <param name="value">The parsed options object.</param>
    /// <param name="typeInfo">The type information for the options class.</param>
    /// <param name="setProperties">Set of property names that were explicitly set during parsing.</param>
    /// <param name="result">The parser result to append errors to.</param>
    /// <returns>True if all required options are provided; otherwise, false.</returns>
    // ReSharper disable once UnusedParameter.Local
    private static bool ValidateRequiredOptions(object value, TypeInfo? typeInfo, HashSet<string> setProperties, ref IParserResult result)
    {
        if (typeInfo == null)
        {
            return true;
        }

        var isValid = true;

        foreach (var property in typeInfo.Properties.Where(p => p.Attribute.Required))
        {
            // Check if this property was explicitly set during parsing
            var wasSet = setProperties.Contains(property.Property.Name);
            
            if (!wasSet)
            {
                var optionName = property.Attribute.LongName.IsNotNullOrEmpty() 
                    ? $"--{property.Attribute.LongName}" 
                    : (property.Attribute.ShortName.IsNotNullOrEmpty() 
                        ? $"-{property.Attribute.ShortName}" 
                        : $"<{property.Attribute.MetaName}>");
                
                result.AppendError(optionName, ParseErrorType.MissingRequired, $"Required option '{optionName}' is missing.");
                isValid = false;
            }
        }


        return isValid;
    }

    private bool ParseNamedOption(List<string> enumerable, ref int startPos, object value, TypeInfo? typeInfo, ref IParserResult result, HashSet<string> setProperties)
    {
        if (!GetRange(enumerable, startPos, out var key, out var values, out var nextPos))
        {
            return true;
        }

        var i1 = key!.IndexOf('=');
        var i2 = key.IndexOf('"');

        if(i1 != -1)
        {
            // contain = 
            if(i2 == -1 || i1 < i2)
            {
                var key2 = key[..i1].Trim();
                var value2 = key[(i1 + 1)..].Trim();
                key = key2;
                values!.Insert(0, value2);
            }                    
        }

        // Find the property for this option to determine how many values it needs
        var property = FindPropertyByKey(typeInfo, key);
        
        // If property is found, limit values based on property type
        if (property != null && values != null)
        {
            // Boolean options handling
            if (property.Type == typeof(bool))
            {
                if (i1 != -1)
                {
                    // Using equals syntax (e.g., --verbose=true), value already added
                    // Don't consume any additional values, keep only the first one
                    if (values.Count > 1)
                    {
                        nextPos = startPos + 1; // Only consume the key (which contains =value)
                        var firstValue = values[0];
                        values.Clear();
                        values.Add(firstValue);
                    }
                }
                else if (values.Count > 0)
                {
                    // Space syntax - check if first value is a boolean literal
                    var firstValue = values[0].Trim().ToLowerInvariant();
                    if (firstValue is "true" or "false")
                    {
                        // Consume only the boolean value
                        nextPos = startPos + 2; // key + one boolean value
                        var boolValue = values[0];
                        values.Clear();
                        values.Add(boolValue);
                    }
                    else
                    {
                        // Not a boolean literal, don't consume any values
                        nextPos = startPos + 1;
                        values.Clear();
                    }
                }
            }
            // Non-array, non-flags options take only one value
            else if (property is { IsArray: false, IsFlags: false })
            {
                if (values.Count > 1)
                {
                    // Adjust nextPos to only consume the first value
                    nextPos = startPos + 2; // key + one value
                    var firstValue = values[0];
                    values.Clear();
                    values.Add(firstValue);
                }
            }
        }

        if(!ProcessValue(value, typeInfo, key, values!, ref result, setProperties))
        {
            return false;
        }

        startPos = nextPos;
        return true;
    }

    private ReflectedPropertyInfo? FindPropertyByKey(TypeInfo? typeInfo, string key)
    {
        if (typeInfo == null || key.IsNullOrEmpty())
        {
            return null;
        }

        // Remove leading dashes
        var name = key.TrimStart('-');
        
        // Try short name first (single char after single dash)
        if (key.StartsWith('-') && !key.StartsWith("--") && name.Length == 1)
        {
            return typeInfo.FindShortProperty(name, !Settings.CaseSensitive);
        }
        
        // Try long name
        return typeInfo.FindLongProperty(name, !Settings.CaseSensitive);
    }

    private static bool GetRange(List<string> args, int startPos, out string? name, out List<string>? values, out int pos)
    {
        name = null;
        values = null;
        pos = startPos;

        var keyPos = -1;
        for(var i=startPos; i<args.Count; ++i)
        {
            var value = args[i];

            if(value.StartsWith('-') || value.StartsWith("--"))
            {
                if(keyPos == -1)
                {
                    keyPos = i;
                    name = value;
                }
                else
                {
                    pos = i;
                    // Collect values between current key and next key, excluding positional args
                    values = [];
                    for (var j = startPos + 1; j < i; j++)
                    {
                        // Stop collecting when we hit a non-option argument that could be positional
                        // But we include all arguments until the next option
                        values.Add(args[j]);
                    }
                    return true;
                }
            }
        }

        if(keyPos != -1)
        {
            values = args.GetRange(startPos + 1, args.Count - startPos - 1);
            pos = args.Count;
        }

        return name.IsNotNullOrEmpty() && values != null;
    }

    private bool ProcessPositionalValue(object value, TypeInfo? typeInfo, int index, string argValue, ref IParserResult result, HashSet<string> setProperties)
    {
        if (typeInfo == null)
        {
            return false;
        }

        var info = typeInfo.FindPositionalProperty(index);

        if (info == null)
        {
            if (!Settings.IgnoreUnknownArguments)
            {
                result.AppendError($"arg{index}", ParseErrorType.UnknownOption, $"Unknown positional argument at index {index}: {argValue}");
                return false;
            }

            return true;
        }

        var loadedValue = GetValue(info.Type, argValue);

        if (loadedValue == null)
        {
            var optionName = info.Attribute.MetaName.IsNotNullOrEmpty() ? info.Attribute.MetaName : $"arg{index}";
            result.AppendError(optionName, ParseErrorType.InvalidValue, $"Failed convert value for positional argument at index {index}: {argValue}");
            return false;
        }

        info.SetValue(value, loadedValue);
        setProperties.Add(info.Property.Name); // Track that this property was set

        return true;
    }

    private bool ProcessValue(object value, TypeInfo? typeInfo, string name, List<string> values, ref IParserResult result, HashSet<string> setProperties)
    {
        if(typeInfo == null)
        {
            return false;
        }

        var opName = name.StartsWith("--") ? name[2..] : name[1..];

        ReflectedPropertyInfo? info = null;
            
        if(opName.Length == 1)
        {
            info = typeInfo.FindShortProperty(opName, !Settings.CaseSensitive);
        }

        info ??= typeInfo.FindLongProperty(opName, !Settings.CaseSensitive);

        if(info == null)
        {
            if (!Settings.IgnoreUnknownArguments)
            {
                result.AppendError(name, ParseErrorType.UnknownOption, $"Unknown option: {name}");
                return false;
            }

            return true;
        }

        var loadedValue = GetValues(info, values, ref result);

        if(loadedValue == null)
        {
            result.AppendError(name, ParseErrorType.InvalidValue, $"Failed to convert value for option: {name}");
            return false;
        }

        info.SetValue(value, loadedValue);
        setProperties.Add(info.Property.Name); // Track that this property was set

        return true;
    }

    private object? GetValues(ReflectedPropertyInfo property, List<string> values, ref IParserResult result)
    {
        if(property.IsArray)
        {
            var list = Activator.CreateInstance(property.Type) as IList;

            Debug.Assert(list != null);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(list == null)
            {
                return null;
            }

            // Apply separator splitting if configured
            var processedValues = ApplySeparator(values, property.Attribute.Separator);
                
            foreach (var obj in processedValues.Select(i => GetValue(property.GetElementType()!, i)).OfType<object>())
            {
                list.Add(obj);
            }

            return list;
        }

        if(property.IsFlags)
        {                
            var valueText = string.Join(", ", values.Select(x=>x.Trim()).ToArray());

            if(Enum.TryParse(property.Type, valueText, !Settings.CaseSensitive, out var v))
            {
                return Convert.ChangeType(v, property.Type);
            }

            result.AppendError($"--{property.Attribute.LongName}", ParseErrorType.InvalidValue, $"Failed to convert \"{valueText}\" to Enum {property.Type}");

            return null;
        }

        if(property.Type == typeof(bool) && values.Count ==0)
        {
            // if type is bool
            // have flag means true
            // --param=True is same with --param
            return true;
        }

        return values.Count <=0 ? ObjectCreator.Create(property.Type) : GetValue(property.Type, values.FirstOrDefault()!);
    }

    /// <summary>
    /// Applies the separator to split values based on the configured separator character.
    /// </summary>
    /// <param name="values">The original list of values.</param>
    /// <param name="separator">The separator character used to split values.</param>
    /// <returns>A list of values with separator-based splitting applied.</returns>
    private static List<string> ApplySeparator(List<string> values, char separator)
    {
        // If separator is '\0' (null character), don't split values
        if (separator == '\0' || values.Count == 0)
        {
            return values;
        }
        
        return values.SelectMany(value => value.Split(separator), (_, part) => part.Trim())
            .Where(trimmed => trimmed.Length > 0).ToList();
    }

    private object? GetValue(Type type, string str)
    {
        str = TrimQuotation(str);
        if(type.IsEnum)
        {
            return Enum.TryParse(type, str.Trim(), !Settings.CaseSensitive, out var v) ? v : null;
        }

        if (type == typeof(bool))
        {
            return str.Trim().iEquals("true");
        }

        try
        {
            return Convert.ChangeType(type.IsNumericType() ? str.Trim() : str, type);
        }
        catch (FormatException)
        {
            return null;
        }
        catch (InvalidCastException)
        {
            return null;
        }
        catch (OverflowException)
        {
            return null;
        }
    }

    private static string TrimQuotation(string text)
    {
        return text.Trim('"');
    }

    #endregion

    #endregion

    #region Static Entries
    /// <summary>
    /// The default parser instance with default settings.
    /// </summary>
    public static readonly Parser Default = new();

    #region Format Object To Command Line 
    /// <summary>
    /// Converts an options object into a command-line string.
    /// </summary>
    /// <param name="target">The options object to convert.</param>
    /// <param name="method">The formatting method specifying which options to include.</param>
    /// <returns>A command-line string representation of the options.</returns>
    public static string FormatCommandLine(object target, CommandLineFormatMethod method)
    {
        var typeInfo = GetTypeInfo(target);
         
        var stringBuilder = new StringBuilder();

        // Determine flags
        var isSimplify = method.HasFlag(CommandLineFormatMethod.Simplify);
        var useEqualSign = method.HasFlag(CommandLineFormatMethod.EqualSignStyle);

        // First, output positional arguments in order
        foreach(var property in typeInfo.PositionalProperties)
        {
            var value = property.GetValue(target);

            if(value == null)
            {
                continue;
            }

            var arguments = GetValueString(property, value);

            if(arguments == null || arguments.Length == 0)
            {
                continue;
            }

            var valueText = ToArguments(arguments, false, '\0');

            if (isSimplify && 
                !property.Attribute.Required &&
                typeInfo.DefaultObject != null &&
                ToArguments(GetValueString(property, property.GetValue(typeInfo.DefaultObject)), false, '\0') == valueText
               )
            {
                continue;
            }

            stringBuilder.Append($"{valueText} ");
        }

        // Then, output named options
        foreach(var property in typeInfo.NamedProperties)
        {
            var value = property.GetValue(target);

            if(value == null)
            {
                continue;
            }

            var arguments = GetValueString(property, value);

            if(arguments == null)
            {
                continue;
            }

            // When using equal sign style, arrays should use separator style
            var useSeparator = useEqualSign && property.IsArray;
            var separator = property.Attribute.Separator;
            var valueText = ToArguments(arguments, useSeparator, separator);

            if (isSimplify && 
                !property.Attribute.Required && // if required = true, must set this command line
                typeInfo.DefaultObject != null &&
                ToArguments(GetValueString(property, property.GetValue(typeInfo.DefaultObject)), useSeparator, separator) == valueText
               )
            {
                continue;
            }

            // Format based on style
            if (useEqualSign)
            {
                // For boolean options with true value, use --option=true style
                // For other options, use --option=value style
                stringBuilder.Append($"--{property.Attribute.LongName}={valueText} ");
            }
            else
            {
                stringBuilder.Append($"--{property.Attribute.LongName} {valueText} ");
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts an array of arguments to a formatted string.
    /// </summary>
    /// <param name="arguments">The arguments to convert.</param>
    /// <param name="useSeparator">If true, join arguments with separator instead of space.</param>
    /// <param name="separator">The separator character to use when useSeparator is true.</param>
    /// <returns>A formatted arguments string.</returns>
    private static string ToArguments(string[]? arguments, bool useSeparator, char separator)
    {
        if(arguments == null || arguments.Length == 0)
        {
            return "";
        }

        if (useSeparator && separator != '\0')
        {
            // When using separator style, join with separator (no quotes needed for individual values)
            return string.Join(separator, arguments);
        }

        return string.Join(' ', arguments.Select(argument => argument.Contains(' ') ? $"\"{argument}\"" : argument)); 
    }

    private static string[]? GetValueString(ReflectedPropertyInfo propertyInfo, object? value)
    {
        if(value == null)
        {
            return null;
        }

        if(propertyInfo.IsArray)
        {
            return (value as IEnumerable)?.Select(x => x.ToString()!).ToArray();                
        }

        return propertyInfo.IsFlags ? (value as Enum)!.GetUniqueFlags().Select(x => x.ToString()).ToArray() : [value.ToString()!];
    }

    /// <summary>
    /// Converts an options object into an array of command-line arguments.
    /// </summary>
    /// <param name="target">The options object to convert.</param>
    /// <param name="method">The formatting method specifying which options to include.</param>
    /// <returns>An array of command-line arguments.</returns>
    public static string[] FormatCommandLineArgs(object target, CommandLineFormatMethod method)
    {
        var text = FormatCommandLine(target, method);

        return text.IsNullOrEmpty() ? [] : Splitter.SplitCommandLineIntoArguments(text).ToArray();
    }
    #endregion

    #region Help Text Generator

    /// <summary>
    /// Default number of spaces for left indentation in help text.
    /// </summary>
    public const int DefaultIndent = 4;

    /// <summary>
    /// Default minimum width allocated for option names in help text.
    /// </summary>
    public const int DefaultBlank = 43;

    /// <summary>
    /// Generates help text for all options defined on the target object.
    /// </summary>
    /// <param name="target">The options object to generate help for.</param>
    /// <param name="formatter">The formatter to use for output. If <c>null</c>, uses the default formatter.</param>
    /// <returns>A formatted help text string describing all available options.</returns>
    public static string GetHelpText(object target, IFormatter? formatter = null)
    {
        formatter ??= new Formatter(DefaultIndent, DefaultBlank);

        var typeInfo = GetTypeInfo(target);

        var builder = new StringBuilder();

        // First, show positional arguments
        var positionalProperties = typeInfo.PositionalProperties;
        if (positionalProperties.Length > 0)
        {
            builder.AppendLine("Positional Arguments:");
            foreach(var property in positionalProperties)
            {
                var name = property.Attribute.MetaName.IsNotNullOrEmpty() 
                    ? $"<{property.Attribute.MetaName}>" 
                    : $"<arg{property.Attribute.Index}>";

                var attribute = GetAttribute(property, typeInfo);
                var usage = GenUsageHelp(property);

                formatter.Append(builder, name, attribute, property.Attribute.HelpText, usage);
            }
            builder.AppendLine();
        }

        // Then, show named options
        var namedProperties = typeInfo.NamedProperties;
        if (namedProperties.Length > 0)
        {
            builder.AppendLine("Options:");
            foreach(var property in namedProperties)
            {
                var name = property.Attribute.ShortName.IsNotNullOrEmpty() 
                    ? $"-{property.Attribute.ShortName}, --{property.Attribute.LongName}" 
                    : $"--{property.Attribute.LongName}";

                var attribute = GetAttribute(property, typeInfo);
                var usage = GenUsageHelp(property);

                formatter.Append(builder, name, attribute, property.Attribute.HelpText, usage);
            }
        }

        return builder.ToString();
    }

    private static string GetAttribute(ReflectedPropertyInfo property, TypeInfo typeInfo)
    {
        var list = new List<string>();
        if(!property.Attribute.Required)
        {
            list.Add("Optional");
        }

        if (property.Attribute.IsPositional)
        {
            list.Add($"Index:{property.Attribute.Index}");
        }

        if(property.IsArray)
        {
            list.Add("Array");
        }

        if(property.IsFlags)
        {
            list.Add("Flags");
        }
        else if(property.Type.IsEnum)
        {
            list.Add("Enum");
        }

        // Show default value if available
        if (typeInfo.DefaultObject != null)
        {
            var defaultValue = property.GetValue(typeInfo.DefaultObject);
            if (defaultValue != null)
            {
                var defaultStr = defaultValue.ToString();
                // Only show non-empty, non-default values
                if (!string.IsNullOrEmpty(defaultStr) && 
                    defaultStr != "0" && 
                    defaultStr != "False" &&
                    defaultStr != property.Type.Name)
                {
                    list.Add($"Default:{defaultStr}");
                }
            }
        }

        // Show environment variable if configured
        if (!string.IsNullOrEmpty(property.Attribute.EnvironmentVariable))
        {
            list.Add($"Env:{property.Attribute.EnvironmentVariable}");
        }

        return string.Join(',', list.ToArray());
    }

    private static string GenUsageHelp(ReflectedPropertyInfo property)
    {
        // For positional arguments
        if (property.Attribute.IsPositional)
        {
            var metaName = property.Attribute.MetaName.IsNotNullOrEmpty()
                ? property.Attribute.MetaName
                : $"arg{property.Attribute.Index}";
            return $"<{metaName}>";
        }

        // this is flags enum
        if(property.IsFlags)
        {
            var names = Enum.GetNames(property.Property.PropertyType);
                
            return $"--{property.Attribute.LongName} {string.Join(' ', names)}";
        }

        if(property.Type.IsEnum)
        {
            var names = Enum.GetNames(property.Property.PropertyType);
            var builder = new StringBuilder();
                
            foreach (var t in names)
            {
                builder.Append(t);
                builder.Append(' ');
            }

            return $"--{property.Attribute.LongName} {builder}";
        }

        // array support
        return property.IsArray ? $"--{property.Attribute.LongName} {property.GetElementType()?.Name}1 {property.GetElementType()?.Name}2" : "";
    }
    #endregion
    #endregion
}