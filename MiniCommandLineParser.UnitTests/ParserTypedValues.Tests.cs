namespace MiniCommandLineParser.UnitTests;

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
