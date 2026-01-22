# MiniCommandLineParser

A **simple**, **lightweight**, and **dependency-free** command-line parsing library for .NET.

## ✨ Features

- 🪶 **Lightweight** - Minimal footprint, no external dependencies
- 🎯 **Simple API** - Intuitive attribute-based configuration
- 📦 **Multi-target** - Supports .NET 6/7/8/9 and .NET Standard 2.1
- 🔄 **Bidirectional** - Parse arguments to objects AND format objects back to command-line strings
- 📝 **Auto Help Text** - Built-in help text generation
- 🔧 **Flexible** - Supports short/long options, arrays, enums, flags, and more
- 📍 **Positional Arguments** - Support index-based positional parameters (e.g., `app clone http://...`)
- ✂️ **Custom Separators** - Split array values with custom separators (e.g., `--tags=a;b;c`)

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
    Tags = ["dev", "test", "prod"],
    Ids = [1, 2, 3]
};

// Space-separated style (Complete/Simplify without EqualSignStyle)
Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);
// Output: --tags dev test prod --ids 1 2 3

// Equal sign style - arrays use separator automatically
Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);
// Output: --tags=dev;test;prod --ids=1,2,3
```

### Format Method Flags

| Flag | Description |
|------|-------------|
| `None` | Default space-separated style |
| `Complete` | Output all options including defaults |
| `Simplify` | Only output non-default values |
| `EqualSignStyle` | Use `--option=value` syntax, arrays use separator |

Flags can be combined using `|` operator for flexible output formatting.

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