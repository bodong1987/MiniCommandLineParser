# MiniCommandLineParser

[![NuGet](https://img.shields.io/nuget/v/MiniCommandLineParser.svg)](https://www.nuget.org/packages/MiniCommandLineParser/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%207.0%20%7C%208.0%20%7C%209.0-purple.svg)](https://dotnet.microsoft.com/)

A **simple**, **lightweight**, and **dependency-free** command-line parsing library for .NET.

## 🎯 Why MiniCommandLineParser?

| Feature | MiniCommandLineParser | Other Libraries |
|---------|----------------------|-----------------|
| **Zero Dependencies** | ✅ No external packages | ❌ Often require multiple dependencies |
| **Lightweight** | ✅ Minimal footprint | ❌ Can be bloated |
| **Bidirectional** | ✅ Parse & Format | ❌ Usually parse-only |
| **Environment Variables** | ✅ Built-in support | ⚠️ Often requires extra code |
| **Structured Errors** | ✅ Typed error handling | ❌ Usually string-only errors |
| **Learning Curve** | ✅ Simple, intuitive API | ❌ Complex configurations |
| **Multi-target** | ✅ .NET 6/7/8/9 + Standard 2.1 | ⚠️ Varies |

## ✨ Features

- 🪶 **Lightweight** - Minimal footprint with zero external dependencies
- 🎯 **Simple API** - Intuitive attribute-based configuration
- 📦 **Multi-target** - Supports .NET 6.0, 7.0, 8.0, 9.0 and .NET Standard 2.1
- 🔄 **Bidirectional** - Parse arguments to objects AND format objects back to command-line strings
- 📝 **Auto Help Text** - Built-in help text generation with default values and environment variable display
- 🔧 **Flexible** - Supports short/long options, positional arguments, arrays, enums, flags, and more
- 🌍 **Environment Variables** - Fallback to environment variables when command-line options not provided
- ⚠️ **Structured Errors** - Typed error handling with `ParseError` and `ParseErrorType`
- 🧵 **Thread-Safe** - Type information caching is thread-safe for concurrent usage
- ⚡ **High Performance** - Efficient parsing with TypeInfo caching and minimal allocations
- 📍 **Positional Arguments** - Support for index-based arguments like `git clone <url>`
- 🔗 **Custom Separators** - Define custom separators for array values (e.g., `--tags=a;b;c`)
- ✅ **Boolean Flexibility** - Multiple syntaxes: `--flag`, `--flag=true`, `--flag true`

## 📥 Installation

### Package Manager Console

```powershell
Install-Package MiniCommandLineParser
```

### .NET CLI

```bash
dotnet add package MiniCommandLineParser
```

### PackageReference

```xml
<PackageReference Include="MiniCommandLineParser" Version="*" />
```

## 🚀 Quick Start

### Step 1: Define Your Options Class

```csharp
using MiniCommandLineParser;

public class Options
{
    [Option('i', "input", Required = true, HelpText = "Input file path")]
    public string InputFile { get; set; }

    [Option('o', "output", HelpText = "Output file path")]
    public string OutputFile { get; set; }

    [Option('v', "verbose", HelpText = "Enable verbose output")]
    public bool Verbose { get; set; }

    [Option("count", HelpText = "Number of iterations")]
    public int Count { get; set; } = 1;
}
```

### Step 2: Parse Command-Line Arguments

```csharp
using MiniCommandLineParser;

class Program
{
    static void Main(string[] args)
    {
        var result = Parser.Default.Parse<Options>(args);

        if (result.Result == ParserResultType.Parsed)
        {
            var options = result.Value;
            Console.WriteLine($"Input: {options.InputFile}");
            Console.WriteLine($"Output: {options.OutputFile}");
            Console.WriteLine($"Verbose: {options.Verbose}");
            Console.WriteLine($"Count: {options.Count}");
        }
        else
        {
            Console.WriteLine($"Error: {result.ErrorMessage}");
            
            // Access structured errors for detailed handling
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  [{error.ErrorType}] {error.OptionName}: {error.Message}");
            }
        }
    }
}
```

### Step 3: Run Your Application

```bash
myapp --input data.txt --output result.txt --verbose --count 5
# or using short names
myapp -i data.txt -o result.txt -v --count 5
```

## 📖 Detailed Usage

### Option Attribute

The `[Option]` attribute is used to mark properties as command-line options:

```csharp
public class Options
{
    // Short name only (property name becomes long name)
    [Option('v')]
    public bool Verbose { get; set; }

    // Long name only
    [Option("configuration")]
    public string Config { get; set; }

    // Both short and long names
    [Option('o', "output")]
    public string OutputPath { get; set; }

    // No parameters (property name becomes long name)
    [Option]
    public string DefaultName { get; set; }

    // Required option
    [Option('i', "input", Required = true, HelpText = "Required input file")]
    public string Input { get; set; }

    // With help text
    [Option("timeout", HelpText = "Timeout in seconds")]
    public int Timeout { get; set; } = 30;

    // Positional argument (Index >= 0 makes it positional)
    [Option(Index = 0, MetaName = "COMMAND", HelpText = "The command to execute")]
    public string Command { get; set; }

    // Array with custom separator
    [Option("tags", Separator = ';', HelpText = "Tags separated by semicolon")]
    public List<string> Tags { get; set; }

    // With environment variable fallback
    [Option("api-key", EnvironmentVariable = "API_KEY", HelpText = "API key for authentication")]
    public string ApiKey { get; set; }
}
```

### Supported Argument Formats

MiniCommandLineParser supports various argument formats:

```bash
# Short options
-v -i input.txt

# Long options
--verbose --input input.txt

# Equals syntax
--input=input.txt
-i=input.txt

# Quoted values (for paths with spaces)
--output "my output file.txt"
--path="C:\Program Files\MyApp"

# Boolean flags - multiple equivalent syntaxes
--verbose           # Sets Verbose = true (flag presence)
--verbose=true      # Sets Verbose = true (equals syntax)
--verbose true      # Sets Verbose = true (space syntax)
--verbose=false     # Sets Verbose = false
--verbose false     # Sets Verbose = false
-v                  # Short form, sets Verbose = true
-v=true             # Short form with equals
-v false            # Short form with space

# Positional arguments (no option prefix needed)
myapp clone http://example.com --verbose
# Where 'clone' is Index=0 and 'http://example.com' is Index=1

# Array with custom separator (using equals syntax)
--tags=dev;test;prod    # Parsed using ';' separator
--ids=1,2,3,4           # Parsed using ',' separator
```

### Array/List Support

Support for collection types like `List<T>` and arrays:

```csharp
public class Options
{
    [Option("files", HelpText = "Input files to process")]
    public List<string> Files { get; set; }

    [Option("numbers", HelpText = "Numbers to sum")]
    public int[] Numbers { get; set; }

    // With custom separator - elements can be passed in a single value
    [Option("tags", Separator = ';', HelpText = "Tags separated by semicolon")]
    public List<string> Tags { get; set; }

    [Option("ids", Separator = ',', HelpText = "IDs separated by comma")]
    public List<int> Ids { get; set; }
}
```

Usage:

```bash
# Space-separated (default)
myapp --files file1.txt file2.txt file3.txt --numbers 1 2 3 4 5

# Using custom separator with equals syntax
myapp --tags=dev;test;prod --ids=1,2,3,4,5

# Mixed: separator works with both syntaxes
myapp --tags dev;test;prod
myapp --tags "dev;test;prod"
```

### Enum Support

Both regular enums and flags enums are supported:

```csharp
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

[Flags]
public enum Features
{
    None = 0,
    Logging = 1,
    Caching = 2,
    Compression = 4,
    All = Logging | Caching | Compression
}

public class Options
{
    [Option("level", HelpText = "Log level")]
    public LogLevel Level { get; set; } = LogLevel.Info;

    [Option("features", HelpText = "Enabled features")]
    public Features EnabledFeatures { get; set; }
}
```

Usage:

```bash
# Regular enum
myapp --level Warning

# Flags enum (multiple values)
myapp --features Logging Caching Compression
```

### Positional Arguments

Positional arguments are values that don't require an option prefix. They are identified by their position in the command line, similar to how `git clone <url>` works.

```csharp
public class GitCloneOptions
{
    // Index = 0 means this is the first positional argument
    [Option(Index = 0, MetaName = "COMMAND", HelpText = "Git command to execute")]
    public string Command { get; set; } = "";

    // Index = 1 means this is the second positional argument
    [Option(Index = 1, MetaName = "URL", HelpText = "Repository URL to clone")]
    public string Url { get; set; } = "";

    // Regular named options still work alongside positional arguments
    [Option('v', "verbose", HelpText = "Enable verbose output")]
    public bool Verbose { get; set; }

    [Option("depth", HelpText = "Create a shallow clone with specified depth")]
    public int Depth { get; set; }
}
```

Usage:

```bash
# Positional arguments come first (or can be mixed with named options)
myapp clone https://github.com/user/repo.git --verbose --depth 1

# Named options can appear before positional arguments too
myapp --verbose clone https://github.com/user/repo.git --depth 1

# The parser correctly identifies positional vs named arguments
```

**Key Points:**
- Use `Index` property to define positional arguments (Index >= 0)
- `MetaName` provides a display name for help text (e.g., "URL" instead of the property name)
- Positional arguments are matched in order by their Index value
- Named options (with `-` or `--` prefix) can appear anywhere in the command line
- Positional and named options can be freely mixed

### Supported Data Types

| Type | Example | Notes |
|------|---------|-------|
| `string` | `--name "John Doe"` | Supports quoted values |
| `int`, `long`, `short` | `--count 42` | All integer types |
| `float`, `double`, `decimal` | `--rate 3.14` | Floating-point types |
| `bool` | `--verbose` or `--flag=true` | Flag presence = true; also supports `--flag true` |
| `enum` | `--level Info` | Case-insensitive by default |
| `[Flags] enum` | `--flags A B C` | Multiple space-separated values |
| `List<T>`, `T[]` | `--items a b c` | Any supported element type |
| Arrays with separator | `--tags=a;b;c` | Use `Separator` property in attribute |

## 🌍 Environment Variable Support

MiniCommandLineParser supports fallback to environment variables when command-line options are not provided. This is useful for configuration that may come from either the command line or the environment.

### Basic Usage

```csharp
public class Options
{
    [Option('c', "config", EnvironmentVariable = "APP_CONFIG", HelpText = "Config file path")]
    public string ConfigFile { get; set; }

    [Option("api-key", EnvironmentVariable = "API_KEY", HelpText = "API key for authentication")]
    public string ApiKey { get; set; }

    [Option("timeout", EnvironmentVariable = "APP_TIMEOUT", HelpText = "Timeout in seconds")]
    public int Timeout { get; set; } = 30;

    [Option("features", EnvironmentVariable = "APP_FEATURES", HelpText = "Enabled features")]
    public List<string> Features { get; set; }
}
```

### Priority Order

Values are resolved in the following order:

1. **Command-line argument** (highest priority)
2. **Environment variable** (if command-line not provided)
3. **Default value** (if neither provided)

```csharp
// Example: If API_KEY environment variable is set to "env-key-123"
var result = Parser.Default.Parse<Options>("--timeout 60");

// result.Value.ApiKey == "env-key-123"  (from environment)
// result.Value.Timeout == 60            (from command line)
// result.Value.ConfigFile == null       (default, nothing set)
```

### Environment Variables with Arrays

For array/list properties, environment variable values are split by comma:

```csharp
public class Options
{
    [Option("tags", EnvironmentVariable = "APP_TAGS", HelpText = "Tags")]
    public List<string> Tags { get; set; }
}

// If APP_TAGS="dev,test,prod"
// result.Value.Tags == ["dev", "test", "prod"]
```

### Environment Variables with Required Options

Environment variables can satisfy required options:

```csharp
public class Options
{
    [Option('k', "key", Required = true, EnvironmentVariable = "API_KEY", HelpText = "API key (required)")]
    public string ApiKey { get; set; }
}

// If API_KEY environment variable is set, the required validation passes
// even without providing --key on the command line
```

### Help Text Display

Environment variable names are automatically shown in help text:

```
Options:
    -c, --config                                [Optional,Env:APP_CONFIG] Config file path
    --api-key                                   [Optional,Env:API_KEY] API key for authentication
    --timeout                                   [Optional,Default:30,Env:APP_TIMEOUT] Timeout in seconds
```

## ⚠️ Structured Error Handling

MiniCommandLineParser provides typed error handling for better error management and user feedback.

### ParseError Class

When parsing fails, you can access detailed error information:

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result == ParserResultType.NotParsed)
{
    // Simple error message
    Console.WriteLine($"Error: {result.ErrorMessage}");
    
    // Detailed structured errors
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"Option: {error.OptionName}");
        Console.WriteLine($"Type: {error.ErrorType}");
        Console.WriteLine($"Message: {error.Message}");
    }
}
```

### Error Types

| ErrorType | Description | Example |
|-----------|-------------|---------|
| `MissingRequired` | A required option was not provided | `--input` is required but not given |
| `InvalidValue` | The value could not be converted to the expected type | `--count abc` when int expected |
| `UnknownOption` | An unrecognized option was provided | `--unknown-opt` not defined |

### Handling Specific Error Types

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result == ParserResultType.NotParsed)
{
    foreach (var error in result.Errors)
    {
        switch (error.ErrorType)
        {
            case ParseErrorType.MissingRequired:
                Console.WriteLine($"❌ Missing required option: {error.OptionName}");
                Console.WriteLine($"   Please provide a value for {error.OptionName}");
                break;
                
            case ParseErrorType.InvalidValue:
                Console.WriteLine($"❌ Invalid value for {error.OptionName}");
                Console.WriteLine($"   {error.Message}");
                break;
                
            case ParseErrorType.UnknownOption:
                Console.WriteLine($"⚠️ Unknown option: {error.OptionName}");
                Console.WriteLine($"   Did you mean one of the known options?");
                break;
        }
    }
    
    // Show help text
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine(Parser.GetHelpText(new Options()));
}
```

### Multiple Errors

The parser collects all errors, not just the first one:

```csharp
// Command: myapp --count abc --unknown-opt
// Results in two errors:
// 1. InvalidValue for --count
// 2. UnknownOption for --unknown-opt (if IgnoreUnknownArguments = false)

foreach (var error in result.Errors)
{
    Console.WriteLine($"[{error.ErrorType}] {error.OptionName}: {error.Message}");
}
```

## ⚙️ Parser Configuration

### Custom Parser Settings

```csharp
var parser = new Parser(new ParserSettings
{
    // Case-sensitive option matching (default: false)
    CaseSensitive = false,
    
    // Ignore unknown arguments (default: true)
    IgnoreUnknownArguments = true
});

var result = parser.Parse<Options>(args);
```

### Parser Settings Reference

| Setting | Default | Description |
|---------|---------|-------------|
| `CaseSensitive` | `false` | When `false`, `--Input` matches `--input` |
| `IgnoreUnknownArguments` | `true` | When `true`, unknown args are silently ignored |

## 🔄 Bidirectional Conversion

### Format Object to Command Line

A unique feature of MiniCommandLineParser is the ability to convert options objects back to command-line strings:

```csharp
var options = new Options
{
    InputFile = "data.txt",
    OutputFile = "result.txt",
    Verbose = true,
    Count = 5
};

// Complete format - includes all options (space-separated style)
string complete = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --input data.txt --output result.txt --verbose True --count 5

// Simplified format - only non-default values
string simplified = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);
// Output: --input data.txt --output result.txt --verbose True --count 5

// Equal sign style - uses --option=value syntax
string equalStyle = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);
// Output: --input=data.txt --output=result.txt --verbose=True --count=5

// Combine flags: Simplify + EqualSignStyle
string combined = Parser.FormatCommandLine(options, 
    CommandLineFormatMethod.Simplify | CommandLineFormatMethod.EqualSignStyle);
// Output: --input=data.txt --output=result.txt --verbose=True --count=5

// Get as string array
string[] args = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Simplify);
```

### Array Formatting

Arrays are formatted differently based on the style:

```csharp
public class BuildOptions
{
    [Option("tags", Separator = ';')]
    public List<string> Tags { get; set; }
    
    [Option("ids", Separator = ',')]
    public int[] Ids { get; set; }
}

var options = new BuildOptions 
{ 
    Tags = new List<string> { "dev", "test", "prod" },
    Ids = new[] { 1, 2, 3 }
};

// Space-separated style (Complete/Simplify without EqualSignStyle)
Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --tags dev test prod --ids 1 2 3

// Equal sign style - arrays automatically use their defined separator
Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);
// Output: --tags=dev;test;prod --ids=1,2,3
```

### Format Method Flags

| Flag | Description |
|------|-------------|
| `None` | Default space-separated style |
| `Complete` | Output all options including default values |
| `Simplify` | Only output options that differ from defaults |
| `EqualSignStyle` | Use `--option=value` syntax; arrays use their defined separator |

> **Note**: Flags can be combined using the `|` operator for flexible output formatting.

### Use Cases

- **Configuration persistence**: Save/restore command-line configurations
- **Process spawning**: Launch child processes with the same options
- **Logging/Debugging**: Log the effective command-line for diagnostics
- **Testing**: Generate test command lines programmatically

## 📝 Help Text Generation

### Auto-Generate Help Text

```csharp
var options = new Options();
string helpText = Parser.GetHelpText(options);
Console.WriteLine(helpText);
```

Output example:

```
Positional Arguments:
    <COMMAND>                                   [Optional,Index:0] Command to execute

Options:
    -i, --input                                 [Required] Input file path
    -o, --output                                [Optional] Output file path
    -v, --verbose                               [Optional] Enable verbose output
    --count                                     [Optional,Default:1] Number of iterations
    --level                                     [Optional,Enum,Default:Info] Log level
                                                --level Debug Info Warning Error
    --api-key                                   [Optional,Env:API_KEY] API key for authentication
    --features                                  [Optional,Flags] Enabled features
                                                --features Logging Caching Compression
```

### Help Text Features

The auto-generated help text includes:

- **Option names** (short and long forms)
- **Required/Optional** status
- **Default values** (when non-trivial)
- **Environment variable names** (when configured)
- **Type information** (Array, Enum, Flags)
- **Usage examples** for enums and flags

### Custom Formatter

Implement `IFormatter` for custom help text formatting:

```csharp
public interface IFormatter
{
    void Append(StringBuilder stringBuilder, string name, string attributes, 
                string? helpText, string? usage);
}
```

```csharp
// Use custom formatter
var customFormatter = new MyCustomFormatter();
string helpText = Parser.GetHelpText(options, customFormatter);
```

### Formatting Options

```csharp
// Default indent and blank spaces
Parser.DefaultIndent  // 4 spaces for left indentation
Parser.DefaultBlank   // 43 characters for option name column width
```

## ⚡ Performance

MiniCommandLineParser is designed for high performance with minimal overhead.

### TypeInfo Caching

Type metadata is cached and reused across parse calls:

```csharp
// First call analyzes the type and caches the result
var result1 = Parser.Default.Parse<Options>(args1);

// Subsequent calls reuse the cached type information
var result2 = Parser.Default.Parse<Options>(args2);

// You can also access the cached TypeInfo directly
var typeInfo = Parser.GetTypeInfo<Options>();
```

### Thread Safety

- The TypeInfo cache uses `ConcurrentDictionary` for thread-safe access
- Multiple threads can parse different option types simultaneously
- Safe for use in multi-threaded applications and web servers

### Performance Characteristics

| Operation | Complexity | Notes |
|-----------|------------|-------|
| First parse of a type | O(n) | Reflection + caching |
| Subsequent parses | O(m) | Cached metadata, m = argument count |
| TypeInfo lookup | O(1) | ConcurrentDictionary lookup |
| Format to command line | O(p) | p = property count |

### Best Practices for Performance

```csharp
// ✅ Good: Reuse Parser instance
var parser = new Parser(settings);
for (int i = 0; i < 1000; i++)
{
    var result = parser.Parse<Options>(GetArgs(i));
}

// ✅ Good: Use Parser.Default for default settings
var result = Parser.Default.Parse<Options>(args);

// ⚠️ Avoid: Creating new Parser instances unnecessarily
// (Though the TypeInfo cache is global, so this is still efficient)
```

## 🔍 API Reference

### Parser Class

| Method | Description |
|--------|-------------|
| `Parse<T>(string arguments)` | Parse a command-line string |
| `Parse<T>(IEnumerable<string> arguments)` | Parse an array of arguments |
| `Parse<T>(string arguments, T value)` | Parse into an existing instance |
| `Parse<T>(IEnumerable<string> arguments, T value)` | Parse array into existing instance |
| `FormatCommandLine(object, CommandLineFormatMethod)` | Convert object to command-line string |
| `FormatCommandLineArgs(object, CommandLineFormatMethod)` | Convert object to string array |
| `GetHelpText(object, IFormatter?)` | Generate help text |
| `GetTypeInfo<T>()` | Get cached type metadata |
| `GetTypeInfo(Type type)` | Get cached type metadata by Type |
| `GetTypeInfo(object target)` | Get cached type metadata from instance |

### Static Members

| Member | Description |
|--------|-------------|
| `Parser.Default` | Default parser instance with default settings |
| `Parser.DefaultIndent` | Default indentation for help text (4 spaces) |
| `Parser.DefaultBlank` | Default option name column width (43 chars) |

### ParserResult\<T\>

| Property | Type | Description |
|----------|------|-------------|
| `Result` | `ParserResultType` | `Parsed` or `NotParsed` |
| `Value` | `T` | The parsed options object |
| `ErrorMessage` | `string` | Combined error message if parsing failed |
| `Errors` | `IReadOnlyList<ParseError>` | List of structured errors |
| `Type` | `TypeInfo` | Type metadata information |

### ParseError

| Property | Type | Description |
|----------|------|-------------|
| `OptionName` | `string` | Name of the option that caused the error |
| `ErrorType` | `ParseErrorType` | Type of error (MissingRequired, InvalidValue, UnknownOption) |
| `Message` | `string` | Detailed error message |

### ParseErrorType

| Value | Description |
|-------|-------------|
| `MissingRequired` | A required option was not provided |
| `InvalidValue` | The value could not be converted to the expected type |
| `UnknownOption` | An unrecognized option was provided |

### OptionAttribute

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ShortName` | `string?` | `null` | Single character option (e.g., 'v' for `-v`) |
| `LongName` | `string?` | `null` | Full option name (e.g., "verbose" for `--verbose`) |
| `Required` | `bool` | `false` | Whether the option must be provided |
| `HelpText` | `string?` | `null` | Description shown in help output |
| `Index` | `int` | `-1` | Positional argument index (>= 0 makes it positional) |
| `MetaName` | `string?` | `null` | Display name for positional arguments in help text |
| `Separator` | `char` | `';'` | Separator character for array/list values |
| `EnvironmentVariable` | `string?` | `null` | Environment variable name for fallback value |

### CommandLineFormatMethod (Flags Enum)

| Flag | Value | Description |
|------|-------|-------------|
| `None` | 0 | Default space-separated style |
| `Complete` | 1 | Output all options including defaults |
| `Simplify` | 2 | Only output non-default values |
| `EqualSignStyle` | 4 | Use `--option=value` syntax |

### TypeInfo

| Property | Description |
|----------|-------------|
| `Properties` | All option properties for the type |
| `PositionalProperties` | Properties with Index >= 0 (sorted by Index) |
| `NamedProperties` | Properties without positional index |
| `DefaultObject` | Default instance for comparison |

## 💡 Best Practices

### 1. Use Meaningful Option Names

```csharp
// Good
[Option('o', "output", HelpText = "Output file path")]
public string OutputPath { get; set; }

// Avoid
[Option('x', "x")]
public string X { get; set; }
```

### 2. Provide Default Values

```csharp
public class Options
{
    [Option("timeout", HelpText = "Request timeout in seconds")]
    public int Timeout { get; set; } = 30;  // Sensible default

    [Option("retries", HelpText = "Number of retry attempts")]
    public int Retries { get; set; } = 3;
}
```

### 3. Always Check Parse Result

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result != ParserResultType.Parsed)
{
    Console.Error.WriteLine(result.ErrorMessage);
    Console.WriteLine(Parser.GetHelpText(new Options()));
    return 1;  // Exit with error code
}

// Safe to use result.Value here
```

### 4. Use Required for Mandatory Options

```csharp
[Option('i', "input", Required = true, HelpText = "Input file (required)")]
public string InputFile { get; set; }
```

### 5. Use Environment Variables for Sensitive Data

```csharp
public class Options
{
    // API keys and secrets should come from environment
    [Option("api-key", EnvironmentVariable = "API_KEY", HelpText = "API key")]
    public string ApiKey { get; set; }
    
    // Database connection strings
    [Option("connection", EnvironmentVariable = "DB_CONNECTION", HelpText = "Database connection")]
    public string ConnectionString { get; set; }
}
```

### 6. Handle Errors Gracefully

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result != ParserResultType.Parsed)
{
    // Provide specific feedback based on error type
    foreach (var error in result.Errors)
    {
        switch (error.ErrorType)
        {
            case ParseErrorType.MissingRequired:
                Console.WriteLine($"Missing: {error.OptionName}");
                break;
            case ParseErrorType.InvalidValue:
                Console.WriteLine($"Invalid: {error.Message}");
                break;
        }
    }
    return 1;
}
```

## 🧪 Example Project

Here's a complete example demonstrating all features:

```csharp
using MiniCommandLineParser;

public enum OutputFormat { Text, Json, Xml }

[Flags]
public enum ProcessingFlags
{
    None = 0,
    Validate = 1,
    Transform = 2,
    Compress = 4
}

public class Options
{
    // Positional argument - the command to execute
    [Option(Index = 0, MetaName = "COMMAND", HelpText = "Command to execute (process, convert, analyze)")]
    public string Command { get; set; } = "";

    // Positional argument - the input file
    [Option(Index = 1, MetaName = "INPUT", HelpText = "Input file path")]
    public string InputFile { get; set; } = "";

    [Option('o', "output", HelpText = "Output file path")]
    public string OutputFile { get; set; }

    [Option('f', "format", HelpText = "Output format")]
    public OutputFormat Format { get; set; } = OutputFormat.Text;

    [Option("flags", HelpText = "Processing flags")]
    public ProcessingFlags Flags { get; set; } = ProcessingFlags.Validate;

    // Array with custom separator
    [Option("tags", Separator = ';', HelpText = "Tags for the output (semicolon-separated)")]
    public List<string> Tags { get; set; }

    [Option("include", HelpText = "Additional files to include")]
    public List<string> IncludeFiles { get; set; }

    [Option('v', "verbose", HelpText = "Enable verbose logging")]
    public bool Verbose { get; set; }

    [Option("threads", HelpText = "Number of worker threads")]
    public int ThreadCount { get; set; } = Environment.ProcessorCount;

    // Environment variable fallback for sensitive configuration
    [Option("api-key", EnvironmentVariable = "APP_API_KEY", HelpText = "API key for external service")]
    public string ApiKey { get; set; }

    [Option("config", EnvironmentVariable = "APP_CONFIG", HelpText = "Configuration file path")]
    public string ConfigFile { get; set; }
}

class Program
{
    static int Main(string[] args)
    {
        // Show help if requested
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
        {
            Console.WriteLine("Usage: myapp <command> <input> [options]");
            Console.WriteLine();
            Console.WriteLine(Parser.GetHelpText(new Options()));
            return 0;
        }

        // Parse arguments
        var result = Parser.Default.Parse<Options>(args);

        if (result.Result != ParserResultType.Parsed)
        {
            Console.Error.WriteLine("Parsing failed:");
            
            // Show structured errors
            foreach (var error in result.Errors)
            {
                Console.Error.WriteLine($"  [{error.ErrorType}] {error.OptionName}: {error.Message}");
            }
            
            Console.WriteLine();
            Console.WriteLine("Use --help for usage information.");
            return 1;
        }

        var options = result.Value;

        // Display parsed options
        if (options.Verbose)
        {
            Console.WriteLine("Parsed options:");
            Console.WriteLine($"  Command: {options.Command}");
            Console.WriteLine($"  Input: {options.InputFile}");
            Console.WriteLine($"  Output: {options.OutputFile ?? "(stdout)"}");
            Console.WriteLine($"  Format: {options.Format}");
            Console.WriteLine($"  Flags: {options.Flags}");
            Console.WriteLine($"  Threads: {options.ThreadCount}");
            Console.WriteLine($"  API Key: {(string.IsNullOrEmpty(options.ApiKey) ? "(not set)" : "****")}");
            Console.WriteLine($"  Config: {options.ConfigFile ?? "(not set)"}");
            
            if (options.Tags?.Count > 0)
                Console.WriteLine($"  Tags: {string.Join(", ", options.Tags)}");
            
            if (options.IncludeFiles?.Count > 0)
                Console.WriteLine($"  Includes: {string.Join(", ", options.IncludeFiles)}");
        }

        // Format back to command line for logging (exclude sensitive data)
        Console.WriteLine($"Effective command: {Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify | CommandLineFormatMethod.EqualSignStyle)}");

        // Your application logic here...
        
        return 0;
    }
}
```

Run examples:

```bash
# Using positional arguments
myapp process data.txt -o result.json -f Json -v

# Positional args with named options mixed
myapp --verbose process data.txt --format=Xml --threads=8

# With custom separator for tags
myapp convert input.csv --tags=important;reviewed;final -o output.json

# With flags and includes
myapp analyze report.txt --flags Validate Transform --include extra1.txt extra2.txt

# Using equals syntax throughout
myapp process data.txt -o=result.json --format=Json --threads=4 --tags=dev;test

# With environment variables (set APP_API_KEY before running)
export APP_API_KEY="secret-key-123"
myapp process data.txt -v
# API key is automatically loaded from environment
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📬 Contact

- **Author**: bodong
- **GitHub**: [https://github.com/bodong1987/MiniCommandLineParser](https://github.com/bodong1987/MiniCommandLineParser)

---

Made with ❤️ for the .NET community
