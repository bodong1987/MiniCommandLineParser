namespace MiniCommandLineParser.UnitTests;

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

        // Assert - Arrays with default separator ';' use semicolon separation
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

        // Assert - In equal sign style with default separator ';', arrays use semicolon separation
        Assert.IsTrue(result.Contains("--files="), $"Result: {result}");
        Assert.IsTrue(result.Contains("normal.txt"), $"Result: {result}");
        Assert.IsTrue(result.Contains("path with space.txt"), $"Result: {result}");
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
