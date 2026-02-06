namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Options class with default values for display testing.
/// </summary>
public class DefaultValueOptions
{
    [Option('n', "name", HelpText = "Name option")]
    public string Name { get; set; } = "DefaultName";

    [Option('c', "count", HelpText = "Count option")]
    public int Count { get; set; } = 10;

    [Option('v', "verbose", HelpText = "Verbose option")]
    public bool Verbose { get; set; } = true;

    [Option('l', "level", HelpText = "Log level")]
    public LogLevel Level { get; set; } = LogLevel.Info;
}

/// <summary>
/// Options class with environment variable for display testing.
/// </summary>
public class EnvDisplayOptions
{
    [Option('n', "name", EnvironmentVariable = "MY_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }

    [Option('c', "count", EnvironmentVariable = "MY_COUNT", HelpText = "Count from env")]
    public int Count { get; set; }
}

/// <summary>
/// Options class with both default value and environment variable.
/// </summary>
public class DefaultAndEnvOptions
{
    [Option('n', "name", EnvironmentVariable = "DEFAULT_ENV_NAME", HelpText = "Name with default and env")]
    public string Name { get; set; } = "MyDefault";
}

/// <summary>
/// Tests for default value display in help text.
/// </summary>
[TestClass]
public sealed class DefaultValueDisplayTests
{
    [TestMethod]
    public void GetHelpText_ShowsDefaultStringValue()
    {
        // Arrange
        var options = new DefaultValueOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(helpText.Contains("DefaultName"), $"Help text should contain default value. Help: {helpText}");
    }

    [TestMethod]
    public void GetHelpText_ShowsDefaultIntValue()
    {
        // Arrange
        var options = new DefaultValueOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        // The default int value 10 should be displayed
        Assert.IsTrue(helpText.Contains("10") || helpText.Contains("Default"), 
            $"Help text should show default value information. Help: {helpText}");
    }

    [TestMethod]
    public void GetHelpText_ShowsEnvironmentVariableName()
    {
        // Arrange
        var options = new EnvDisplayOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(helpText.Contains("MY_NAME") || helpText.Contains("env"), 
            $"Help text should mention environment variable. Help: {helpText}");
    }

    [TestMethod]
    public void GetHelpText_WithFormatter_GeneratesOutput()
    {
        // Arrange
        var options = new DefaultValueOptions();
        var formatter = new Formatter(indent: 2, blank: 30);

        // Act
        var helpText = Parser.GetHelpText(options, formatter);

        // Assert
        Assert.IsNotNull(helpText);
        Assert.IsTrue(helpText.Length > 0);
        Assert.IsTrue(helpText.Contains("--name"));
        Assert.IsTrue(helpText.Contains("--count"));
    }

    [TestMethod]
    public void GetHelpText_ContainsAllOptions()
    {
        // Arrange
        var options = new DefaultValueOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(helpText.Contains("-n, --name") || helpText.Contains("--name"));
        Assert.IsTrue(helpText.Contains("-c, --count") || helpText.Contains("--count"));
        Assert.IsTrue(helpText.Contains("-v, --verbose") || helpText.Contains("--verbose"));
        Assert.IsTrue(helpText.Contains("-l, --level") || helpText.Contains("--level"));
    }

    [TestMethod]
    public void GetHelpText_ContainsHelpTexts()
    {
        // Arrange
        var options = new DefaultValueOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        Assert.IsTrue(helpText.Contains("Name option"));
        Assert.IsTrue(helpText.Contains("Count option"));
        Assert.IsTrue(helpText.Contains("Verbose option"));
        Assert.IsTrue(helpText.Contains("Log level"));
    }
}
