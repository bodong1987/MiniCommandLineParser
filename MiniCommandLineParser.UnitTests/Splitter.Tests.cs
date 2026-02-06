namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class SplitterTests
{
    [TestMethod]
    public void SplitCommandLine_SimpleArguments_ReturnsCorrectList()
    {
        // Arrange
        var commandLine = "--name test --count 5";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("test", result[1]);
        Assert.AreEqual("--count", result[2]);
        Assert.AreEqual("5", result[3]);
    }

    [TestMethod]
    public void SplitCommandLine_QuotedArguments_PreservesQuotedContent()
    {
        // Arrange
        var commandLine = "--message \"Hello World\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--message", result[0]);
        Assert.AreEqual("Hello World", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_WithHashComment_RemovesComment()
    {
        // Arrange
        var commandLine = "--name test # this is a comment";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, removeHashComments: true);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("test", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_WithHashCommentDisabled_KeepsComment()
    {
        // Arrange
        var commandLine = "--name test #comment";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, removeHashComments: false);

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("#comment", result[2]);
    }

    [TestMethod]
    public void SplitCommandLine_EmptyString_ReturnsEmptyList()
    {
        // Arrange
        var commandLine = "";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SplitCommandLine_WhitespaceOnly_ReturnsEmptyList()
    {
        // Arrange
        var commandLine = "   \t  ";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SplitCommandLine_EscapedQuotes_HandlesCorrectly()
    {
        // Arrange
        var commandLine = "--path \"C:\\Program Files\\test\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("C:\\Program Files\\test", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var commandLine = "--name=test --count=5";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("--name=test", result[0]);
        Assert.AreEqual("--count=5", result[1]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_WithSpacesAroundEquals_SplitsAsArguments()
    {
        // Arrange - Note: spaces around = means they are separate arguments
        var commandLine = "--name = test";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("--name", result[0]);
        Assert.AreEqual("=", result[1]);
        Assert.AreEqual("test", result[2]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_QuotedValueWithEquals_PreservesQuotes()
    {
        // Arrange
        // Note: Quotes are only removed when the ENTIRE argument is surrounded by quotes.
        // Here, quotes only surround the value part, so they are preserved in the output.
        var commandLine = "--config=\"key=value\"";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--config=\"key=value\"", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_MultipleEquals_PreservesAll()
    {
        // Arrange
        var commandLine = "--expr=a=b=c";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--expr=a=b=c", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_EmptyValue_Preserved()
    {
        // Arrange
        var commandLine = "--name=";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("--name=", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_EqualsSyntax_ShortOption_ParsesCorrectly()
    {
        // Arrange
        var commandLine = "-n=value";

        // Act
        var result = Splitter.SplitCommandLineIntoArguments(commandLine);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("-n=value", result[0]);
    }

    [TestMethod]
    public void SplitCommandLine_IllegalCharacter_ReportsIllegalChar()
    {
        // Arrange
        var commandLine = "--name test|value";

        // Act
        // ReSharper disable once UnusedVariable
        var result = Splitter.SplitCommandLineIntoArguments(commandLine, false, out var illegalChar);

        // Assert
        Assert.IsNotNull(illegalChar);
        Assert.AreEqual('|', illegalChar.Value);
    }
}
