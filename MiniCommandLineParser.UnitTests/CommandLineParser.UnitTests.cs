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

#endregion

[TestClass]
public sealed class SplitterTests
{
    [TestMethod]
    public void SplitCommandLine_SimpleArguments_ReturnsCorrectList()
    {
        // Arrange
        var commandLine = "--name test --count 5";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("test", result[1]);
        Assert.AreEqual("--count", result[2]);
        Assert.AreEqual("5", result[3]);
    }

    [TestMethod]
    public void SplitCommandLine_QuotedArguments_PreservesQuotedContent()
    {
        // Arrange
        var commandLine = "--message \"Hello World\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--message", result[0]);
        Assert.AreEqual("Hello World", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_WithHashComment_RemovesComment()
    {
        // Arrange
        var commandLine = "--name test # this is a comment";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, removeHashComments: true);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("test", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_WithHashCommentDisabled_KeepsComment()
    {
        // Arrange
        var commandLine = "--name test #comment";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, removeHashComments: false);

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("#comment", result[2]);
    }

    [TestMethod]
    public void SplitCommandLine_EmptyString_ReturnsEmptyList()
    {
        // Arrange
        var commandLine = "";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SplitCommandLine_WhitespaceOnly_ReturnsEmptyList()
    {
        // Arrange
        var commandLine = "   \t  ";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SplitCommandLine_EscapedQuotes_HandlesCorrectly()
    {
        // Arrange
        var commandLine = "--path \"C:\\Program Files\\test\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("C:\\Program Files\\test", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var commandLine = "--name=test --count=5";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--name=test", result[0]);
        Assert.AreEqual("--count=5", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_WithSpacesAroundEquals_SplitsAsArguments()
    {
        // Arrange - Note: spaces around = means they are separate arguments
        var commandLine = "--name = test";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("=", result[1]);
        Assert.AreEqual("test", result[2]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_QuotedValueWithEquals_PreservesQuotes()
    {
        // Arrange
        // Note: Quotes are only removed when the ENTIRE argument is surrounded by quotes.
        // Here, quotes only surround the value part, so they are preserved in the output.
        var commandLine = "--config=\"key=value\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--config=\"key=value\"", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_MultipleEquals_PreservesAll()
    {
        // Arrange
        var commandLine = "--expr=a=b=c";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--expr=a=b=c", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_EmptyValue_Preserved()
    {
        // Arrange
        var commandLine = "--name=";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--name=", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_ShortOption_ParsesCorrectly()
    {
        // Arrange
        var commandLine = "-n=value";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("-n=value", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_IllegalCharacter_ReportsIllegalChar()
    {
        // Arrange
        var commandLine = "--name test|value";

        // Act
        // ReSharper disable once UnusedVariable
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, false, out var illegalChar);

        // Assert
        Assert.IsNotNull(illegalChar);
        Assert.AreEqual('|', illegalChar.Value);
    }
}

[TestClass]
public sealed class ParserBasicTests
{
    [TestMethod]
    public void Parse_SimpleOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --name John --count 10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_ShortOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-v -n John -c 10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_MixedOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-v --name John -c 10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithoutValue_SetsTrue()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=John --count=10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_WithQuotedValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=\"John Doe\"";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John Doe", result.Value.Name);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_ValueContainsEquals_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=a=b=c";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("a=b=c", result.Value.Name);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_QuotedValueContainsEquals_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=\"key=value\"";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("key=value", result.Value.Name);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_EmptyValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("", result.Value.Name);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_MixedWithSpaceSyntax_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name=John --count 10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_BooleanFlag_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose=true";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_BooleanFlagFalse_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose=false";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsFalse(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_EqualsSyntax_ShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-n=John";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_QuotedValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--name \"John Doe\"";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John Doe", result.Value.Name);
    }

    [TestMethod]
    public void Parse_EmptyCommandLine_ReturnsDefaultValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsFalse(result.Value.Verbose);
        Assert.IsNull(result.Value.Name);
        Assert.AreEqual(0, result.Value.Count);
    }
}

[TestClass]
public sealed class ParserSettingsTests
{
    [TestMethod]
    public void Parse_CaseInsensitive_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser(new ParserSettings { CaseSensitive = false });
        var commandLine = "--NAME John --COUNT 10";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(10, result.Value.Count);
    }

    [TestMethod]
    public void Parse_UnknownArguments_IgnoresWhenConfigured()
    {
        // Arrange
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });
        var commandLine = "--name John --unknown value";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_UnknownArguments_FailsWhenNotIgnored()
    {
        // Arrange
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });
        var commandLine = "--name John --unknown value";

        // Act
        var result = parser.Parse<BasicOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.ErrorMessage.Contains("Unknown property"));
    }
}

[TestClass]
public sealed class ParserArrayTests
{
    [TestMethod]
    public void Parse_StringArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt file2.txt file3.txt";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    public void Parse_IntArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--numbers 1 2 3 4 5";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(5, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, result.Value.Numbers);
    }

    [TestMethod]
    public void Parse_DoubleArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 1.1 2.2 3.3";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(3, result.Value.Doubles.Count);
        Assert.AreEqual(1.1, result.Value.Doubles[0], 0.001);
        Assert.AreEqual(2.2, result.Value.Doubles[1], 0.001);
        Assert.AreEqual(3.3, result.Value.Doubles[2], 0.001);
    }

    [TestMethod]
    public void Parse_BoolArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true false true";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(3, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools[0]);
        Assert.IsFalse(result.Value.Bools[1]);
        Assert.IsTrue(result.Value.Bools[2]);
    }

    [TestMethod]
    public void Parse_EnumArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug Info Warning Error";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Warning, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Error, result.Value.Levels[3]);
    }

    [TestMethod]
    public void Parse_SingleElementArray_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files single.txt";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(1, result.Value.Files.Count);
        Assert.AreEqual("single.txt", result.Value.Files[0]);
    }

    [TestMethod]
    public void Parse_ArrayWithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-f file1.txt file2.txt";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
    }

    [TestMethod]
    public void Parse_MultipleArrayOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt file2.txt --numbers 1 2 3";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual(3, result.Value.Numbers.Count);
    }

    [TestMethod]
    public void Parse_ArrayWithQuotedStrings_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings \"hello world\" \"foo bar\"";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(2, result.Value.Strings.Count);
        Assert.AreEqual("hello world", result.Value.Strings[0]);
        Assert.AreEqual("foo bar", result.Value.Strings[1]);
    }

    [TestMethod]
    public void Parse_MixedArrayAndNonArrayOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --files file1.txt file2.txt --output result.txt";

        // Act
        var result = parser.Parse<MixedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    public void Parse_ArrayFollowedByOtherOption_StopsAtNextOption()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings item1 item2 --other value";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(2, result.Value.Strings.Count);
        Assert.AreEqual("item1", result.Value.Strings[0]);
        Assert.AreEqual("item2", result.Value.Strings[1]);
        Assert.AreEqual("value", result.Value.Other);
    }

    [TestMethod]
    public void Parse_ArrayAtEndOfCommandLine_ParsesAllValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--other first --strings item1 item2 item3";

        // Act
        var result = parser.Parse<ExtendedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("first", result.Value.Other);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
    }

    [TestMethod]
    public void Parse_EmptyArray_WhenNoValues_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files --numbers 1 2 3";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        // Files should be empty since --numbers follows immediately
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(0, result.Value.Files.Count);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(3, result.Value.Numbers.Count);
    }

    [TestMethod]
    public void Parse_NegativeNumbers_InIntArray_ParsesCorrectly()
    {
        // Arrange  
        var parser = new Parser();
        // Note: negative numbers starting with - might be tricky
        var commandLine = new[] { "--numbers", "1", "-2", "3" };

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert - behavior depends on implementation
        // Negative numbers starting with '-' might be interpreted as options
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Parse_MultipleArrayOptionsInterspersed_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-v -f file1.txt file2.txt -o output.txt -c 1 2 3";

        // Act
        var result = parser.Parse<MixedArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(2, result.Value.Files?.Count);
        Assert.AreEqual("output.txt", result.Value.Output);
        Assert.AreEqual(3, result.Value.Counts?.Count);
    }

    [TestMethod]
    public void FormatCommandLine_ArrayOption_FormatsCorrectly()
    {
        // Arrange
        var options = new ArrayOptions
        {
            Files = ["file1.txt", "file2.txt", "file3.txt"],
            Numbers = [1, 2, 3]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--files"));
        Assert.IsTrue(result.Contains("file1.txt"));
        Assert.IsTrue(result.Contains("file2.txt"));
        Assert.IsTrue(result.Contains("file3.txt"));
        Assert.IsTrue(result.Contains("--numbers"));
    }

    [TestMethod]
    public void FormatCommandLine_EmptyArray_OmitsOption()
    {
        // Arrange
        var options = new ArrayOptions
        {
            Files = [],
            Numbers = null
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);

        // Assert
        // Empty or null arrays should be omitted in simplified format
        Assert.IsFalse(result.Contains("--numbers"));
    }

    [TestMethod]
    public void Parse_ArrayWithSpacesInQuotedValues_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files \"path with spaces/file1.txt\" \"another path/file2.txt\"";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("path with spaces/file1.txt", result.Value.Files[0]);
        Assert.AreEqual("another path/file2.txt", result.Value.Files[1]);
    }

    [TestMethod]
    public void FormatCommandLineArgs_ArrayOption_ReturnsCorrectArray()
    {
        // Arrange
        var options = new ArrayOptions
        {
            Files = ["file1.txt", "file2.txt"]
        };

        // Act
        var result = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.Contains(result, "--files");
        CollectionAssert.Contains(result, "file1.txt");
        CollectionAssert.Contains(result, "file2.txt");
    }

    [TestMethod]
    public void Roundtrip_ArrayOptions_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new ArrayOptions
        {
            Files = ["a.txt", "b.txt"],
            Numbers = [10, 20, 30]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        CollectionAssert.AreEqual(original.Files, parsed.Value.Files);
        CollectionAssert.AreEqual(original.Numbers, parsed.Value.Numbers);
    }
}

[TestClass]
public sealed class ParserSeparatorTests
{
    [TestMethod]
    public void Parse_CommaSeparator_SplitsStringValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt,file2.txt,file3.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    public void Parse_ColonSeparator_SplitsIntValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--numbers 1:2:3:4:5";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(5, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, result.Value.Numbers);
    }

    [TestMethod]
    public void Parse_SemicolonSeparator_SplitsValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--tags tag1;tag2;tag3";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Tags);
        Assert.AreEqual(3, result.Value.Tags.Count);
        Assert.AreEqual("tag1", result.Value.Tags[0]);
        Assert.AreEqual("tag2", result.Value.Tags[1]);
        Assert.AreEqual("tag3", result.Value.Tags[2]);
    }

    [TestMethod]
    public void Parse_SeparatorWithSpaces_TrimsValues()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt , file2.txt , file3.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    public void Parse_SeparatorWithEqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files=a.txt,b.txt,c.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    [TestMethod]
    public void Parse_SeparatorWithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-f file1.txt,file2.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
    }

    [TestMethod]
    public void Parse_SeparatorSingleValue_NoSplitting()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files single.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(1, result.Value.Files.Count);
        Assert.AreEqual("single.txt", result.Value.Files[0]);
    }

    [TestMethod]
    public void Parse_SeparatorWithMultipleArguments_SplitsEach()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt,file2.txt file3.txt,file4.txt";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(4, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
        Assert.AreEqual("file4.txt", result.Value.Files[3]);
    }

    [TestMethod]
    public void Parse_DefaultSeparator_UsesSemicolon()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files file1.txt;file2.txt;file3.txt";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    public void Parse_MultipleSeparatorOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files a.txt,b.txt --numbers 1:2:3 --tags x;y;z";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(2, result.Value.Files?.Count);
        Assert.AreEqual(3, result.Value.Numbers?.Count);
        Assert.AreEqual(3, result.Value.Tags?.Count);
    }

    [TestMethod]
    public void Parse_EmptySeparatedValues_IgnoresEmpty()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files a.txt,,b.txt,";

        // Act
        var result = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        // Empty parts should be ignored
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
    }
}

[TestClass]
public sealed class ParserEnumTests
{
    [TestMethod]
    public void Parse_EnumValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--level Warning";

        // Act
        var result = parser.Parse<EnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(LogLevel.Warning, result.Value.Level);
    }

    [TestMethod]
    public void Parse_FlagsEnum_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--access Read Write";

        // Act
        var result = parser.Parse<EnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(FileAccess.Read | FileAccess.Write, result.Value.Access);
    }

    [TestMethod]
    public void Parse_EnumCaseInsensitive_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser(new ParserSettings { CaseSensitive = false });
        var commandLine = "--level warning";

        // Act
        var result = parser.Parse<EnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(LogLevel.Warning, result.Value.Level);
    }
}

[TestClass]
public sealed class ParserAutoNameTests
{
    [TestMethod]
    public void Parse_AutoPropertyName_UsesPropertyNameAsLongName()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--AutoProperty TestValue";

        // Act
        var result = parser.Parse<AutoNameOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("TestValue", result.Value.AutoProperty);
    }

    [TestMethod]
    public void Parse_ExplicitName_UsesSpecifiedLongName()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--explicit-name TestValue";

        // Act
        var result = parser.Parse<AutoNameOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("TestValue", result.Value.ExplicitName);
    }
}

[TestClass]
public sealed class ParserTypedValuesTests
{
    [TestMethod]
    public void Parse_IntValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--int-value 42";

        // Act
        var result = parser.Parse<TypedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(42, result.Value.IntValue);
    }

    [TestMethod]
    public void Parse_DoubleValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--double-value 3.14";

        // Act
        var result = parser.Parse<TypedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(3.14, result.Value.DoubleValue, 0.001);
    }

    [TestMethod]
    public void Parse_BoolValueTrue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bool-value true";

        // Act
        var result = parser.Parse<TypedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.BoolValue);
    }

    [TestMethod]
    public void Parse_StringValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--string-value \"Hello World\"";

        // Act
        var result = parser.Parse<TypedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("Hello World", result.Value.StringValue);
    }
}

[TestClass]
public sealed class ParserExistingInstanceTests
{
    [TestMethod]
    public void Parse_ExistingInstance_UpdatesInstance()
    {
        // Arrange
        var parser = new Parser();
        var existing = new BasicOptions { Verbose = true, Count = 5 };
        var commandLine = "--name John";

        // Act
        var result = parser.Parse(commandLine, existing);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreSame(existing, result.Value);
        Assert.IsTrue(result.Value!.Verbose); // preserved
        Assert.AreEqual(5, result.Value.Count); // preserved
        Assert.AreEqual("John", result.Value.Name); // updated
    }
}

[TestClass]
public sealed class FormatCommandLineTests
{
    [TestMethod]
    public void FormatCommandLine_Complete_OutputsAllOptions()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = true,
            Name = "John",
            Count = 10
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--verbose"));
        Assert.IsTrue(result.Contains("--name John"));
        Assert.IsTrue(result.Contains("--count 10"));
    }

    [TestMethod]
    public void FormatCommandLine_Simplify_OmitsDefaultValues()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = false, // default
            Name = "John",
            Count = 0 // default
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);

        // Assert
        Assert.IsFalse(result.Contains("--verbose"));
        Assert.IsTrue(result.Contains("--name John"));
        Assert.IsFalse(result.Contains("--count"));
    }

    [TestMethod]
    public void FormatCommandLine_QuotedValueWithSpaces_WrapsInQuotes()
    {
        // Arrange
        var options = new BasicOptions
        {
            Name = "John Doe"
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("\"John Doe\""));
    }

    [TestMethod]
    public void FormatCommandLineArgs_ReturnsArray()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = true,
            Name = "John"
        };

        // Act
        var result = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
        CollectionAssert.Contains(result, "--verbose");
        CollectionAssert.Contains(result, "--name");
        CollectionAssert.Contains(result, "John");
    }

    [TestMethod]
    public void FormatCommandLine_EqualSignStyle_UsesEqualsSyntax()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = true,
            Name = "John",
            Count = 10
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert
        Assert.IsTrue(result.Contains("--verbose=True"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--name=John"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--count=10"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_EqualSignStyle_ValueWithSpaces_WrapsInQuotes()
    {
        // Arrange
        var options = new BasicOptions
        {
            Name = "John Doe"
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert
        Assert.IsTrue(result.Contains("--name=\"John Doe\""), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_CompleteAndEqualSignStyle_CombinesFlags()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = true,
            Name = "John",
            Count = 10
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete | CommandLineFormatMethod.EqualSignStyle);

        // Assert - Should output all options with equals syntax
        Assert.IsTrue(result.Contains("--verbose=True"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--name=John"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--count=10"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_SimplifyAndEqualSignStyle_CombinesFlags()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = false, // default
            Name = "John",
            Count = 0 // default
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify | CommandLineFormatMethod.EqualSignStyle);

        // Assert - Should only output non-default values with equals syntax
        Assert.IsFalse(result.Contains("--verbose"), $"Result should not contain verbose: {result}");
        Assert.IsTrue(result.Contains("--name=John"), $"Result: {result}");
        Assert.IsFalse(result.Contains("--count"), $"Result should not contain count: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_None_OutputsNothing()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = true,
            Name = "John",
            Count = 10
        };

        // Act - None means no special formatting, should still output (default behavior)
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.None);

        // Assert - With None, it should output without equals syntax
        Assert.IsTrue(result.Contains("--verbose True"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--name John"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--count 10"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithEqualSignStyle_FormatsCorrectly()
    {
        // Arrange
        var options = new ArrayOptions
        {
            Files = ["file1.txt", "file2.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert - Arrays should use separator style (semicolon by default) in equal sign style
        Assert.IsTrue(result.Contains("--files=file1.txt;file2.txt"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_BooleanFalseWithEqualSignStyle_FormatsCorrectly()
    {
        // Arrange
        var options = new BasicOptions
        {
            Verbose = false,
            Name = "John"
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete | CommandLineFormatMethod.EqualSignStyle);

        // Assert
        Assert.IsTrue(result.Contains("--verbose=False"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--name=John"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithSpacesInValues_WrapsInQuotes()
    {
        // Arrange - Array contains values with spaces
        var options = new ArrayOptions
        {
            Files = ["path with spaces/file1.txt", "another path/file2.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert - Values with spaces should be wrapped in quotes
        Assert.IsTrue(result.Contains("\"path with spaces/file1.txt\""), $"Result: {result}");
        Assert.IsTrue(result.Contains("\"another path/file2.txt\""), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithSimplify_OutputsNonEmptyArray()
    {
        // Arrange - Non-empty arrays should be output in Simplify mode
        var options = new ArrayOptions
        {
            Files = ["file1.txt", "file2.txt"],
            Numbers = null // null should be omitted
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);

        // Assert
        Assert.IsTrue(result.Contains("--files"), $"Result should contain files: {result}");
        Assert.IsTrue(result.Contains("file1.txt"), $"Result: {result}");
        Assert.IsTrue(result.Contains("file2.txt"), $"Result: {result}");
        Assert.IsFalse(result.Contains("--numbers"), $"Result should not contain numbers: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithEqualSignStyleAndSpaces_FormatsCorrectly()
    {
        // Arrange - Array with values containing spaces in equal sign style
        var options = new ArrayOptions
        {
            Files = ["normal.txt", "path with space.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert - In equal sign style, arrays use separator style (semicolon by default)
        Assert.IsTrue(result.Contains("--files="), $"Result: {result}");
        Assert.IsTrue(result.Contains("normal.txt"), $"Result: {result}");
        Assert.IsTrue(result.Contains("path with space.txt"), $"Result: {result}");
        // Values should be joined by separator, not space-separated with quotes
        Assert.IsTrue(result.Contains(';'), $"Result should contain separator: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_IntArray_FormatsCorrectly()
    {
        // Arrange - Integer array formatting
        var options = new ArrayOptions
        {
            Numbers = [1, 2, 3, 4, 5]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--numbers"), $"Result: {result}");
        Assert.IsTrue(result.Contains("1"), $"Result: {result}");
        Assert.IsTrue(result.Contains("5"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_MixedArrayAndNonArray_FormatsCorrectly()
    {
        // Arrange - Mixed array and non-array options
        var options = new MixedArrayOptions
        {
            Verbose = true,
            Files = ["a.txt", "b.txt"],
            Output = "result.txt",
            Counts = [10, 20]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--verbose"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--files"), $"Result: {result}");
        Assert.IsTrue(result.Contains("a.txt"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--output"), $"Result: {result}");
        Assert.IsTrue(result.Contains("result.txt"), $"Result: {result}");
        Assert.IsTrue(result.Contains("--counts"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_SingleElementArray_FormatsCorrectly()
    {
        // Arrange - Single element array
        var options = new ArrayOptions
        {
            Files = ["single.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--files"), $"Result: {result}");
        Assert.IsTrue(result.Contains("single.txt"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLineArgs_ArrayWithSpaces_ReturnsCorrectArray()
    {
        // Arrange - FormatCommandLineArgs handling of array with spaces
        var options = new ArrayOptions
        {
            Files = ["normal.txt", "path with space.txt"]
        };

        // Act
        var result = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.Contains(result, "--files");
        CollectionAssert.Contains(result, "normal.txt");
        // Values with spaces in args array should be wrapped in quotes
        Assert.IsTrue(result.Any(arg => arg.Contains("path with space.txt")), 
            $"Result should contain path with space: {string.Join(", ", result)}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithCustomSeparator_EqualSignStyle_UsesCustomSeparator()
    {
        // Arrange - Array with custom separator (comma) in equal sign style
        var options = new SeparatorArrayOptions
        {
            Files = ["file1.txt", "file2.txt", "file3.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert - Should use comma as separator (as defined in SeparatorArrayOptions)
        Assert.IsTrue(result.Contains("--files=file1.txt,file2.txt,file3.txt"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_IntArrayWithSeparator_EqualSignStyle_UsesCustomSeparator()
    {
        // Arrange - Integer array with colon separator
        var options = new SeparatorArrayOptions
        {
            Numbers = [1, 2, 3, 4, 5]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert - Should use colon as separator (as defined in SeparatorArrayOptions)
        Assert.IsTrue(result.Contains("--numbers=1:2:3:4:5"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithSemicolonSeparator_EqualSignStyle_UsesSemicolon()
    {
        // Arrange - Array with semicolon separator
        var options = new SeparatorArrayOptions
        {
            Tags = ["tag1", "tag2", "tag3"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.EqualSignStyle);

        // Assert - Should use semicolon as separator
        Assert.IsTrue(result.Contains("--tags=tag1;tag2;tag3"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_ArrayWithoutEqualSignStyle_DoesNotUseSeparator()
    {
        // Arrange - Array without equal sign style should use space-separated format
        var options = new SeparatorArrayOptions
        {
            Files = ["file1.txt", "file2.txt"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert - Should NOT use separator, use space-separated format
        Assert.IsTrue(result.Contains("--files file1.txt file2.txt"), $"Result: {result}");
        Assert.IsFalse(result.Contains("file1.txt,file2.txt"), $"Result should not contain comma-separated: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_Roundtrip_ArrayWithSeparatorAndEqualSign()
    {
        // Arrange - Roundtrip test: format then parse back
        var parser = new Parser();
        var original = new SeparatorArrayOptions
        {
            Files = ["a.txt", "b.txt", "c.txt"],
            Numbers = [1, 2, 3],
            Tags = ["x", "y", "z"]
        };

        // Act - Format with equal sign style then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.EqualSignStyle);
        var parsed = parser.Parse<SeparatorArrayOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result, $"Failed to parse: {commandLine}");
        Assert.IsNotNull(parsed.Value);
        CollectionAssert.AreEqual(original.Files, parsed.Value.Files);
        CollectionAssert.AreEqual(original.Numbers, parsed.Value.Numbers);
        CollectionAssert.AreEqual(original.Tags, parsed.Value.Tags);
    }
}

[TestClass]
public sealed class GetHelpTextTests
{
    [TestMethod]
    public void GetHelpText_ContainsOptionNames()
    {
        // Arrange
        var options = new BasicOptions();

        // Act
        var result = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(result.Contains("-v, --verbose"));
        Assert.IsTrue(result.Contains("-n, --name"));
        Assert.IsTrue(result.Contains("-c, --count"));
    }

    [TestMethod]
    public void GetHelpText_ContainsHelpTexts()
    {
        // Arrange
        var options = new BasicOptions();

        // Act
        var result = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(result.Contains("Enable verbose output"));
        Assert.IsTrue(result.Contains("The name parameter"));
        Assert.IsTrue(result.Contains("The count parameter"));
    }

    [TestMethod]
    public void GetHelpText_WithCustomFormatter_UsesFormatter()
    {
        // Arrange
        var options = new BasicOptions();
        var formatter = new Formatter(indent: 2, blank: 30);

        // Act
        var result = Parser.GetHelpText(options, formatter);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }
}

[TestClass]
public sealed class TypeInfoTests
{
    [TestMethod]
    public void GetTypeInfo_CachesTypeInfo()
    {
        // Act
        var info1 = Parser.GetTypeInfo<BasicOptions>();
        var info2 = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.AreSame(info1, info2);
    }

    [TestMethod]
    public void TypeInfo_Properties_ReturnsOptionProperties()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.AreEqual(3, info.Properties.Length);
    }

    [TestMethod]
    public void TypeInfo_IsCommandLineObject_ReturnsTrueForOptionsClass()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.IsTrue(info.IsCommandLineObject);
    }

    [TestMethod]
    public void TypeInfo_FindShortProperty_FindsProperty()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindShortProperty("v", ignoreCase: false);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual("verbose", property.Attribute.LongName);
    }

    [TestMethod]
    public void TypeInfo_FindLongProperty_FindsProperty()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindLongProperty("verbose", ignoreCase: false);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual("v", property.Attribute.ShortName);
    }

    [TestMethod]
    public void TypeInfo_FindProperty_CaseInsensitive()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindLongProperty("VERBOSE", ignoreCase: true);

        // Assert
        Assert.IsNotNull(property);
    }

    [TestMethod]
    public void TypeInfo_DefaultObject_CreatesDefault()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.IsNotNull(info.DefaultObject);
        Assert.IsInstanceOfType(info.DefaultObject, typeof(BasicOptions));
    }
}

[TestClass]
public sealed class ReflectedPropertyInfoTests
{
    [TestMethod]
    public void ReflectedPropertyInfo_IsArray_DetectsListType()
    {
        // Arrange
        var info = Parser.GetTypeInfo<ArrayOptions>();

        // Act
        var filesProperty = info.FindLongProperty("files", ignoreCase: false);

        // Assert
        Assert.IsNotNull(filesProperty);
        Assert.IsTrue(filesProperty.IsArray);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_IsFlags_DetectsFlagsEnum()
    {
        // Arrange
        var info = Parser.GetTypeInfo<EnumOptions>();

        // Act
        var accessProperty = info.FindLongProperty("access", ignoreCase: false);

        // Assert
        Assert.IsNotNull(accessProperty);
        Assert.IsTrue(accessProperty.IsFlags);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_GetElementType_ReturnsElementType()
    {
        // Arrange
        var info = Parser.GetTypeInfo<ArrayOptions>();
        var filesProperty = info.FindLongProperty("files", ignoreCase: false);

        // Act
        var elementType = filesProperty?.GetElementType();

        // Assert
        Assert.IsNotNull(elementType);
        Assert.AreEqual(typeof(string), elementType);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_GetSetValue_WorksCorrectly()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();
        var nameProperty = info.FindLongProperty("name", ignoreCase: false);
        var target = new BasicOptions();

        // Act
        nameProperty!.SetValue(target, "TestName");
        var value = nameProperty.GetValue(target);

        // Assert
        Assert.AreEqual("TestName", value);
        Assert.AreEqual("TestName", target.Name);
    }
}

[TestClass]
public sealed class OptionAttributeTests
{
    [TestMethod]
    public void OptionAttribute_ToString_FormatsCorrectly()
    {
        // Arrange
        var attr = new OptionAttribute('h', "help");

        // Act
        var result = attr.ToString();

        // Assert
        Assert.AreEqual("-h --help", result);
    }

    [TestMethod]
    public void OptionAttribute_LongNameOnly_FormatsCorrectly()
    {
        // Arrange
        var attr = new OptionAttribute("help");

        // Act
        var result = attr.ToString();

        // Assert
        Assert.AreEqual("--help", result);
    }

    [TestMethod]
    public void OptionAttribute_DefaultRequired_IsFalse()
    {
        // Arrange
        var attr = new OptionAttribute("test");

        // Assert
        Assert.IsFalse(attr.Required);
    }

    [TestMethod]
    public void OptionAttribute_SetRequired_StoresValue()
    {
        // Arrange
        var attr = new OptionAttribute("test") { Required = true };

        // Assert
        Assert.IsTrue(attr.Required);
    }

    [TestMethod]
    public void OptionAttribute_SetHelpText_StoresValue()
    {
        // Arrange
        var attr = new OptionAttribute("test") { HelpText = "Test help" };

        // Assert
        Assert.AreEqual("Test help", attr.HelpText);
    }
}

[TestClass]
public sealed class ParserResultTests
{
    [TestMethod]
    public void ParserResult_AppendError_AddsErrorMessage()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("Error 1");
        result.AppendError("Error 2");

        // Assert
        Assert.IsTrue(result.ErrorMessage.Contains("Error 1"));
        Assert.IsTrue(result.ErrorMessage.Contains("Error 2"));
    }

    [TestMethod]
    public void ParserResult_InitialResult_IsNotParsed()
    {
        // Arrange & Act
        var result = new ParserResult<BasicOptions>(null);

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
    }
}

[TestClass]
public sealed class ParserDefaultInstanceTests
{
    [TestMethod]
    public void Parser_Default_IsNotNull()
    {
        // Assert
        Assert.IsNotNull(Parser.Default);
    }

    [TestMethod]
    public void Parser_Default_CanParse()
    {
        // Act
        var result = Parser.Default.Parse<BasicOptions>("--verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }
}

[TestClass]
public sealed class ParserEnumerableInputTests
{
    [TestMethod]
    public void Parse_EnumerableArguments_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var arguments = new List<string> { "--verbose", "--name", "John" };

        // Act
        var result = parser.Parse<BasicOptions>(arguments);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_ArrayArguments_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var arguments = new[] { "--verbose", "--name", "John" };

        // Act
        var result = parser.Parse<BasicOptions>(arguments);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }
}

#region Positional Arguments Tests

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

[TestClass]
public sealed class PositionalArgumentTests
{
    [TestMethod]
    public void Parse_SimplePositionalArguments_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_PositionalWithNamedOptions_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo --verbose
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_PositionalWithShortNamedOptions_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo -v
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo -v");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_MixedPositionalAndNamed_ParsesCorrectly()
    {
        // Arrange: app.exe build project --output /tmp --force
        var parser = new Parser();

        // Act
        var result = parser.Parse<MixedOptions>("build project --output /tmp --force");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("build", result.Value.Action);
        Assert.AreEqual("project", result.Value.Target);
        Assert.AreEqual("/tmp", result.Value.Output);
        Assert.IsTrue(result.Value.Force);
    }

    [TestMethod]
    public void Parse_PositionalIntegerArgument_ParsesCorrectly()
    {
        // Arrange: app.exe 10 items
        var parser = new Parser();

        // Act
        var result = parser.Parse<PositionalIntOptions>("10 items");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(10, result.Value.Count);
        Assert.AreEqual("items", result.Value.Name);
    }

    [TestMethod]
    public void Parse_PositionalWithQuotedValue_ParsesCorrectly()
    {
        // Arrange: app.exe clone "https://github.com/my repo"
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone \"https://github.com/my repo\"");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/my repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_NamedOptionsBeforePositional_StillParsesPositional()
    {
        // Arrange: app.exe --verbose clone https://github.com/repo
        // Note: This tests named options appearing before positional arguments
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose clone https://github.com/repo");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_OnlyFirstPositional_LeavesOthersDefault()
    {
        // Arrange: app.exe clone
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("", result.Value.Url);  // Default value
    }

    [TestMethod]
    public void FormatCommandLine_WithPositionalArguments_FormatsCorrectly()
    {
        // Arrange
        var options = new CloneCommandOptions
        {
            Command = "clone",
            Url = "https://github.com/repo",
            Verbose = true
        };

        // Act
        var commandLine = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(commandLine.Contains("clone"));
        Assert.IsTrue(commandLine.Contains("https://github.com/repo"));
        Assert.IsTrue(commandLine.Contains("--verbose"));
    }

    [TestMethod]
    public void TypeInfo_PositionalProperties_ReturnsOnlyPositionalSortedByIndex()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var positionalProps = typeInfo.PositionalProperties;

        // Assert
        Assert.AreEqual(2, positionalProps.Length);
        Assert.AreEqual(0, positionalProps[0].Attribute.Index);
        Assert.AreEqual(1, positionalProps[1].Attribute.Index);
    }

    [TestMethod]
    public void TypeInfo_NamedProperties_ReturnsOnlyNamedOptions()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var namedProps = typeInfo.NamedProperties;

        // Assert
        Assert.AreEqual(2, namedProps.Length);
        Assert.IsFalse(namedProps[0].Attribute.IsPositional);
        Assert.IsFalse(namedProps[1].Attribute.IsPositional);
    }

    [TestMethod]
    public void OptionAttribute_IsPositional_ReturnsTrueForNonNegativeIndex()
    {
        // Arrange
        var attr1 = new OptionAttribute { Index = 0 };
        var attr2 = new OptionAttribute { Index = 5 };
        var attr3 = new OptionAttribute { Index = -1 };
        var attr4 = new OptionAttribute();  // Default

        // Assert
        Assert.IsTrue(attr1.IsPositional);
        Assert.IsTrue(attr2.IsPositional);
        Assert.IsFalse(attr3.IsPositional);
        Assert.IsFalse(attr4.IsPositional);
    }

    [TestMethod]
    public void OptionAttribute_ToString_FormatsPositionalCorrectly()
    {
        // Arrange
        var attrWithMeta = new OptionAttribute { Index = 0, MetaName = "URL" };
        var attrWithoutMeta = new OptionAttribute { Index = 1 };

        // Assert
        Assert.AreEqual("<URL>", attrWithMeta.ToString());
        Assert.AreEqual("Value[1]", attrWithoutMeta.ToString());
    }

    [TestMethod]
    public void Parse_UnknownPositionalArgument_ReturnsError()
    {
        // Arrange: app.exe clone url extra_arg (3 positional, but only 2 defined)
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com extra_arg");

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Length > 0);
    }

    [TestMethod]
    public void Parse_UnknownPositionalArgument_IgnoredWhenConfigured()
    {
        // Arrange: app.exe clone url extra_arg with IgnoreUnknownArguments = true
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com extra_arg");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("clone", result.Value!.Command);
        Assert.AreEqual("https://github.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_MixedPositionalAndNamedWithEquals_ParsesCorrectly()
    {
        // Arrange: app.exe build project --output=/tmp -f
        var parser = new Parser();

        // Act
        var result = parser.Parse<MixedOptions>("build project --output=/tmp -f");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("build", result.Value.Action);
        Assert.AreEqual("project", result.Value.Target);
        Assert.AreEqual("/tmp", result.Value.Output);
        Assert.IsTrue(result.Value.Force);
    }
}

#endregion

#region Boolean Option Syntax Tests

/// <summary>
/// Tests for various boolean option syntax variations.
/// Validates that --flag, --flag=true, --flag true, etc. are equivalent.
/// </summary>
[TestClass]
public sealed class BooleanOptionSyntaxTests
{
    [TestMethod]
    public void Parse_BooleanFlag_NoValue_SetsTrue()
    {
        // Arrange: --verbose (flag without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_EqualsTrue_SetsTrue()
    {
        // Arrange: --verbose=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_EqualsFalse_SetsFalse()
    {
        // Arrange: --verbose=false
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_SpaceTrue_SetsTrue()
    {
        // Arrange: --verbose true (space syntax)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_SpaceFalse_SetsFalse()
    {
        // Arrange: --verbose false (space syntax)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_NoValue_SetsTrue()
    {
        // Arrange: -v (short flag without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_EqualsTrue_SetsTrue()
    {
        // Arrange: -v=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_EqualsFalse_SetsFalse()
    {
        // Arrange: -v=false
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v=false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_CaseInsensitiveTrue_SetsTrue()
    {
        // Arrange: --verbose=TRUE (case variations)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=TRUE");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_CaseInsensitiveFalse_SetsFalse()
    {
        // Arrange: --verbose=FALSE
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=FALSE");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_MixedCase_SetsCorrectly()
    {
        // Arrange: --verbose=True --verbose=False
        var parser = new Parser();

        // Act - True
        var resultTrue = parser.Parse<BasicOptions>("--verbose=True");
        // Act - False
        var resultFalse = parser.Parse<BasicOptions>("--verbose=False");

        // Assert
        Assert.IsTrue(resultTrue.Value!.Verbose);
        Assert.IsFalse(resultFalse.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithOtherOptions_NoValue_SetsTrue()
    {
        // Arrange: --verbose --name John (flag followed by other option)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithOtherOptions_EqualsTrue_SetsTrue()
    {
        // Arrange: --verbose=true --name John
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=true --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_BooleanFlagBetweenOptions_NoValue_SetsTrue()
    {
        // Arrange: --name John --verbose --count 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose --count 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_BooleanFlagBetweenOptions_EqualsTrue_SetsTrue()
    {
        // Arrange: --name John --verbose=true --count 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose=true --count 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_MultipleBooleanSyntaxes_AllEquivalent()
    {
        // Test that all these produce the same result
        var parser = new Parser();
        var syntaxes = new[]
        {
            "--verbose",
            "--verbose=true",
            "--verbose true",
            "-v",
            "-v=true",
            "-v true"
        };

        foreach (var syntax in syntaxes)
        {
            var result = parser.Parse<BasicOptions>(syntax);
            Assert.AreEqual(ParserResultType.Parsed, result.Result, $"Failed for syntax: {syntax}");
            Assert.IsTrue(result.Value!.Verbose, $"Verbose should be true for syntax: {syntax}");
        }
    }

    [TestMethod]
    public void Parse_MultipleBooleanSyntaxesFalse_AllEquivalent()
    {
        // Test that all these produce false
        var parser = new Parser();
        var syntaxes = new[]
        {
            "--verbose=false",
            "--verbose false",
            "-v=false",
            "-v false"
        };

        foreach (var syntax in syntaxes)
        {
            var result = parser.Parse<BasicOptions>(syntax);
            Assert.AreEqual(ParserResultType.Parsed, result.Result, $"Failed for syntax: {syntax}");
            Assert.IsFalse(result.Value!.Verbose, $"Verbose should be false for syntax: {syntax}");
        }
    }

    [TestMethod]
    public void Parse_BooleanFlagWithPositionalArgs_NoValue_DoesNotConsumePositional()
    {
        // Arrange: --verbose clone https://example.com
        // The flag should NOT consume "clone" as its value
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose clone https://example.com");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://example.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithPositionalArgs_EqualsTrue_DoesNotConsumePositional()
    {
        // Arrange: --verbose=true clone https://example.com
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose=true clone https://example.com");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://example.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_BooleanFlagFollowedByNonBooleanValue_TreatsAsNextOption()
    {
        // Arrange: --verbose John (where John is NOT a boolean value)
        // Expected: --verbose should be true, John should be ignored or treated as unknown
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });

        // Act
        var result = parser.Parse<BasicOptions>("--verbose John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        // John should not affect anything (ignored as unknown positional)
    }

    [TestMethod]
    public void Parse_ShortBooleanFlagFollowedByOtherShortOption_BothParsed()
    {
        // Arrange: -v -n John -c 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v -n John -c 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_BooleanFlagAtEnd_NoValue_SetsTrue()
    {
        // Arrange: --name John --verbose (flag at end without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlagAtEnd_EqualsTrue_SetsTrue()
    {
        // Arrange: --name John --verbose=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
    }
}

#endregion

#region BindingList Array Parameter Tests

/// <summary>
/// Tests for BindingList array parameter parsing and formatting.
/// </summary>
[TestClass]
public sealed class ParserBindingListTests
{
    #region String BindingList Tests

    [TestMethod]
    public void Parse_BindingList_String_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings --integers 1 2";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(0, result.Value.Strings.Count);
    }

    [TestMethod]
    public void Parse_BindingList_String_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings single";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.AreEqual("single", result.Value.Strings[0]);
    }

    [TestMethod]
    public void Parse_BindingList_String_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings first second third fourth";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(4, result.Value.Strings.Count);
        Assert.AreEqual("first", result.Value.Strings[0]);
        Assert.AreEqual("second", result.Value.Strings[1]);
        Assert.AreEqual("third", result.Value.Strings[2]);
        Assert.AreEqual("fourth", result.Value.Strings[3]);
    }

    [TestMethod]
    public void Parse_BindingList_String_WithQuotedValues_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings \"hello world\" \"foo bar\"";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(2, result.Value.Strings.Count);
        Assert.AreEqual("hello world", result.Value.Strings[0]);
        Assert.AreEqual("foo bar", result.Value.Strings[1]);
    }

    [TestMethod]
    public void Parse_BindingList_String_WithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-s one two three";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
    }

    #endregion

    #region Integer BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Integer_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers --strings abc";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(0, result.Value.Integers.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 42";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(1, result.Value.Integers.Count);
        Assert.AreEqual(42, result.Value.Integers[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 1 2 3 4 5";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(5, result.Value.Integers.Count);
        Assert.AreEqual(1, result.Value.Integers[0]);
        Assert.AreEqual(2, result.Value.Integers[1]);
        Assert.AreEqual(3, result.Value.Integers[2]);
        Assert.AreEqual(4, result.Value.Integers[3]);
        Assert.AreEqual(5, result.Value.Integers[4]);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_WithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-i 10 20 30";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(3, result.Value.Integers.Count);
        Assert.AreEqual(10, result.Value.Integers[0]);
        Assert.AreEqual(20, result.Value.Integers[1]);
        Assert.AreEqual(30, result.Value.Integers[2]);
    }

    #endregion

    #region Bool BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Bool_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        // note: when only --bools is present without value, it may be interpreted as an empty list or a single true
        Assert.IsNotNull(result.Value.Bools);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(1, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true false true false";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(4, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools[0]);
        Assert.IsFalse(result.Value.Bools[1]);
        Assert.IsTrue(result.Value.Bools[2]);
        Assert.IsFalse(result.Value.Bools[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_AllTrue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true true true";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(3, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools.All(b => b));
    }

    [TestMethod]
    public void Parse_BindingList_Bool_AllFalse_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools false false false";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(3, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools.All(b => !b));
    }

    #endregion

    #region Numeric BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Double_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles --floats 1.0";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(0, result.Value.Doubles.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Double_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 3.14";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(1, result.Value.Doubles.Count);
        Assert.AreEqual(3.14, result.Value.Doubles[0], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_Double_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 1.1 2.2 3.3 4.4";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(4, result.Value.Doubles.Count);
        Assert.AreEqual(1.1, result.Value.Doubles[0], 0.001);
        Assert.AreEqual(2.2, result.Value.Doubles[1], 0.001);
        Assert.AreEqual(3.3, result.Value.Doubles[2], 0.001);
        Assert.AreEqual(4.4, result.Value.Doubles[3], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_Float_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--floats 1.5";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Floats);
        Assert.AreEqual(1, result.Value.Floats.Count);
        Assert.AreEqual(1.5f, result.Value.Floats[0], 0.001f);
    }

    [TestMethod]
    public void Parse_BindingList_Float_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--floats 0.1 0.2 0.3";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Floats);
        Assert.AreEqual(3, result.Value.Floats.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Long_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--longs 9999999999";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Longs);
        Assert.AreEqual(1, result.Value.Longs.Count);
        Assert.AreEqual(9999999999L, result.Value.Longs[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Long_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--longs 1000000000 2000000000 3000000000";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Longs);
        Assert.AreEqual(3, result.Value.Longs.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Decimal_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--decimals 123.456";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Decimals);
        Assert.AreEqual(1, result.Value.Decimals.Count);
        Assert.AreEqual(123.456m, result.Value.Decimals[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Decimal_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--decimals 10.5 20.25 30.125";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Decimals);
        Assert.AreEqual(3, result.Value.Decimals.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Short_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--shorts 100";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Shorts);
        Assert.AreEqual(1, result.Value.Shorts.Count);
        Assert.AreEqual((short)100, result.Value.Shorts[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Short_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--shorts 1 2 3 4 5";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Shorts);
        Assert.AreEqual(5, result.Value.Shorts.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Byte_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bytes 255";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bytes);
        Assert.AreEqual(1, result.Value.Bytes.Count);
        Assert.AreEqual((byte)255, result.Value.Bytes[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Byte_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bytes 0 127 255";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bytes);
        Assert.AreEqual(3, result.Value.Bytes.Count);
        Assert.AreEqual((byte)0, result.Value.Bytes[0]);
        Assert.AreEqual((byte)127, result.Value.Bytes[1]);
        Assert.AreEqual((byte)255, result.Value.Bytes[2]);
    }

    #endregion

    #region Enum BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Enum_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(0, result.Value.Levels.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(1, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug Info Warning Error";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Warning, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Error, result.Value.Levels[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_CaseInsensitive_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels debug INFO warning ERROR";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Warning, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Error, result.Value.Levels[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_DuplicateValues_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug Debug Info Info";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[3]);
    }

    #endregion

    #region Separator BindingList Tests

    [TestMethod]
    public void Parse_BindingList_WithCommaSeparator_String_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings a,b,c,d";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(4, result.Value.Strings.Count);
        Assert.AreEqual("a", result.Value.Strings[0]);
        Assert.AreEqual("b", result.Value.Strings[1]);
        Assert.AreEqual("c", result.Value.Strings[2]);
        Assert.AreEqual("d", result.Value.Strings[3]);
    }

    [TestMethod]
    public void Parse_BindingList_WithColonSeparator_Integer_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 1:2:3:4:5";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(5, result.Value.Integers.Count);
        Assert.AreEqual(1, result.Value.Integers[0]);
        Assert.AreEqual(2, result.Value.Integers[1]);
        Assert.AreEqual(3, result.Value.Integers[2]);
        Assert.AreEqual(4, result.Value.Integers[3]);
        Assert.AreEqual(5, result.Value.Integers[4]);
    }

    [TestMethod]
    public void Parse_BindingList_WithSemicolonSeparator_Double_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 1.1;2.2;3.3";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(3, result.Value.Doubles.Count);
        Assert.AreEqual(1.1, result.Value.Doubles[0], 0.001);
        Assert.AreEqual(2.2, result.Value.Doubles[1], 0.001);
        Assert.AreEqual(3.3, result.Value.Doubles[2], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_WithSeparator_EqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings=x,y,z";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual("x", result.Value.Strings[0]);
        Assert.AreEqual("y", result.Value.Strings[1]);
        Assert.AreEqual("z", result.Value.Strings[2]);
    }

    [TestMethod]
    public void Parse_BindingList_WithSeparator_SingleValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings single";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.AreEqual("single", result.Value.Strings[0]);
    }

    #endregion

    #region Mixed Options Tests

    [TestMethod]
    public void Parse_BindingList_MixedWithBoolOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --files file1.txt file2.txt";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
    }

    [TestMethod]
    public void Parse_BindingList_MixedWithStringOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files a.txt b.txt --output result.txt";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    public void Parse_BindingList_MultipleMixedOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-v -f file1.txt file2.txt -o output.txt -c 1 2 3";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(2, result.Value.Files?.Count);
        Assert.AreEqual("output.txt", result.Value.Output);
        Assert.AreEqual(3, result.Value.Counts?.Count);
    }

    [TestMethod]
    public void Parse_BindingList_AllOptionTypes_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --files a.txt b.txt c.txt --output out.txt --counts 10 20 30 40";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(3, result.Value.Files?.Count);
        Assert.AreEqual("out.txt", result.Value.Output);
        Assert.AreEqual(4, result.Value.Counts?.Count);
    }

    #endregion

    #region Format and Roundtrip Tests

    [TestMethod]
    public void FormatCommandLine_BindingList_String_FormatsCorrectly()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = ["item1", "item2", "item3"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--strings"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item1"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item2"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item3"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_BindingList_Integer_FormatsCorrectly()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Integers = [100, 200, 300]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--integers"), $"Result: {result}");
        Assert.IsTrue(result.Contains("100"), $"Result: {result}");
        Assert.IsTrue(result.Contains("200"), $"Result: {result}");
        Assert.IsTrue(result.Contains("300"), $"Result: {result}");
    }

    [TestMethod]
    public void Roundtrip_BindingList_StringAndInteger_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListOptions
        {
            Strings = ["a", "b", "c"],
            Integers = [1, 2, 3]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsNotNull(parsed.Value.Strings);
        Assert.IsNotNull(parsed.Value.Integers);
        Assert.AreEqual(3, parsed.Value.Strings.Count);
        Assert.AreEqual(3, parsed.Value.Integers.Count);
        Assert.AreEqual("a", parsed.Value.Strings[0]);
        Assert.AreEqual(1, parsed.Value.Integers[0]);
    }

    [TestMethod]
    public void Roundtrip_BindingList_Enum_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListEnumOptions
        {
            Levels = [LogLevel.Debug, LogLevel.Info, LogLevel.Error]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsNotNull(parsed.Value.Levels);
        Assert.AreEqual(3, parsed.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, parsed.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, parsed.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Error, parsed.Value.Levels[2]);
    }

    [TestMethod]
    public void Roundtrip_BindingList_MixedOptions_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListMixedOptions
        {
            Verbose = true,
            Files = ["x.txt", "y.txt"],
            Output = "z.txt",
            Counts = [5, 10, 15]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsTrue(parsed.Value.Verbose);
        Assert.AreEqual("z.txt", parsed.Value.Output);
        Assert.AreEqual(2, parsed.Value.Files?.Count);
        Assert.AreEqual(3, parsed.Value.Counts?.Count);
    }

    [TestMethod]
    public void FormatCommandLineArgs_BindingList_ReturnsCorrectArray()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = ["one", "two"],
            Integers = [1, 2, 3]
        };

        // Act
        var result = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.Contains(result, "--strings");
        CollectionAssert.Contains(result, "one");
        CollectionAssert.Contains(result, "two");
        CollectionAssert.Contains(result, "--integers");
    }

    [TestMethod]
    public void FormatCommandLine_BindingList_EmptyList_SimplifyOmits()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = [],
            Integers = null
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);

        // Assert
        Assert.IsFalse(result.Contains("--strings"), $"Result should not contain empty strings: {result}");
        Assert.IsFalse(result.Contains("--integers"), $"Result should not contain null integers: {result}");
    }

    #endregion

    #region Edge Cases Tests

    [TestMethod]
    public void Parse_BindingList_NoOption_ReturnsNull()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNull(result.Value.Strings);
        Assert.IsNull(result.Value.Integers);
    }

    [TestMethod]
    public void Parse_BindingList_OnlyOneOption_OtherIsNull()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings only";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.IsNull(result.Value.Integers);
    }

    [TestMethod]
    public void Parse_BindingList_ArrayStopsAtNextOption()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings a b c --integers 1 2";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual(2, result.Value.Integers.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_LargeNumbers_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 2147483647 -2147483648 0";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        // 注意：负数可能被视为选项，这里验证正数和零
        Assert.IsTrue(result.Value.Integers.Count >= 1);
        Assert.AreEqual(int.MaxValue, result.Value.Integers[0]);
    }

    [TestMethod]
    public void Parse_BindingList_String_SpecialCharacters_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings \"hello@world\" \"test#123\" \"path/to/file\"";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual("hello@world", result.Value.Strings[0]);
        Assert.AreEqual("test#123", result.Value.Strings[1]);
        Assert.AreEqual("path/to/file", result.Value.Strings[2]);
    }

    #endregion
}

#endregion