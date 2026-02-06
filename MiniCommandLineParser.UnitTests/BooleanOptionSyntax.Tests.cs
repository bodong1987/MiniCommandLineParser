namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for various boolean option syntax variations.
/// Validates that --flag, --flag=true, --flag true, etc. are equivalent.
/// </summary>
[TestClass]
public sealed class BooleanOptionSyntaxTests
{
    [TestMethod]
    public void Parse_BooleanFlag_NoValue_SetsTrue()
    {
        // Arrange: --verbose (flag without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_EqualsTrue_SetsTrue()
    {
        // Arrange: --verbose=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_EqualsFalse_SetsFalse()
    {
        // Arrange: --verbose=false
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_SpaceTrue_SetsTrue()
    {
        // Arrange: --verbose true (space syntax)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_SpaceFalse_SetsFalse()
    {
        // Arrange: --verbose false (space syntax)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_NoValue_SetsTrue()
    {
        // Arrange: -v (short flag without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_EqualsTrue_SetsTrue()
    {
        // Arrange: -v=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanShortFlag_EqualsFalse_SetsFalse()
    {
        // Arrange: -v=false
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v=false");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_CaseInsensitiveTrue_SetsTrue()
    {
        // Arrange: --verbose=TRUE (case variations)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=TRUE");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_CaseInsensitiveFalse_SetsFalse()
    {
        // Arrange: --verbose=FALSE
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=FALSE");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsFalse(result.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlag_MixedCase_SetsCorrectly()
    {
        // Arrange: --verbose=True --verbose=False
        var parser = new Parser();

        // Act - True
        var resultTrue = parser.Parse<BasicOptions>("--verbose=True");
        // Act - False
        var resultFalse = parser.Parse<BasicOptions>("--verbose=False");

        // Assert
        Assert.IsTrue(resultTrue.Value!.Verbose);
        Assert.IsFalse(resultFalse.Value!.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithOtherOptions_NoValue_SetsTrue()
    {
        // Arrange: --verbose --name John (flag followed by other option)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithOtherOptions_EqualsTrue_SetsTrue()
    {
        // Arrange: --verbose=true --name John
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=true --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
    }

    [TestMethod]
    public void Parse_BooleanFlagBetweenOptions_NoValue_SetsTrue()
    {
        // Arrange: --name John --verbose --count 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose --count 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_BooleanFlagBetweenOptions_EqualsTrue_SetsTrue()
    {
        // Arrange: --name John --verbose=true --count 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose=true --count 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_MultipleBooleanSyntaxes_AllEquivalent()
    {
        // Test that all these produce the same result
        var parser = new Parser();
        var syntaxes = new[]
        {
            "--verbose",
            "--verbose=true",
            "--verbose true",
            "-v",
            "-v=true",
            "-v true"
        };

        foreach (var syntax in syntaxes)
        {
            var result = parser.Parse<BasicOptions>(syntax);
            Assert.AreEqual(ParserResultType.Parsed, result.Result, $"Failed for syntax: {syntax}");
            Assert.IsTrue(result.Value!.Verbose, $"Verbose should be true for syntax: {syntax}");
        }
    }

    [TestMethod]
    public void Parse_MultipleBooleanSyntaxesFalse_AllEquivalent()
    {
        // Test that all these produce false
        var parser = new Parser();
        var syntaxes = new[]
        {
            "--verbose=false",
            "--verbose false",
            "-v=false",
            "-v false"
        };

        foreach (var syntax in syntaxes)
        {
            var result = parser.Parse<BasicOptions>(syntax);
            Assert.AreEqual(ParserResultType.Parsed, result.Result, $"Failed for syntax: {syntax}");
            Assert.IsFalse(result.Value!.Verbose, $"Verbose should be false for syntax: {syntax}");
        }
    }

    [TestMethod]
    public void Parse_BooleanFlagWithPositionalArgs_NoValue_DoesNotConsumePositional()
    {
        // Arrange: --verbose clone https://example.com
        // The flag should NOT consume "clone" as its value
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose clone https://example.com");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://example.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_BooleanFlagWithPositionalArgs_EqualsTrue_DoesNotConsumePositional()
    {
        // Arrange: --verbose=true clone https://example.com
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("--verbose=true clone https://example.com");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("clone", result.Value.Command);
        Assert.AreEqual("https://example.com", result.Value.Url);
    }

    [TestMethod]
    public void Parse_BooleanFlagFollowedByNonBooleanValue_TreatsAsNextOption()
    {
        // Arrange: --verbose John (where John is NOT a boolean value)
        // Expected: --verbose should be true, John should be ignored or treated as unknown
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = true });

        // Act
        var result = parser.Parse<BasicOptions>("--verbose John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        // John should not affect anything (ignored as unknown positional)
    }

    [TestMethod]
    public void Parse_ShortBooleanFlagFollowedByOtherShortOption_BothParsed()
    {
        // Arrange: -v -n John -c 5
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-v -n John -c 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.Value!.Verbose);
        Assert.AreEqual("John", result.Value.Name);
        Assert.AreEqual(5, result.Value.Count);
    }

    [TestMethod]
    public void Parse_BooleanFlagAtEnd_NoValue_SetsTrue()
    {
        // Arrange: --name John --verbose (flag at end without value)
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_BooleanFlagAtEnd_EqualsTrue_SetsTrue()
    {
        // Arrange: --name John --verbose=true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("John", result.Value!.Name);
        Assert.IsTrue(result.Value.Verbose);
    }
}
