namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for dual-mode options: properties that have both a positional Index AND named long/short names.
/// This allows the same property to be set either by position or by explicit --name syntax.
/// </summary>
[TestClass]
public sealed class DualModePositionalNamedTests
{
    #region Positional Parsing (dual-mode properties used positionally)

    [TestMethod]
    [Description("Dual-mode options parsed as positional arguments")]
    public void Parse_DualMode_Positional_ParsesBothArgs()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("/path/to/file.json /output/dir");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("/path/to/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/output/dir", result.Value.Output);
        Assert.IsFalse(result.Value.ValidateOnly);
    }

    [TestMethod]
    [Description("Dual-mode options parsed as positional with trailing named flag")]
    public void Parse_DualMode_PositionalWithNamedFlag_ParsesAll()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("/path/to/file.json /output/dir --validate-only");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("/path/to/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/output/dir", result.Value.Output);
        Assert.IsTrue(result.Value.ValidateOnly);
    }

    [TestMethod]
    [Description("Dual-mode options with URL as positional argument")]
    public void Parse_DualMode_UrlPositional_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("https://example.com/project.json D:/Projects/MyProject");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("https://example.com/project.json", result.Value!.Descriptor);
        Assert.AreEqual("D:/Projects/MyProject", result.Value.Output);
    }

    #endregion

    #region Named Parsing (dual-mode properties used with --name)

    [TestMethod]
    [Description("Dual-mode options parsed with long named arguments")]
    public void Parse_DualMode_LongNamed_ParsesBothArgs()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--descriptor /path/to/file.json --output /output/dir");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/to/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/output/dir", result.Value.Output);
    }

    [TestMethod]
    [Description("Dual-mode options parsed with short named arguments")]
    public void Parse_DualMode_ShortNamed_ParsesBothArgs()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("-d /path/to/file.json -o /output/dir");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/to/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/output/dir", result.Value.Output);
    }

    [TestMethod]
    [Description("Dual-mode options parsed with named args and trailing flag")]
    public void Parse_DualMode_NamedWithFlag_ParsesAll()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--descriptor /path/file.json --output /out --validate-only");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/out", result.Value.Output);
        Assert.IsTrue(result.Value.ValidateOnly);
    }

    [TestMethod]
    [Description("Dual-mode options parsed with equals syntax")]
    public void Parse_DualMode_EqualsSyntax_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--descriptor=/path/file.json --output=/out");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/out", result.Value.Output);
    }

    [TestMethod]
    [Description("Dual-mode long-only options parsed with named args")]
    public void Parse_DualModeLongOnly_Named_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeLongOnlyOptions>("--source /src --destination /dst");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/src", result.Value!.Source);
        Assert.AreEqual("/dst", result.Value.Destination);
    }

    [TestMethod]
    [Description("Dual-mode long-only options parsed positionally")]
    public void Parse_DualModeLongOnly_Positional_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeLongOnlyOptions>("/src /dst");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("/src", result.Value!.Source);
        Assert.AreEqual("/dst", result.Value.Destination);
    }

    #endregion

    #region Mixed Positional and Named (some args positional, some named)

    [TestMethod]
    [Description("First arg positional, second arg named")]
    public void Parse_DualMode_FirstPositionalSecondNamed_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("/path/file.json --output /out");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/out", result.Value.Output);
    }

    [TestMethod]
    [Description("Named args in reverse order should still work")]
    public void Parse_DualMode_NamedReverseOrder_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--output /out --descriptor /path/file.json");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", result.Value!.Descriptor);
        Assert.AreEqual("/out", result.Value.Output);
    }

    #endregion

    #region Required Validation with Dual-Mode

    [TestMethod]
    [Description("Required dual-mode options missing should fail validation")]
    public void Parse_DualMode_MissingRequired_Fails()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--validate-only");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.ErrorMessage.Contains("descriptor") || result.ErrorMessage.Contains("DESCRIPTOR")
            || result.ErrorMessage.Contains("Required"),
            $"Error should mention missing required option. Got: {result.ErrorMessage}");
    }

    [TestMethod]
    [Description("Required positional provided, required named missing should fail")]
    public void Parse_DualMode_OneRequiredMissing_Fails()
    {
        var parser = new Parser();

        // Only first positional provided, second required is missing
        var result = parser.Parse<DualModeOptions>("/path/file.json");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.ErrorMessage.Contains("output") || result.ErrorMessage.Contains("OUTPUT_DIR")
            || result.ErrorMessage.Contains("Required"),
            $"Error should mention missing required option. Got: {result.ErrorMessage}");
    }

    [TestMethod]
    [Description("All required provided positionally should pass")]
    public void Parse_DualMode_AllRequiredPositional_Passes()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("/path/file.json /output/dir");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
    }

    [TestMethod]
    [Description("All required provided via named should pass")]
    public void Parse_DualMode_AllRequiredNamed_Passes()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModeOptions>("--descriptor /path/file.json --output /output/dir");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
    }

    [TestMethod]
    [Description("Partial required: only required option provided positionally")]
    public void Parse_DualModePartialRequired_OnlyRequired_Passes()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModePartialRequiredOptions>("/src/path");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/src/path", result.Value!.Source);
        Assert.AreEqual("", result.Value.Dest); // Optional, default
    }

    [TestMethod]
    [Description("Partial required: required option provided via named")]
    public void Parse_DualModePartialRequired_NamedRequired_Passes()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModePartialRequiredOptions>("--source /src/path");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/src/path", result.Value!.Source);
    }

    [TestMethod]
    [Description("Partial required: required option missing should fail")]
    public void Parse_DualModePartialRequired_MissingRequired_Fails()
    {
        var parser = new Parser();

        var result = parser.Parse<DualModePartialRequiredOptions>("--dest /dst/path --force");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
    }

    #endregion

    #region Existing Instance Parsing

    [TestMethod]
    [Description("Dual-mode options parsed into existing instance with positional args")]
    public void Parse_DualMode_ExistingInstance_Positional()
    {
        var parser = new Parser();
        var instance = new DualModeOptions();

        var result = parser.Parse("/path/file.json /output/dir", instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", instance.Descriptor);
        Assert.AreEqual("/output/dir", instance.Output);
    }

    [TestMethod]
    [Description("Dual-mode options parsed into existing instance with named args")]
    public void Parse_DualMode_ExistingInstance_Named()
    {
        var parser = new Parser();
        var instance = new DualModeOptions();

        var result = parser.Parse("--descriptor /path/file.json --output /output/dir", instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", instance.Descriptor);
        Assert.AreEqual("/output/dir", instance.Output);
    }

    [TestMethod]
    [Description("Dual-mode options parsed into existing instance with IEnumerable positional args")]
    public void Parse_DualMode_ExistingInstance_EnumerablePositional()
    {
        var parser = new Parser();
        var instance = new DualModeOptions();

        var result = parser.Parse(new[] { "/path/file.json", "/output/dir" }, instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", instance.Descriptor);
        Assert.AreEqual("/output/dir", instance.Output);
    }

    [TestMethod]
    [Description("Dual-mode options parsed into existing instance with IEnumerable named args")]
    public void Parse_DualMode_ExistingInstance_EnumerableNamed()
    {
        var parser = new Parser();
        var instance = new DualModeOptions();

        var result = parser.Parse(
            new[] { "--descriptor", "/path/file.json", "--output", "/output/dir" }, instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("/path/file.json", instance.Descriptor);
        Assert.AreEqual("/output/dir", instance.Output);
    }

    #endregion

    #region TypeInfo Tests

    [TestMethod]
    [Description("Dual-mode properties should appear in PositionalProperties")]
    public void TypeInfo_DualMode_AppearsInPositionalProperties()
    {
        var typeInfo = Parser.GetTypeInfo<DualModeOptions>();

        var positional = typeInfo.PositionalProperties;

        Assert.AreEqual(2, positional.Length);
        Assert.AreEqual(0, positional[0].Attribute.Index);
        Assert.AreEqual("DESCRIPTOR", positional[0].Attribute.MetaName);
        Assert.AreEqual(1, positional[1].Attribute.Index);
        Assert.AreEqual("OUTPUT_DIR", positional[1].Attribute.MetaName);
    }

    [TestMethod]
    [Description("FindLongProperty should find dual-mode properties by long name")]
    public void TypeInfo_FindLongProperty_FindsDualModeProperty()
    {
        var typeInfo = Parser.GetTypeInfo<DualModeOptions>();

        var descriptor = typeInfo.FindLongProperty("descriptor", true);
        var output = typeInfo.FindLongProperty("output", true);

        Assert.IsNotNull(descriptor, "FindLongProperty should find 'descriptor' even though it's positional");
        Assert.IsNotNull(output, "FindLongProperty should find 'output' even though it's positional");
        Assert.AreEqual("Descriptor", descriptor.Property.Name);
        Assert.AreEqual("Output", output.Property.Name);
    }

    [TestMethod]
    [Description("FindShortProperty should find dual-mode properties by short name")]
    public void TypeInfo_FindShortProperty_FindsDualModeProperty()
    {
        var typeInfo = Parser.GetTypeInfo<DualModeOptions>();

        var descriptor = typeInfo.FindShortProperty("d", true);
        var output = typeInfo.FindShortProperty("o", true);

        Assert.IsNotNull(descriptor, "FindShortProperty should find 'd' even though it's positional");
        Assert.IsNotNull(output, "FindShortProperty should find 'o' even though it's positional");
    }

    [TestMethod]
    [Description("FindPositionalProperty should still find dual-mode properties by index")]
    public void TypeInfo_FindPositionalProperty_FindsDualModeProperty()
    {
        var typeInfo = Parser.GetTypeInfo<DualModeOptions>();

        var pos0 = typeInfo.FindPositionalProperty(0);
        var pos1 = typeInfo.FindPositionalProperty(1);

        Assert.IsNotNull(pos0);
        Assert.IsNotNull(pos1);
        Assert.AreEqual("Descriptor", pos0.Property.Name);
        Assert.AreEqual("Output", pos1.Property.Name);
    }

    #endregion

    #region Help Text

    [TestMethod]
    [Description("Help text should show dual-mode properties in positional section")]
    public void HelpText_DualMode_ShowsPositionalArgs()
    {
        var helpText = Parser.GetHelpText(new DualModeOptions());

        Assert.IsNotNull(helpText);
        Assert.IsTrue(helpText.Contains("DESCRIPTOR"),
            $"Help text should contain DESCRIPTOR. Got:\n{helpText}");
        Assert.IsTrue(helpText.Contains("OUTPUT_DIR"),
            $"Help text should contain OUTPUT_DIR. Got:\n{helpText}");
    }

    #endregion

    #region FormatCommandLine

    [TestMethod]
    [Description("FormatCommandLine should include dual-mode property values")]
    public void FormatCommandLine_DualMode_IncludesValues()
    {
        var options = new DualModeOptions
        {
            Descriptor = "/path/file.json",
            Output = "/output/dir",
            ValidateOnly = true
        };

        var commandLine = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        Assert.IsTrue(commandLine.Contains("/path/file.json"),
            $"Should contain descriptor value. Got: {commandLine}");
        Assert.IsTrue(commandLine.Contains("/output/dir"),
            $"Should contain output value. Got: {commandLine}");
        Assert.IsTrue(commandLine.Contains("--validate-only"),
            $"Should contain validate-only flag. Got: {commandLine}");
    }

    #endregion
}
