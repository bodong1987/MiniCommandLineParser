namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class ArgumentsDictionaryTests
{
    #region Basic Named Arguments

    [TestMethod]
    public void Arguments_RecognizedNamedOption_RecordedInDictionary()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --count 10");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("name"));
        Assert.IsTrue(result.HasArgument("count"));
        Assert.IsTrue(result.TryGetArgumentValue("name", out var nameVal));
        Assert.AreEqual("John", nameVal);
        Assert.IsTrue(result.TryGetArgumentValue("count", out var countVal));
        Assert.AreEqual("10", countVal);
    }

    [TestMethod]
    public void Arguments_BooleanFlag_RecordedAsNull()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("verbose"));
        Assert.IsTrue(result.TryGetArgumentValue("verbose", out var val));
        Assert.IsNull(val);
    }

    [TestMethod]
    public void Arguments_BooleanFlagWithExplicitTrue_RecordedAsTrue()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose=true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("verbose"));
        Assert.IsTrue(result.TryGetArgumentValue("verbose", out var val));
        Assert.AreEqual("true", val);
    }

    [TestMethod]
    public void Arguments_BooleanFlagWithSpaceTrue_RecordedAsTrue()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--verbose true");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("verbose"));
        Assert.IsTrue(result.TryGetArgumentValue("verbose", out var val));
        Assert.AreEqual("true", val);
    }

    [TestMethod]
    public void Arguments_EqualsSyntax_RecordedCorrectly()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name=John --count=10");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.TryGetArgumentValue("name", out var nameVal));
        Assert.AreEqual("John", nameVal);
        Assert.IsTrue(result.TryGetArgumentValue("count", out var countVal));
        Assert.AreEqual("10", countVal);
    }

    [TestMethod]
    public void Arguments_ShortOption_RecordedWithShortName()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("-n John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("n"));
        Assert.IsTrue(result.TryGetArgumentValue("n", out var val));
        Assert.AreEqual("John", val);
    }

    #endregion

    #region Unrecognized Arguments (IgnoreUnknownArguments=true)

    [TestMethod]
    public void Arguments_UnrecognizedOption_RecordedWhenIgnoreUnknown()
    {
        // Arrange: default IgnoreUnknownArguments is true
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --unknown-option SomeValue --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("unknown-option"));
        Assert.IsTrue(result.TryGetArgumentValue("unknown-option", out var val));
        Assert.AreEqual("SomeValue", val);
        // Recognized args should also be there
        Assert.IsTrue(result.HasArgument("name"));
        Assert.IsTrue(result.HasArgument("verbose"));
    }

    [TestMethod]
    public void Arguments_UnrecognizedOptionWithEquals_RecordedWhenIgnoreUnknown()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --p4-assets-username=build-bot");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("p4-assets-username"));
        Assert.IsTrue(result.TryGetArgumentValue("p4-assets-username", out var val));
        Assert.AreEqual("build-bot", val);
    }

    [TestMethod]
    public void Arguments_MultipleUnrecognizedOptions_AllRecorded()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--p4-assets-username build-bot --p4-assets-password secret123 --svn-username admin");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.TryGetArgumentValue("p4-assets-username", out var user1));
        Assert.AreEqual("build-bot", user1);
        Assert.IsTrue(result.TryGetArgumentValue("p4-assets-password", out var pass1));
        Assert.AreEqual("secret123", pass1);
        Assert.IsTrue(result.TryGetArgumentValue("svn-username", out var user2));
        Assert.AreEqual("admin", user2);
    }

    [TestMethod]
    public void Arguments_UnrecognizedBooleanFlag_RecordedAsNull()
    {
        // Arrange
        var parser = new Parser();

        // Act: --dry-run is not recognized, followed by another option so it has no value
        var result = parser.Parse<BasicOptions>("--dry-run --name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("dry-run"));
    }

    #endregion

    #region Case Insensitivity

    [TestMethod]
    public void Arguments_CaseInsensitiveLookup()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--Name John");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("name"));
        Assert.IsTrue(result.HasArgument("NAME"));
        Assert.IsTrue(result.HasArgument("Name"));
        Assert.IsTrue(result.TryGetArgumentValue("NAME", out var val));
        Assert.AreEqual("John", val);
    }

    #endregion

    #region Positional Arguments

    [TestMethod]
    public void Arguments_PositionalArgs_RecordedWithIndexKey()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://example.com --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.HasArgument("__positional_0"));
        Assert.IsTrue(result.HasArgument("__positional_1"));
        Assert.IsTrue(result.TryGetArgumentValue("__positional_0", out var cmd));
        Assert.AreEqual("clone", cmd);
        Assert.IsTrue(result.TryGetArgumentValue("__positional_1", out var url));
        Assert.AreEqual("https://example.com", url);
        Assert.IsTrue(result.HasArgument("verbose"));
    }

    #endregion

    #region Arguments Dictionary Property

    [TestMethod]
    public void Arguments_DictionaryProperty_ContainsAllArguments()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --verbose --count 5");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        var args = result.Arguments;
        Assert.IsTrue(args.ContainsKey("name"));
        Assert.IsTrue(args.ContainsKey("verbose"));
        Assert.IsTrue(args.ContainsKey("count"));
    }

    [TestMethod]
    public void Arguments_DictionaryProperty_ContainsBothRecognizedAndUnrecognized()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John --custom-arg CustomVal");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        var args = result.Arguments;
        Assert.IsTrue(args.ContainsKey("name"));
        Assert.IsTrue(args.ContainsKey("custom-arg"));
        Assert.AreEqual("John", args["name"]);
        Assert.AreEqual("CustomVal", args["custom-arg"]);
    }

    #endregion

    #region Non-Existent Argument Queries

    [TestMethod]
    public void TryGetArgumentValue_NonExistent_ReturnsFalse()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John");

        // Assert
        Assert.IsFalse(result.TryGetArgumentValue("nonexistent", out var val));
        Assert.IsNull(val);
    }

    [TestMethod]
    public void HasArgument_NonExistent_ReturnsFalse()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name John");

        // Assert
        Assert.IsFalse(result.HasArgument("nonexistent"));
    }

    #endregion

    #region Empty Command Line

    [TestMethod]
    public void Arguments_EmptyCommandLine_EmptyDictionary()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual(0, result.Arguments.Count);
    }

    #endregion

    #region Later Argument Overrides Earlier (Same Name)

    [TestMethod]
    public void Arguments_DuplicateNamedOption_LaterOverridesEarlier()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<BasicOptions>("--name First --name Second");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsTrue(result.TryGetArgumentValue("name", out var val));
        Assert.AreEqual("Second", val);
    }

    #endregion

    #region Real-World Scenario: Per-Mount Credential Override

    [TestMethod]
    public void Arguments_PerMountCredentials_CanBeQueried()
    {
        // Simulates: upm clone project.mixvcs ./out --p4-assets-username=build-bot --p4-assets-password=secret --svn-username=admin --verbose
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.Parse<CloneCommandOptions>("clone https://example.com --p4-assets-username=build-bot --p4-assets-password=secret --username=global-user --verbose");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.AreEqual("clone", result.Value!.Command);
        Assert.AreEqual("https://example.com", result.Value.Url);
        Assert.IsTrue(result.Value.Verbose);

        // Per-mount credential query
        Assert.IsTrue(result.TryGetArgumentValue("p4-assets-username", out var perMountUser));
        Assert.AreEqual("build-bot", perMountUser);
        Assert.IsTrue(result.TryGetArgumentValue("p4-assets-password", out var perMountPass));
        Assert.AreEqual("secret", perMountPass);

        // Global credential query
        Assert.IsTrue(result.TryGetArgumentValue("username", out var globalUser));
        Assert.AreEqual("global-user", globalUser);
    }

    [TestMethod]
    public void Arguments_CredentialPriority_PerMountOverGlobal()
    {
        // Simulates the priority logic: per-mount > global > default
        var parser = new Parser();
        var result = parser.Parse<BasicOptions>("--username=global-user --p4-assets-username=per-mount-user --verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);

        // Simulate mount "p4-assets" - check per-mount first
        var mountId = "p4-assets";
        string? effectiveUsername;
        if (result.TryGetArgumentValue($"{mountId}-username", out var perMount) && perMount != null)
        {
            effectiveUsername = perMount;
        }
        else if (result.TryGetArgumentValue("username", out var global) && global != null)
        {
            effectiveUsername = global;
        }
        else
        {
            effectiveUsername = null;
        }

        Assert.AreEqual("per-mount-user", effectiveUsername);

        // Simulate mount "svn-docs" - no per-mount, falls back to global
        mountId = "svn-docs";
        if (result.TryGetArgumentValue($"{mountId}-username", out perMount) && perMount != null)
        {
            effectiveUsername = perMount;
        }
        else if (result.TryGetArgumentValue("username", out var global2) && global2 != null)
        {
            effectiveUsername = global2;
        }
        else
        {
            effectiveUsername = null;
        }

        Assert.AreEqual("global-user", effectiveUsername);
    }

    #endregion

    #region IParserResult Interface Access

    [TestMethod]
    public void Arguments_AccessViaInterface_Works()
    {
        // Arrange
        var parser = new Parser();

        // Act
        IParserResult result = parser.Parse<BasicOptions>("--name John --custom-val Test");

        // Assert
        Assert.IsTrue(result.HasArgument("name"));
        Assert.IsTrue(result.HasArgument("custom-val"));
        Assert.IsTrue(result.TryGetArgumentValue("name", out var val));
        Assert.AreEqual("John", val);
    }

    #endregion
}