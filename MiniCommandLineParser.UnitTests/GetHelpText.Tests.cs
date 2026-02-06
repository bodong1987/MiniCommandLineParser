namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class GetHelpTextTests
{
    [TestMethod]
    public void GetHelpText_ContainsOptionNames()
    {
        // Arrange
        var options = new BasicOptions();

        // Act
        var result = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(result.Contains("-v, --verbose"));
        Assert.IsTrue(result.Contains("-n, --name"));
        Assert.IsTrue(result.Contains("-c, --count"));
    }

    [TestMethod]
    public void GetHelpText_ContainsHelpTexts()
    {
        // Arrange
        var options = new BasicOptions();

        // Act
        var result = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(result.Contains("Enable verbose output"));
        Assert.IsTrue(result.Contains("The name parameter"));
        Assert.IsTrue(result.Contains("The count parameter"));
    }

    [TestMethod]
    public void GetHelpText_WithCustomFormatter_UsesFormatter()
    {
        // Arrange
        var options = new BasicOptions();
        var formatter = new Formatter(indent: 2, blank: 30);

        // Act
        var result = Parser.GetHelpText(options, formatter);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }
}
