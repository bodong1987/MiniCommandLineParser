namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for the <c>--no-xxx</c> boolean negation prefix convention.
/// When a boolean option <c>--force</c> exists, <c>--no-force</c> should set it to <c>false</c>.
/// This follows the POSIX/GNU convention used by tools like git, npm, and Python argparse.
/// </summary>
[TestClass]
public sealed class NoBooleanPrefixTests
{
    // ── Basic --no-xxx → false ──────────────────────────────────

    [TestMethod]
    public void Parse_NoPrefix_SetsBooleanFalse()
    {
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--no-verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose, "--no-verbose should set Verbose to false.");
    }

    [TestMethod]
    public void Parse_NoPrefix_OverridesDefaultTrue()
    {
        // Verify --no-force overrides a default-true boolean property
        var parser = new Parser();
        var opts = new DefaultTrueOptions(); // Force defaults to true
        var result = parser.Parse(["--no-force"], opts);

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Force, "--no-force should override default true.");
    }

    [TestMethod]
    public void Parse_NoPrefix_CaseInsensitive()
    {
        var parser = new Parser(); // CaseSensitive defaults to false
        var result = parser.Parse<BasicOptions>("--NO-VERBOSE");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose, "--NO-VERBOSE should work case-insensitively.");
    }

    [TestMethod]
    public void Parse_NoPrefix_CaseSensitive_ExactMatchRequired()
    {
        var parser = new Parser(new ParserSettings { CaseSensitive = true });
        var result = parser.Parse<BasicOptions>("--no-verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    // ── With other options ──────────────────────────────────────

    [TestMethod]
    public void Parse_NoPrefix_WithOtherOptions_AllParsed()
    {
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--name John --no-verbose --count 5");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsFalse(result.Value.Verbose);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_NoPrefix_BeforePositionalArgs_DoesNotConsumePositional()
    {
        var parser = new Parser();
        var result = parser.Parse<CloneCommandOptions>("--no-verbose clone https://example.com");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose, "--no-verbose should set false.");
        Assert.AreEqual("clone", result.Value.Command, "Positional arg should not be consumed by --no-verbose.");
        Assert.AreEqual("https://example.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_NoPrefix_AtEnd_SetsFalse()
    {
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--name John --no-verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsFalse(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_NoPrefix_AtBeginning_SetsFalse()
    {
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--no-verbose --name John");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    // ── Precedence: --force vs --no-force ───────────────────────

    [TestMethod]
    public void Parse_NoPrefix_AfterPositive_LastWins()
    {
        // --verbose --no-verbose → last one wins → false
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--verbose --no-verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose, "Last occurrence should win: --no-verbose → false.");
    }

    [TestMethod]
    public void Parse_Positive_AfterNoPrefix_LastWins()
    {
        // --no-verbose --verbose → last one wins → true
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--no-verbose --verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose, "Last occurrence should win: --verbose → true.");
    }

    // ── Non-boolean properties should NOT match ─────────────────

    [TestMethod]
    public void Parse_NoPrefix_NonBooleanProperty_TreatedAsUnknown()
    {
        // --no-name should NOT match the string "name" property
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });
        var result = parser.Parse<BasicOptions>("--no-name");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        // Name should remain null (not affected)
        Assert.IsNull(result.Value!.Name);
    }

    [TestMethod]
    public void Parse_NoPrefix_NonBooleanProperty_ErrorWhenStrict()
    {
        // With strict mode, --no-name should cause an error
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });
        var result = parser.Parse<BasicOptions>("--no-name");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
    }

    // ── Edge case: property actually named "no-xxx" ─────────────

    [TestMethod]
    public void Parse_PropertyNamedNoXxx_ExactMatchTakesPriority()
    {
        // If a property is actually named "no-debug", it should match directly
        // without being treated as negation of "debug"
        var parser = new Parser();
        var result = parser.Parse<OptionsWithNoPrefix>("--no-debug");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.NoDebug, "Exact match 'no-debug' property should be set to true.");
    }

    // ── Mixed with positional args (dual-mode) ──────────────────

    [TestMethod]
    public void Parse_NoPrefix_WithDualModePositional()
    {
        var parser = new Parser();
        var result = parser.Parse<MixedOptions>("build target --no-force");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("build", result.Value!.Action);
        Assert.AreEqual("target", result.Value.Target);
        Assert.IsFalse(result.Value.Force);
    }

    [TestMethod]
    public void Parse_NoPrefix_EquivalentToEqualsFalse()
    {
        // --no-verbose should produce the same result as --verbose=false
        var parser = new Parser();

        var noResult = parser.Parse<BasicOptions>("--no-verbose --name John");
        var falseResult = parser.Parse<BasicOptions>("--verbose=false --name John");

        Assert.AreEqual(noResult.Value!.Verbose, falseResult.Value!.Verbose,
            "--no-verbose and --verbose=false should be equivalent.");
        Assert.AreEqual(noResult.Value.Name, falseResult.Value.Name);
    }

    // ── Helper class with default-true boolean ──────────────────

    /// <summary>
    /// Options class where Force defaults to true, testing --no-force override.
    /// </summary>
    private class DefaultTrueOptions
    {
        [Option("force", HelpText = "Force operation")]
        public bool Force { get; set; } = true;

        [Option("clean", HelpText = "Clean up")]
        public bool Clean { get; set; }
    }

    /// <summary>
    /// Options class with a property whose long name is literally "no-debug",
    /// verifying that exact matches take priority over the --no- prefix convention.
    /// </summary>
    private class OptionsWithNoPrefix
    {
        [Option("no-debug", HelpText = "A property that happens to start with 'no-'")]
        public bool NoDebug { get; set; }
    }
}
