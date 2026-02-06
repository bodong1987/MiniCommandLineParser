# MiniCommandLineParser

A **simple**, **lightweight**, and **dependency-free** command-line parsing library for .NET.

## ✨ Features

- 🪶 **Lightweight** - Minimal footprint, no external dependencies
- 🎯 **Simple API** - Intuitive attribute-based configuration
- 📦 **Multi-target** - Supports .NET 6/7/8/9 and .NET Standard 2.1
- 🔄 **Bidirectional** - Parse arguments to objects AND format objects back to command-line strings
- 📝 **Auto Help Text** - Built-in help text generation with default value display
- 🔧 **Flexible** - Supports short/long options, arrays, enums, flags, and more
- 📍 **Positional Arguments** - Support index-based positional parameters (e.g., `app clone http://...`)
- ✂️ **Custom Separators** - Split array values with custom separators (e.g., `--tags=a;b;c`)
- 🌍 **Environment Variables** - Fallback to environment variables when options not provided
- ⚠️ **Structured Errors** - Typed error handling with `ParseError` and `ParseErrorType`
- ⚡ **High Performance** - Thread-safe TypeInfo caching for efficient parsing

## 📥 Installation

```bash
dotnet add package MiniCommandLineParser
```

## 🚀 Quick Start

### 1. Define Your Options Class

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

    [Option("tags", HelpText = "List of tags")]
    public List<string> Tags { get; set; }
}
```

### 2. Parse Command-Line Arguments

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result == ParserResultType.Parsed)
{
    var options = result.Value;
    Console.WriteLine($"Input: {options.InputFile}");
}
else
{
    Console.WriteLine(result.ErrorMessage);
    
    // Access structured errors
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"[{error.ErrorType}] {error.OptionName}: {error.Message}");
    }
}
```

## 🌍 Environment Variable Support

Fallback to environment variables when command-line options are not provided:

```csharp
public class Options
{
    [Option('c', "config", EnvironmentVariable = "APP_CONFIG", HelpText = "Config file path")]
    public string ConfigFile { get; set; }

    [Option("api-key", EnvironmentVariable = "API_KEY", HelpText = "API key for authentication")]
    public string ApiKey { get; set; }

    [Option("timeout", EnvironmentVariable = "APP_TIMEOUT", HelpText = "Timeout in seconds")]
    public int Timeout { get; set; } = 30;
}

// Command line takes precedence over environment variables
// If --config is not provided, APP_CONFIG environment variable is used
// If neither is set, property keeps its default value
```

**Priority order**: Command-line argument > Environment variable > Default value

## 📍 Positional Arguments

Support CLI-style positional parameters without option names:

```csharp
public class CloneOptions
{
    [Option("command", Index = 0, HelpText = "Command name")]
    public string Command { get; set; }

    [Option("url", Index = 1, HelpText = "Repository URL")]
    public string Url { get; set; }

    [Option('v', "verbose", HelpText = "Verbose output")]
    public bool Verbose { get; set; }
}

// Parse: myapp clone https://github.com/user/repo --verbose
// Result: Command="clone", Url="https://github.com/user/repo", Verbose=true
```

## ✂️ Custom Array Separators

Split array values using custom separators:

```csharp
public class BuildOptions
{
    // Default separator is ';'
    [Option("tags", Separator = ';', HelpText = "Tags separated by semicolon")]
    public List<string> Tags { get; set; }

    // Custom separator
    [Option("ids", Separator = ',', HelpText = "IDs separated by comma")]
    public int[] Ids { get; set; }
}

// Parse: --tags=dev;test;prod --ids=1,2,3
// Result: Tags=["dev","test","prod"], Ids=[1,2,3]
```

## ⚠️ Structured Error Handling

Access detailed error information with typed errors:

```csharp
var result = Parser.Default.Parse<Options>(args);

if (result.Result == ParserResultType.NotParsed)
{
    foreach (var error in result.Errors)
    {
        switch (error.ErrorType)
        {
            case ParseErrorType.MissingRequired:
                Console.WriteLine($"Missing required option: {error.OptionName}");
                break;
            case ParseErrorType.InvalidValue:
                Console.WriteLine($"Invalid value for {error.OptionName}: {error.Message}");
                break;
            case ParseErrorType.UnknownOption:
                Console.WriteLine($"Unknown option: {error.OptionName}");
                break;
        }
    }
}
```

**Error Types**:
| Type | Description |
|------|-------------|
| `MissingRequired` | A required option was not provided |
| `InvalidValue` | The value could not be converted to the expected type |
| `UnknownOption` | An unrecognized option was provided |

## 📖 Supported Argument Formats

```bash
# Short options
-v -i input.txt

# Long options
--verbose --input input.txt

# Equals syntax
--input=input.txt --config="key=value"

# Boolean options (all equivalent)
--verbose
--verbose=true
--verbose true

# Quoted values (with spaces)
--output "my output file.txt"

# Array values (space-separated)
--tags tag1 tag2 tag3

# Array values (with separator)
--tags=tag1;tag2;tag3

# Positional arguments
clone https://example.com --verbose

# Flags enum
--flags Flag1 Flag2 Flag3
```

## 🔄 Format Object to Command Line

Convert objects back to command-line strings with flexible formatting options:

```csharp
var options = new Options { InputFile = "test.txt", Verbose = true };

// Full output (all options with space syntax)
string cmdLine = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --input test.txt --verbose True --count 1

// Simplified output (only non-default values)
string simplified = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);
// Output: --input test.txt --verbose True

// Equal sign style (use = between option and value)
string equalStyle = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);
// Output: --input=test.txt --verbose=True --count=1

// Combine flags: Simplify + EqualSignStyle
string combined = Parser.FormatCommandLine(options, 
    CommandLineFormatMethod.Simplify | CommandLineFormatMethod.EqualSignStyle);
// Output: --input=test.txt --verbose=True
```

### Format Method Flags

| Flag | Description |
|------|-------------|
| `None` | Default space-separated style |
| `Complete` | Output all options including defaults |
| `Simplify` | Only output non-default values |
| `EqualSignStyle` | Use `--option=value` syntax, arrays use separator |

## 📝 Auto-Generate Help Text

```csharp
var helpText = Parser.GetHelpText(new Options());
Console.WriteLine(helpText);
```

Help text automatically shows:
- Option names (short and long)
- Required/Optional status
- Default values
- Environment variable names
- Type information (Array, Enum, Flags)

## ⚙️ Parser Settings

```csharp
var parser = new Parser(new ParserSettings
{
    CaseSensitive = false,          // Case-insensitive matching (default)
    IgnoreUnknownArguments = true   // Ignore unknown arguments (default)
});
```

## ⚡ Performance

- **TypeInfo Caching**: Type metadata is cached and reused across parse calls
- **Thread-Safe**: Safe for concurrent usage in multi-threaded applications
- **Minimal Allocations**: Optimized for low memory overhead

```csharp
// TypeInfo is cached - subsequent calls are fast
var typeInfo = Parser.GetTypeInfo<Options>();
```

## 📄 License

MIT License - see [GitHub Repository](https://github.com/bodong1987/MiniCommandLineParser) for details.
