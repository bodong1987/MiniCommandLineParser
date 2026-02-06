namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class PositionalArgumentTests
{
    [TestMethod]
    public void Parse_SimplePositionalArguments_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_PositionalWithNamedOptions_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo --verbose
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_PositionalWithShortNamedOptions_ParsesCorrectly()
    {
        // Arrange: app.exe clone https://github.com/repo -v
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com/repo -v");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_MixedPositionalAndNamed_ParsesCorrectly()
    {
        // Arrange: app.exe build project --output /tmp --force
        var parser = new Parser();

        // Act
        var result = parser.Parse<MixedOptions>("build project --output /tmp --force");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("build", result.Value.Action);
        Assert.AreEqual("project", result.Value.Target);
        Assert.AreEqual("/tmp", result.Value.Output);
        Assert.IsTrue(result.Value.Force);
    }

    [TestMethod]
    public void Parse_PositionalIntegerArgument_ParsesCorrectly()
    {
        // Arrange: app.exe 10 items
        var parser = new Parser();

        // Act
        var result = parser.Parse<PositionalIntOptions>("10 items");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(10, result.Value.Count);
        Assert.AreEqual("items", result.Value.Name);
    }

    [TestMethod]
    public void Parse_PositionalWithQuotedValue_ParsesCorrectly()
    {
        // Arrange: app.exe clone "https://github.com/my repo"
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone \"https://github.com/my repo\"");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/my repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_NamedOptionsBeforePositional_StillParsesPositional()
    {
        // Arrange: app.exe --verbose clone https://github.com/repo
        // Note: This tests named options appearing before positional arguments
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose clone https://github.com/repo");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://github.com/repo", result.Value.Url);
    }

    [TestMethod]
    public void Parse_OnlyFirstPositional_LeavesOthersDefault()
    {
        // Arrange: app.exe clone
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("", result.Value.Url);  // Default value
    }

    [TestMethod]
    public void FormatCommandLine_WithPositionalArguments_FormatsCorrectly()
    {
        // Arrange
        var options = new CloneCommandOptions
        {
            Command = "clone",
            Url = "https://github.com/repo",
            Verbose = true
        };

        // Act
        var commandLine = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(commandLine.Contains("clone"));
        Assert.IsTrue(commandLine.Contains("https://github.com/repo"));
        Assert.IsTrue(commandLine.Contains("--verbose"));
    }

    [TestMethod]
    public void TypeInfo_PositionalProperties_ReturnsOnlyPositionalSortedByIndex()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var positionalProps = typeInfo.PositionalProperties;

        // Assert
        Assert.AreEqual(2, positionalProps.Length);
        Assert.AreEqual(0, positionalProps[0].Attribute.Index);
        Assert.AreEqual(1, positionalProps[1].Attribute.Index);
    }

    [TestMethod]
    public void TypeInfo_NamedProperties_ReturnsOnlyNamedOptions()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var namedProps = typeInfo.NamedProperties;

        // Assert
        Assert.AreEqual(2, namedProps.Length);
        Assert.IsFalse(namedProps[0].Attribute.IsPositional);
        Assert.IsFalse(namedProps[1].Attribute.IsPositional);
    }

    [TestMethod]
    public void OptionAttribute_IsPositional_ReturnsTrueForNonNegativeIndex()
    {
        // Arrange
        var attr1 = new OptionAttribute { Index = 0 };
        var attr2 = new OptionAttribute { Index = 5 };
        var attr3 = new OptionAttribute { Index = -1 };
        var attr4 = new OptionAttribute();  // Default

        // Assert
        Assert.IsTrue(attr1.IsPositional);
        Assert.IsTrue(attr2.IsPositional);
        Assert.IsFalse(attr3.IsPositional);
        Assert.IsFalse(attr4.IsPositional);
    }

    [TestMethod]
    public void OptionAttribute_ToString_FormatsPositionalCorrectly()
    {
        // Arrange
        var attrWithMeta = new OptionAttribute { Index = 0, MetaName = "URL" };
        var attrWithoutMeta = new OptionAttribute { Index = 1 };

        // Assert
        Assert.AreEqual("<URL>", attrWithMeta.ToString());
        Assert.AreEqual("Value[1]", attrWithoutMeta.ToString());
    }

    [TestMethod]
    public void Parse_UnknownPositionalArgument_ReturnsError()
    {
        // Arrange: app.exe clone url extra_arg (3 positional, but only 2 defined)
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com extra_arg");

        // Assert
        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Count > 0);
    }

    [TestMethod]
    public void Parse_UnknownPositionalArgument_IgnoredWhenConfigured()
    {
        // Arrange: app.exe clone url extra_arg with IgnoreUnknownArguments = true
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://github.com extra_arg");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("clone", result.Value!.Command);
        Assert.AreEqual("https://github.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_MixedPositionalAndNamedWithEquals_ParsesCorrectly()
    {
        // Arrange: app.exe build project --output=/tmp -f
        var parser = new Parser();

        // Act
        var result = parser.Parse<MixedOptions>("build project --output=/tmp -f");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("build", result.Value.Action);
        Assert.AreEqual("project", result.Value.Target);
        Assert.AreEqual("/tmp", result.Value.Output);
        Assert.IsTrue(result.Value.Force);
    }
}
