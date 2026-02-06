namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class ParserExistingInstanceTests
{
    [TestMethod]
    public void Parse_ExistingInstance_UpdatesInstance()
    {
        // Arrange
        var parser = new Parser();
        var existing = new BasicOptions { Verbose = true, Count = 5 };
        var commandLine = "--name John";

        // Act
        var result = parser.Parse(commandLine, existing);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreSame(existing, result.Value);
        Assert.IsTrue(result.Value!.Verbose); // preserved
        Assert.AreEqual(5, result.Value.Count); // preserved
        Assert.AreEqual("John", result.Value.Name); // updated
    }
}
