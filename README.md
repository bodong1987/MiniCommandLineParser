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
| **Learning Curve** | ✅ Simple, intuitive API | ❌ Complex configurations |
| **Multi-target** | ✅ .NET 6/7/8/9 + Standard 2.1 | ⚠️ Varies |

## ✨ Features

- 🪶 **Lightweight** - Minimal footprint with zero external dependencies
- 🎯 **Simple API** - Intuitive attribute-based configuration
- 📦 **Multi-target** - Supports .NET 6.0, 7.0, 8.0, 9.0 and .NET Standard 2.1
- 🔄 **Bidirectional** - Parse arguments to objects AND format objects back to command-line strings
- 📝 **Auto Help Text** - Built-in help text generation with customizable formatting
- 🔧 **Flexible** - Supports short/long options, arrays, enums, flags, and more
- 🧵 **Thread-Safe** - Type information caching is thread-safe for concurrent usage
- ⚡ **High Performance** - Efficient parsing with minimal allocations

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

# Boolean flags (presence means true)
--verbose        # Sets Verbose = true
--verbose=true   # Also sets Verbose = true
--verbose=false  # Sets Verbose = false
```

### Array/List Support

Support for collection types like `List<T>`:

```csharp
public class Options
{
    [Option("files", HelpText = "Input files to process")]
    public List<string> Files { get; set; }

    [Option("numbers", HelpText = "Numbers to sum")]
    public List<int> Numbers { get; set; }
}
```

Usage:

```bash
myapp --files file1.txt file2.txt file3.txt --numbers 1 2 3 4 5
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

### Supported Data Types

| Type | Example | Notes |
|------|---------|-------|
| `string` | `--name "John Doe"` | Supports quoted values |
| `int`, `long`, `short` | `--count 42` | All integer types |
| `float`, `double`, `decimal` | `--rate 3.14` | Floating-point types |
| `bool` | `--verbose` or `--flag=true` | Flag presence = true |
| `enum` | `--level Info` | Case-insensitive by default |
| `[Flags] enum` | `--flags A B C` | Multiple space-separated values |
| `List<T>` | `--items a b c` | Any supported element type |

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

// Complete format - includes all options
string complete = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --input data.txt --output result.txt --verbose True --count 5

// Simplified format - only non-default values
string simplified = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);
// Output: --input data.txt --output result.txt --verbose True --count 5

// Get as string array
string[] args = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Simplify);
```

### Format Methods

| Method | Description |
|--------|-------------|
| `Complete` | Outputs all options regardless of default values |
| `Simplify` | Outputs only options that differ from defaults |

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
    -i, --input                                 [Required] Input file path
    -o, --output                                [Optional] Output file path
    -v, --verbose                               [Optional] Enable verbose output
    --count                                     [Optional] Number of iterations
    --level                                     [Optional,Enum] Log level
                                                --level Debug Info Warning Error
    --features                                  [Optional,Flags] Enabled features
                                                --features Logging Caching Compression
```

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

## 🔍 API Reference

### Parser Class

| Method | Description |
|--------|-------------|
| `Parse<T>(string arguments)` | Parse a command-line string |
| `Parse<T>(IEnumerable<string> arguments)` | Parse an array of arguments |
| `Parse<T>(string arguments, T value)` | Parse into an existing instance |
| `FormatCommandLine(object, CommandLineFormatMethod)` | Convert object to command-line string |
| `FormatCommandLineArgs(object, CommandLineFormatMethod)` | Convert object to string array |
| `GetHelpText(object, IFormatter?)` | Generate help text |
| `GetTypeInfo<T>()` | Get cached type metadata |

### Static Members

| Member | Description |
|--------|-------------|
| `Parser.Default` | Default parser instance with default settings |

### ParserResult<T>

| Property | Description |
|----------|-------------|
| `Result` | `Parsed` or `NotParsed` |
| `Value` | The parsed options object |
| `ErrorMessage` | Error details if parsing failed |
| `Type` | Type metadata information |

### OptionAttribute

| Property | Description |
|----------|-------------|
| `ShortName` | Single character option (e.g., 'v' for `-v`) |
| `LongName` | Full option name (e.g., "verbose" for `--verbose`) |
| `Required` | Whether the option must be provided |
| `HelpText` | Description shown in help output |

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
    [Option('i', "input", Required = true, HelpText = "Input file path")]
    public string InputFile { get; set; }

    [Option('o', "output", HelpText = "Output file path")]
    public string OutputFile { get; set; }

    [Option('f', "format", HelpText = "Output format")]
    public OutputFormat Format { get; set; } = OutputFormat.Text;

    [Option("flags", HelpText = "Processing flags")]
    public ProcessingFlags Flags { get; set; } = ProcessingFlags.Validate;

    [Option("include", HelpText = "Files to include")]
    public List<string> IncludeFiles { get; set; }

    [Option('v', "verbose", HelpText = "Enable verbose logging")]
    public bool Verbose { get; set; }

    [Option("threads", HelpText = "Number of worker threads")]
    public int ThreadCount { get; set; } = Environment.ProcessorCount;
}

class Program
{
    static int Main(string[] args)
    {
        // Show help if requested
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
        {
            Console.WriteLine("Usage: myapp [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine(Parser.GetHelpText(new Options()));
            return 0;
        }

        // Parse arguments
        var result = Parser.Default.Parse<Options>(args);

        if (result.Result != ParserResultType.Parsed)
        {
            Console.Error.WriteLine($"Error: {result.ErrorMessage}");
            return 1;
        }

        var options = result.Value;

        // Display parsed options
        if (options.Verbose)
        {
            Console.WriteLine("Parsed options:");
            Console.WriteLine($"  Input: {options.InputFile}");
            Console.WriteLine($"  Output: {options.OutputFile ?? "(stdout)"}");
            Console.WriteLine($"  Format: {options.Format}");
            Console.WriteLine($"  Flags: {options.Flags}");
            Console.WriteLine($"  Threads: {options.ThreadCount}");
            
            if (options.IncludeFiles?.Count > 0)
                Console.WriteLine($"  Includes: {string.Join(", ", options.IncludeFiles)}");
        }

        // Your application logic here...
        
        return 0;
    }
}
```

Run examples:

```bash
# Basic usage
myapp -i data.txt -o result.json -f Json -v

# With flags and includes
myapp --input data.txt --flags Validate Transform --include extra1.txt extra2.txt

# Using equals syntax
myapp -i=data.txt --format=Xml --threads=8
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