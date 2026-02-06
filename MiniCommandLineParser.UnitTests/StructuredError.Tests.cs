namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for structured error handling in parsing.
/// </summary>
[TestClass]
public sealed class StructuredErrorTests
{
    [TestMethod]
    public void ParseError_Constructor_SetsAllProperties()
    {
        // Arrange & Act
        var error = new ParseError("input", ParseErrorType.MissingRequired, "Option --input is required");

        // Assert
        Assert.AreEqual("input", error.OptionName);
        Assert.AreEqual(ParseErrorType.MissingRequired, error.Type);
        Assert.AreEqual("Option --input is required", error.Message);
    }

    [TestMethod]
    public void ParseError_ToString_ReturnsMessage()
    {
        // Arrange
        var error = new ParseError("test", ParseErrorType.General, "Test error message");

        // Act
        var result = error.ToString();

        // Assert
        Assert.AreEqual("Test error message", result);
    }

    [TestMethod]
    public void ParserResult_Errors_IsEmptyInitially()
    {
        // Arrange & Act
        var result = new ParserResult<BasicOptions>(null);

        // Assert
        Assert.IsNotNull(result.Errors);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void ParserResult_AppendError_WithType_AddsStructuredError()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("input", ParseErrorType.MissingRequired, "Option is required");

        // Assert
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("input", result.Errors[0].OptionName);
        Assert.AreEqual(ParseErrorType.MissingRequired, result.Errors[0].Type);
        Assert.AreEqual("Option is required", result.Errors[0].Message);
    }

    [TestMethod]
    public void ParserResult_AppendError_WithoutType_AddsGeneralError()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("Some error occurred");

        // Assert
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("", result.Errors[0].OptionName);
        Assert.AreEqual(ParseErrorType.General, result.Errors[0].Type);
        Assert.AreEqual("Some error occurred", result.Errors[0].Message);
    }

    [TestMethod]
    public void ParserResult_MultipleErrors_CollectsAll()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("input", ParseErrorType.MissingRequired, "Input is required");
        result.AppendError("output", ParseErrorType.InvalidValue, "Invalid output value");
        result.AppendError("unknown", ParseErrorType.UnknownOption, "Unknown option");

        // Assert
        Assert.AreEqual(3, result.Errors.Count);
        Assert.AreEqual(ParseErrorType.MissingRequired, result.Errors[0].Type);
        Assert.AreEqual(ParseErrorType.InvalidValue, result.Errors[1].Type);
        Assert.AreEqual(ParseErrorType.UnknownOption, result.Errors[2].Type);
    }

    [TestMethod]
    public void Parse_MissingRequired_ReturnsStructuredError()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<RequiredOptions>("");

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Count > 0);
        Assert.IsTrue(result.Errors.Any(e => e.Type == ParseErrorType.MissingRequired));
    }

    [TestMethod]
    public void Parse_UnknownOption_ReturnsStructuredError()
    {
        // Arrange
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });

        // Act
        var result = parser.Parse<BasicOptions>("--unknown-option value");

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Count > 0);
        Assert.IsTrue(result.Errors.Any(e => e.Type == ParseErrorType.UnknownOption));
    }

    [TestMethod]
    public void Parse_InvalidValue_ReturnsStructuredError()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--count not-a-number");

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Count > 0);
        Assert.IsTrue(result.Errors.Any(e => e.Type == ParseErrorType.InvalidValue));
    }

    [TestMethod]
    public void Parse_ErrorMessage_ContainsAllErrors()
    {
        // Arrange
        var result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("input", ParseErrorType.MissingRequired, "Error 1");
        result.AppendError("output", ParseErrorType.InvalidValue, "Error 2");

        // Assert
        var errorMessage = result.ErrorMessage;
        Assert.IsTrue(errorMessage.Contains("Error 1"));
        Assert.IsTrue(errorMessage.Contains("Error 2"));
    }

    [TestMethod]
    public void ParseErrorType_AllTypesAreDefined()
    {
        // Assert - verify all expected error types exist
        Assert.AreEqual(ParseErrorType.MissingRequired, (ParseErrorType)0);
        Assert.AreEqual(ParseErrorType.UnknownOption, (ParseErrorType)1);
        Assert.AreEqual(ParseErrorType.InvalidValue, (ParseErrorType)2);
        Assert.AreEqual(ParseErrorType.General, (ParseErrorType)3);
    }

    [TestMethod]
    public void Parse_Success_HasNoErrors()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void IParserResult_Errors_ReturnsReadOnlyList()
    {
        // Arrange
        IParserResult result = new ParserResult<BasicOptions>(null);

        // Act
        result.AppendError("test", ParseErrorType.General, "Test error");

        // Assert
        Assert.IsInstanceOfType(result.Errors, typeof(IReadOnlyList<ParseError>));
        Assert.AreEqual(1, result.Errors.Count);
    }
}
