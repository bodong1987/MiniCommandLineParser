// ReSharper disable ConvertToConstant.Local
// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace MiniCommandLineParser.UnitTests;

#region Test Option Classes

/// <summary>
/// Basic options class for simple parsing tests.
/// </summary>
public class BasicOptions
{
    [Option('v', "verbose", HelpText = "Enable verbose output")]
    public bool Verbose { get; set; }

    [Option('n', "name", HelpText = "The name parameter")]
    public string? Name { get; set; }

    [Option('c', "count", HelpText = "The count parameter")]
    public int Count { get; set; }
}

/// <summary>
/// Options class with required fields.
/// </summary>
public class RequiredOptions
{
    [Option('i', "input", Required = true, HelpText = "Required input file")]
    public string? Input { get; set; }

    [Option('o', "output", HelpText = "Optional output file")]
    public string? Output { get; set; }
}

/// <summary>
/// Options class with array/list support.
/// </summary>
public class ArrayOptions
{
    [Option('f', "files", HelpText = "List of files")]
    public List<string>? Files { get; set; }

    [Option('n', "numbers", HelpText = "List of numbers")]
    public List<int>? Numbers { get; set; }
}

/// <summary>
/// Options class with custom separator for array values.
/// </summary>
public class SeparatorArrayOptions
{
    [Option('f', "files", Separator = ',', HelpText = "List of files separated by comma")]
    public List<string>? Files { get; set; }

    [Option('n', "numbers", Separator = ':', HelpText = "List of numbers separated by colon")]
    public List<int>? Numbers { get; set; }

    [Option('t', "tags", Separator = ';', HelpText = "List of tags separated by semicolon (default)")]
    public List<string>? Tags { get; set; }
}

/// <summary>
/// Options class for testing various List types.
/// </summary>
public class ExtendedArrayOptions
{
    [Option('d', "doubles", HelpText = "List of double values")]
    public List<double>? Doubles { get; set; }

    [Option('b', "bools", HelpText = "List of boolean values")]
    public List<bool>? Bools { get; set; }

    [Option('e', "levels", HelpText = "List of enum values")]
    public List<LogLevel>? Levels { get; set; }

    [Option('s', "strings", HelpText = "List of strings")]
    public List<string>? Strings { get; set; }

    [Option("other", HelpText = "Another option")]
    public string? Other { get; set; }
}

/// <summary>
/// Options class for testing mixed array and non-array options.
/// </summary>
public class MixedArrayOptions
{
    [Option('v', "verbose", HelpText = "Verbose mode")]
    public bool Verbose { get; set; }

    [Option('f', "files", HelpText = "List of files")]
    public List<string>? Files { get; set; }

    [Option('o', "output", HelpText = "Output path")]
    public string? Output { get; set; }

    [Option('c', "counts", HelpText = "List of counts")]
    public List<int>? Counts { get; set; }
}

/// <summary>
/// Options class with enum support.
/// </summary>
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

[Flags]
public enum FileAccess
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}

public class EnumOptions
{
    [Option('l', "level", HelpText = "Log level")]
    public LogLevel Level { get; set; }

    [Option('a', "access", HelpText = "File access flags")]
    public FileAccess Access { get; set; }
}

/// <summary>
/// Options class using property name as long name.
/// </summary>
public class AutoNameOptions
{
    [Option(HelpText = "Auto-named option")]
    public string? AutoProperty { get; set; }

    [Option("explicit-name", HelpText = "Explicit long name")]
    public string? ExplicitName { get; set; }
}

/// <summary>
/// Options class with various data types.
/// </summary>
public class TypedOptions
{
    [Option("int-value")]
    public int IntValue { get; set; }

    [Option("double-value")]
    public double DoubleValue { get; set; }

    [Option("bool-value")]
    public bool BoolValue { get; set; }

    [Option("string-value")]
    public string? StringValue { get; set; }
}

/// <summary>
/// Options class with BindingList for various types.
/// </summary>
public class BindingListOptions
{
    [Option('s', "strings", HelpText = "BindingList of strings")]
    public System.ComponentModel.BindingList<string>? Strings { get; set; }

    [Option('i', "integers", HelpText = "BindingList of integers")]
    public System.ComponentModel.BindingList<int>? Integers { get; set; }
}

/// <summary>
/// Options class with BindingList for bool values.
/// </summary>
public class BindingListBoolOptions
{
    [Option('b', "bools", HelpText = "BindingList of booleans")]
    public System.ComponentModel.BindingList<bool>? Bools { get; set; }
}

/// <summary>
/// Options class with BindingList for numeric types.
/// </summary>
public class BindingListNumericOptions
{
    [Option('d', "doubles", HelpText = "BindingList of doubles")]
    public System.ComponentModel.BindingList<double>? Doubles { get; set; }

    [Option('f', "floats", HelpText = "BindingList of floats")]
    public System.ComponentModel.BindingList<float>? Floats { get; set; }

    [Option('l', "longs", HelpText = "BindingList of longs")]
    public System.ComponentModel.BindingList<long>? Longs { get; set; }

    [Option("decimals", HelpText = "BindingList of decimals")]
    public System.ComponentModel.BindingList<decimal>? Decimals { get; set; }

    [Option("shorts", HelpText = "BindingList of shorts")]
    public System.ComponentModel.BindingList<short>? Shorts { get; set; }

    [Option("bytes", HelpText = "BindingList of bytes")]
    public System.ComponentModel.BindingList<byte>? Bytes { get; set; }
}

/// <summary>
/// Options class with BindingList for enum values.
/// </summary>
public class BindingListEnumOptions
{
    [Option('e', "levels", HelpText = "BindingList of log levels")]
    public System.ComponentModel.BindingList<LogLevel>? Levels { get; set; }
}

/// <summary>
/// Options class with BindingList using custom separator.
/// </summary>
public class BindingListSeparatorOptions
{
    [Option('s', "strings", Separator = ',', HelpText = "BindingList with comma separator")]
    public System.ComponentModel.BindingList<string>? Strings { get; set; }

    [Option('i', "integers", Separator = ':', HelpText = "BindingList with colon separator")]
    public System.ComponentModel.BindingList<int>? Integers { get; set; }

    [Option('d', "doubles", Separator = ';', HelpText = "BindingList with semicolon separator")]
    public System.ComponentModel.BindingList<double>? Doubles { get; set; }
}

/// <summary>
/// Options class with mixed BindingList and other options.
/// </summary>
public class BindingListMixedOptions
{
    [Option('v', "verbose", HelpText = "Verbose mode")]
    public bool Verbose { get; set; }

    [Option('f', "files", HelpText = "BindingList of files")]
    public System.ComponentModel.BindingList<string>? Files { get; set; }

    [Option('o', "output", HelpText = "Output path")]
    public string? Output { get; set; }

    [Option('c', "counts", HelpText = "BindingList of counts")]
    public System.ComponentModel.BindingList<int>? Counts { get; set; }
}

/// <summary>
/// Test options class with positional arguments like: app.exe clone https://example.com
/// </summary>
public class CloneCommandOptions
{
    [Option(Index = 0, MetaName = "COMMAND", HelpText = "The command to execute")]
    public string Command { get; set; } = "";

    [Option(Index = 1, MetaName = "URL", HelpText = "The URL to clone")]
    public string Url { get; set; } = "";

    [Option('v', "verbose", HelpText = "Enable verbose output")]
    public bool Verbose { get; set; }
}

/// <summary>
/// Test options class mixing positional and named arguments
/// </summary>
public class MixedOptions
{
    [Option(Index = 0, MetaName = "ACTION", HelpText = "The action to perform")]
    public string Action { get; set; } = "";

    [Option(Index = 1, MetaName = "TARGET", HelpText = "The target of the action")]
    public string Target { get; set; } = "";

    [Option('o', "output", HelpText = "Output directory")]
    public string Output { get; set; } = "";

    [Option('f', "force", HelpText = "Force operation")]
    public bool Force { get; set; }
}

/// <summary>
/// Test options with positional integer argument
/// </summary>
public class PositionalIntOptions
{
    [Option(Index = 0, MetaName = "COUNT", HelpText = "Number of items")]
    public int Count { get; set; }

    [Option(Index = 1, MetaName = "NAME", HelpText = "Item name")]
    public string Name { get; set; } = "";
}

#endregion
