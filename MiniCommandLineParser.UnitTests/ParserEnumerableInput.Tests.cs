namespace MiniCommandLineParser.UnitTests;

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
