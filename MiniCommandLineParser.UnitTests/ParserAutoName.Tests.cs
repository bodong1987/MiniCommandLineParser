namespace MiniCommandLineParser.UnitTests;

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
