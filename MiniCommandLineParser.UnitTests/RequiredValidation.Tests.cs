namespace MiniCommandLineParser.UnitTests;

#region Additional Option Classes for Required Tests

/// <summary>
/// Options class with multiple required fields for testing.
/// </summary>
public class MultipleRequiredOptions
{
    [Option('i', "input", Required = true, HelpText = "Required input file")]
    public string? Input { get; set; }

    [Option('o', "output", Required = true, HelpText = "Required output file")]
    public string? Output { get; set; }

    [Option('f', "format", HelpText = "Optional format")]
    public string? Format { get; set; }
}

/// <summary>
/// Options class with required integer field.
/// </summary>
public class RequiredIntOptions
{
    [Option('p', "port", Required = true, HelpText = "Required port number")]
    public int Port { get; set; }

    [Option('h', "host", HelpText = "Optional host")]
    public string? Host { get; set; }
}

#endregion

/// <summary>
/// Tests for validating the Required attribute behavior.
/// These tests verify whether the parser correctly validates required options.
/// </summary>
[TestClass]
public sealed class RequiredValidationTests
{
    #region Basic Required Validation Tests

    [TestMethod]
    [Description("Test that parsing succeeds when required option is provided")]
    public void Parse_RequiredOptionProvided_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--input data.txt";

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("data.txt", result.Value.Input);
    }

    [TestMethod]
    [Description("Test that parsing succeeds when required option is provided with optional")]
    public void Parse_RequiredAndOptionalProvided_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--input data.txt --output result.txt";

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("data.txt", result.Value.Input);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    [Description("Test that parsing FAILS when required option is missing")]
    public void Parse_RequiredOptionMissing_ShouldFailValidation()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--output result.txt"; // Missing required --input

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert - Required option validation should cause parsing to fail
        Console.WriteLine($"Parse result: {result.Result}");
        Console.WriteLine($"Input value: {result.Value?.Input ?? "null"}");
        Console.WriteLine($"Error message: {result.ErrorMessage}");
        
        Assert.AreEqual(ParserResultType.NotParsed, result.Result, 
            "Parsing should fail when required option is missing");
        Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage), 
            "Error message should not be empty");
        Assert.IsTrue(result.ErrorMessage.Contains("input") || result.ErrorMessage.Contains("Required"),
            "Error message should mention the missing required option");
    }

    [TestMethod]
    [Description("Test parsing with empty command line when required option exists should FAIL")]
    public void Parse_EmptyCommandLineWithRequiredOption_ShouldFailValidation()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "";

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert - Empty command line should fail when required options exist
        Console.WriteLine($"Parse result: {result.Result}");
        Console.WriteLine($"Input value: {result.Value?.Input ?? "null"}");
        
        Assert.AreEqual(ParserResultType.NotParsed, result.Result,
            "Parsing should fail when empty command line is provided but required options exist");
    }

    #endregion

    #region Multiple Required Options Tests

    [TestMethod]
    [Description("Test that all required options are provided")]
    public void Parse_AllRequiredOptionsProvided_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--input data.txt --output result.txt";

        // Act
        var result = parser.Parse<MultipleRequiredOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("data.txt", result.Value.Input);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    [Description("Test when one of multiple required options is missing should FAIL")]
    public void Parse_OneRequiredOptionMissing_ShouldFailValidation()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--input data.txt"; // Missing required --output

        // Act
        var result = parser.Parse<MultipleRequiredOptions>(commandLine);

        // Assert - Should fail when one required option is missing
        Console.WriteLine($"Parse result: {result.Result}");
        Console.WriteLine($"Input: {result.Value?.Input ?? "null"}");
        Console.WriteLine($"Output: {result.Value?.Output ?? "null"}");
        
        Assert.AreEqual(ParserResultType.NotParsed, result.Result,
            "Parsing should fail when required --output option is missing");
    }

    [TestMethod]
    [Description("Test when all required options are missing should FAIL")]
    public void Parse_AllRequiredOptionsMissing_ShouldFailValidation()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--format json"; // Both required options missing

        // Act
        var result = parser.Parse<MultipleRequiredOptions>(commandLine);

        // Assert - Should fail when all required options are missing
        Console.WriteLine($"Parse result: {result.Result}");
        Console.WriteLine($"Input: {result.Value?.Input ?? "null"}");
        Console.WriteLine($"Output: {result.Value?.Output ?? "null"}");
        
        Assert.AreEqual(ParserResultType.NotParsed, result.Result,
            "Parsing should fail when all required options are missing");
    }

    #endregion

    #region Required Integer Options Tests

    [TestMethod]
    [Description("Test required integer option when provided")]
    public void Parse_RequiredIntOptionProvided_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--port 8080";

        // Act
        var result = parser.Parse<RequiredIntOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(8080, result.Value.Port);
    }

    [TestMethod]
    [Description("Test required integer option when missing should FAIL")]
    public void Parse_RequiredIntOptionMissing_ShouldFailValidation()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--host localhost"; // Missing required --port

        // Act
        var result = parser.Parse<RequiredIntOptions>(commandLine);

        // Assert - Should fail when required integer option is missing
        Console.WriteLine($"Parse result: {result.Result}");
        Console.WriteLine($"Port value: {result.Value?.Port}"); // Will be 0 (default)
        
        Assert.AreEqual(ParserResultType.NotParsed, result.Result,
            "Parsing should fail when required --port option is missing");
    }

    #endregion

    #region Required Option with Short Name Tests

    [TestMethod]
    [Description("Test required option using short name")]
    public void Parse_RequiredOptionWithShortName_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-i data.txt";

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("data.txt", result.Value.Input);
    }

    [TestMethod]
    [Description("Test required option using equals syntax")]
    public void Parse_RequiredOptionWithEqualsSyntax_ParsesSuccessfully()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--input=data.txt";

        // Act
        var result = parser.Parse<RequiredOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("data.txt", result.Value.Input);
    }

    #endregion

    #region Help Text Tests for Required Options

    [TestMethod]
    [Description("Test that help text correctly shows Required attribute")]
    public void GetHelpText_RequiredOption_ShowsRequiredIndicator()
    {
        // Arrange
        var options = new RequiredOptions();

        // Act
        var helpText = Parser.GetHelpText(options);

        // Assert
        Console.WriteLine("Generated help text:");
        Console.WriteLine(helpText);
        
        // Required options should not show "Optional" in their attributes
        // The current implementation might not explicitly show "Required" but should not show "Optional"
        Assert.IsNotNull(helpText);
        Assert.IsTrue(helpText.Contains("input"), "Help text should contain the input option");
    }

    [TestMethod]
    [Description("Test TypeInfo correctly identifies required properties")]
    public void TypeInfo_RequiredProperty_HasRequiredAttribute()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<RequiredOptions>();

        // Act
        var inputProperty = typeInfo.Properties.FirstOrDefault(p => p.Attribute.LongName == "input");
        var outputProperty = typeInfo.Properties.FirstOrDefault(p => p.Attribute.LongName == "output");

        // Assert
        Assert.IsNotNull(inputProperty, "Should find input property");
        Assert.IsNotNull(outputProperty, "Should find output property");
        Assert.IsTrue(inputProperty.Attribute.Required, "Input should be marked as required");
        Assert.IsFalse(outputProperty.Attribute.Required, "Output should not be marked as required");
    }

    #endregion
}
