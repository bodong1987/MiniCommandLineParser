namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Options class for testing multi-line formatting and column alignment.
/// </summary>
public class FormattingTestOptions
{
    [Option('f', "files", Index = 0, MetaName = "PATHS",
        HelpText = "File or directory paths to process.")]
    public List<string> Files { get; set; } = [];

    [Option('p', "project",
        HelpText = "Project root directory.")]
    public string Project { get; set; } = "";

    [Option('n', "dry-run",
        HelpText = "List files without actually processing.")]
    public bool DryRun { get; set; }

    [Option("force",
        HelpText = "Force the operation even if conflicts exist.")]
    public bool Force { get; set; } = true;

    [Option("long-option-name-that-is-very-descriptive",
        HelpText = "A very long option name to test alignment.")]
    public string LongOption { get; set; } = "";
}

/// <summary>
/// Options class with environment variable for formatting test.
/// </summary>
public class FormattingEnvOptions
{
    [Option('k', "api-key", EnvironmentVariable = "API_KEY",
        HelpText = "API key for authentication.")]
    public string ApiKey { get; set; } = "";

    [Option("timeout",
        HelpText = "Timeout in seconds.")]
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// Tests for the improved multi-line help text formatting with proper column alignment.
/// </summary>
[TestClass]
public sealed class HelpTextFormattingTests
{
    [TestMethod]
    public void HelpText_OptionNameAndAttributes_OnSeparateLineFromDescription()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);
        var lines = helpText.Split(Environment.NewLine);

        // Find the line with --project option name
        var projectNameLine = lines.FirstOrDefault(l => l.Contains("-p, --project"));
        Assert.IsNotNull(projectNameLine, $"Should have a line with '-p, --project'. Help:\n{helpText}");

        // The attribute [Optional] should be on the same line as the name
        Assert.IsTrue(projectNameLine.Contains("[Optional]"),
            $"Attribute should be on the same line as option name. Line: {projectNameLine}");

        // The help text should be on a SEPARATE line
        Assert.IsFalse(projectNameLine.Contains("Project root"),
            $"Help text should NOT be on the same line as option name. Line: {projectNameLine}");

        // Description should be on the next non-empty line
        var descLine = lines.FirstOrDefault(l => l.Contains("Project root directory"));
        Assert.IsNotNull(descLine, $"Should have a description line. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_DescriptionLine_IsIndentedToAttributeColumn()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);
        var lines = helpText.Split(Environment.NewLine);

        // Find option name line and description line for --project
        var nameLine = lines.FirstOrDefault(l => l.Contains("-p, --project"));
        var descLine = lines.FirstOrDefault(l => l.Contains("Project root directory"));

        Assert.IsNotNull(nameLine);
        Assert.IsNotNull(descLine);

        // The description should start at the same column as [Optional]
        var attrStart = nameLine!.IndexOf('[');
        var descStart = descLine!.Length - descLine.TrimStart().Length;

        // Both should be indented the same amount (indent + blank)
        Assert.IsTrue(attrStart > 0, "Attribute column should be > 0");
        Assert.AreEqual(attrStart, descStart,
            $"Description indent ({descStart}) should match attribute column ({attrStart})");
    }

    [TestMethod]
    public void HelpText_LongOptionName_StillAlignsCorrectly()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // Even long names should have the attribute on the same line
        Assert.IsTrue(helpText.Contains("--long-option-name-that-is-very-descriptive"),
            "Should contain the long option name.");
        Assert.IsTrue(helpText.Contains("A very long option name"),
            "Should contain the description.");
    }

    [TestMethod]
    public void HelpText_PositionalArgument_ShowsAliases()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // Positional argument should show aliases
        Assert.IsTrue(helpText.Contains("<PATHS>"),
            $"Should contain positional meta name. Help:\n{helpText}");
        Assert.IsTrue(helpText.Contains("-f, --files") || helpText.Contains("(-f, --files)"),
            $"Should contain named aliases for dual-mode property. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_DefaultTrueBoolean_ShowsInAttributes()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // --force defaults to true, should show Default: True
        Assert.IsTrue(helpText.Contains("Default: True"),
            $"Should show 'Default: True' for force option. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_DefaultFalseBoolean_NotShown()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);
        var lines = helpText.Split(Environment.NewLine);

        // --dry-run defaults to false, should NOT show Default: False
        var dryRunLine = lines.FirstOrDefault(l => l.Contains("--dry-run"));
        Assert.IsNotNull(dryRunLine);
        Assert.IsFalse(dryRunLine!.Contains("Default:"),
            $"Should not show default for false bool. Line: {dryRunLine}");
    }

    [TestMethod]
    public void HelpText_EnvironmentVariable_ShownInAttributes()
    {
        var options = new FormattingEnvOptions();
        var helpText = Parser.GetHelpText(options);

        Assert.IsTrue(helpText.Contains("Env: API_KEY"),
            $"Should show environment variable name. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_AttributeSeparator_UsesCommaSpace()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // Attributes should be separated by ", " not ","
        // Check for the pattern with spaces after comma
        Assert.IsTrue(
            helpText.Contains(", Index: 0") || helpText.Contains(", Array") || helpText.Contains(", Default: "),
            $"Attributes should use ', ' separator. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_CollectionDefault_NotShowSystemTypeName()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // Should NOT show System.ComponentModel.BindingList`1[...] as default value
        Assert.IsFalse(helpText.Contains("System."),
            $"Should not show System.* type names as default values. Help:\n{helpText}");
        Assert.IsFalse(helpText.Contains("`1"),
            $"Should not show generic type markers. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_UsageLine_OnSeparateLine()
    {
        var options = new FormattingTestOptions();
        var helpText = Parser.GetHelpText(options);

        // Usage lines should start with "usage:" on their own line
        if (helpText.Contains("usage:"))
        {
            var lines = helpText.Split(Environment.NewLine);
            var usageLines = lines.Where(l => l.TrimStart().StartsWith("usage:")).ToList();
            Assert.IsTrue(usageLines.Count > 0,
                "Usage should appear at the start of its line (after indentation).");
        }
    }

    [TestMethod]
    public void HelpText_CustomFormatter_RespectsIndentAndBlank()
    {
        var options = new FormattingTestOptions();
        var formatter = new Formatter(indent: 2, blank: 25);
        var helpText = Parser.GetHelpText(options, formatter);

        Assert.IsNotNull(helpText);
        Assert.IsTrue(helpText.Contains("--project"), "Should contain option names.");
        Assert.IsTrue(helpText.Contains("Project root directory"), "Should contain description.");

        // With indent=2, lines should start with 2 spaces
        var lines = helpText.Split(Environment.NewLine)
            .Where(l => l.Contains("--project"))
            .ToList();
        Assert.IsTrue(lines.Count > 0);
        Assert.IsTrue(lines[0].StartsWith("  "), $"Should start with 2-space indent. Line: '{lines[0]}'");
    }

    [TestMethod]
    public void HelpText_RequiredOption_ShowsRequired()
    {
        var options = new RequiredOptions();
        var helpText = Parser.GetHelpText(options);

        Assert.IsTrue(helpText.Contains("Required"),
            $"Should show Required for required options. Help:\n{helpText}");
    }

    [TestMethod]
    public void HelpText_DefaultIntValue_ShownCorrectly()
    {
        var options = new FormattingEnvOptions();
        var helpText = Parser.GetHelpText(options);

        // --timeout has default 30
        Assert.IsTrue(helpText.Contains("Default: 30"),
            $"Should show 'Default: 30' for timeout. Help:\n{helpText}");
    }
}
