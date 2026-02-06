namespace MiniCommandLineParser.UnitTests;

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
        Assert.IsTrue(result.ErrorMessage.Contains("Unknown option"));
    }
}
