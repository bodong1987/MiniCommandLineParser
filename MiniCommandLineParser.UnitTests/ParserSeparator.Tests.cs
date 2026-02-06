namespace MiniCommandLineParser.UnitTests;

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
        // Arrange - default separator is ';'
        var parser = new Parser();
        var commandLine = "--files file1.txt;file2.txt;file3.txt";

        // Act
        var result = parser.Parse<ArrayOptions>(commandLine);

        // Assert - with default separator ';', the value is split
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
