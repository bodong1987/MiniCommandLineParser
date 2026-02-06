namespace MiniCommandLineParser.UnitTests;

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
