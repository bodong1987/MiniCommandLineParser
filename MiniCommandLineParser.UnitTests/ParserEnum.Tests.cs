namespace MiniCommandLineParser.UnitTests;

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
