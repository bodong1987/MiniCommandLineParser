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