namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class ParserResultTests
{
    [TestMethod]
    public void ParserResult_AppendError_AddsErrorMessage()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("Error 1");
        result.AppendError("Error 2");

        // Assert
        Assert.IsTrue(result.ErrorMessage.Contains("Error 1"));
        Assert.IsTrue(result.ErrorMessage.Contains("Error 2"));
    }

    [TestMethod]
    public void ParserResult_InitialResult_IsNotParsed()
    {
        // Arrange & Act
        var result = new ParserResult<BasicOptions>(null);

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
    }
}
