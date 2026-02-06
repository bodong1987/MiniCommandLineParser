namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class OptionAttributeTests
{
    [TestMethod]
    public void OptionAttribute_ToString_FormatsCorrectly()
    {
        // Arrange
        var attr = new OptionAttribute('h', "help");

        // Act
        var result = attr.ToString();

        // Assert
        Assert.AreEqual("-h --help", result);
    }

    [TestMethod]
    public void OptionAttribute_LongNameOnly_FormatsCorrectly()
    {
        // Arrange
        var attr = new OptionAttribute("help");

        // Act
        var result = attr.ToString();

        // Assert
        Assert.AreEqual("--help", result);
    }

    [TestMethod]
    public void OptionAttribute_DefaultRequired_IsFalse()
    {
        // Arrange
        var attr = new OptionAttribute("test");

        // Assert
        Assert.IsFalse(attr.Required);
    }

    [TestMethod]
    public void OptionAttribute_SetRequired_StoresValue()
    {
        // Arrange
        var attr = new OptionAttribute("test") { Required = true };

        // Assert
        Assert.IsTrue(attr.Required);
    }

    [TestMethod]
    public void OptionAttribute_SetHelpText_StoresValue()
    {
        // Arrange
        var attr = new OptionAttribute("test") { HelpText = "Test help" };

        // Assert
        Assert.AreEqual("Test help", attr.HelpText);
    }
}
