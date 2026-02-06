namespace MiniCommandLineParser.UnitTests;

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
