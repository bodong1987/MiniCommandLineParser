# MiniCommandLineParser

A **simple**, **lightweight**, and **dependency-free** command-line parsing library for .NET.

## ✨ Features

- 🪶 **Lightweight** - Minimal footprint, no external dependencies
- 🎯 **Simple API** - Intuitive attribute-based configuration
- 📦 **Multi-target** - Supports .NET 6/7/8/9 and .NET Standard 2.1
- 🔄 **Bidirectional** - Parse arguments to objects AND format objects back to command-line strings
- 📝 **Auto Help Text** - Built-in help text generation
- 🔧 **Flexible** - Supports short/long options, arrays, enums, flags, and more

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
}
```

## 📖 Supported Argument Formats

```bash
# Short options
-v -i input.txt

# Long options
--verbose --input input.txt

# Equals syntax
--input=input.txt

# Quoted values (with spaces)
--output "my output file.txt"

# Array values
--tags tag1 tag2 tag3

# Flags enum
--flags Flag1 Flag2 Flag3
```

## 🔄 Format Object to Command Line

```csharp
var options = new Options { InputFile = "test.txt", Verbose = true };

// Full output (all options)
string cmdLine = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --input test.txt --verbose True --count 1

// Simplified output (only non-default values)
string simplified = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);
// Output: --input test.txt --verbose True
```

## 📝 Auto-Generate Help Text

```csharp
var helpText = Parser.GetHelpText(new Options());
Console.WriteLine(helpText);
```

## ⚙️ Parser Settings

```csharp
var parser = new Parser(new ParserSettings
{
    CaseSensitive = false,          // Case-insensitive matching (default)
    IgnoreUnknownArguments = true   // Ignore unknown arguments (default)
});
```

## 📄 License

MIT License - see [GitHub Repository](https://github.com/bodong1987/MiniCommandLineParser) for details.