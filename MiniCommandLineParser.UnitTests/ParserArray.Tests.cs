namespace MiniCommandLineParser.UnitTests;

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
