namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for positional collection (variadic positional) argument parsing.
/// Covers the ProcessPositionalValue() fix that adds collection type support,
/// and the FindLastPositionalProperty() fallback for variadic positional arguments.
/// </summary>
[TestClass]
public sealed class PositionalCollectionTests
{
    #region Single Positional Collection (BindingList)

    [TestMethod]
    [Description("Single positional BindingList<string> with one value")]
    public void Parse_PositionalBindingList_SingleValue_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionOptions>("file1.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(1, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
    }

    [TestMethod]
    [Description("Single positional BindingList<string> with multiple values")]
    public void Parse_PositionalBindingList_MultipleValues_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionOptions>("file1.txt file2.txt file3.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    [Description("Single positional BindingList<string> with quoted values")]
    public void Parse_PositionalBindingList_QuotedValues_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionOptions>("\"path with spaces/file1.txt\" \"another path/file2.txt\"");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("path with spaces/file1.txt", result.Value.Files[0]);
        Assert.AreEqual("another path/file2.txt", result.Value.Files[1]);
    }

    #endregion

    #region Single Positional Collection (List<int>)

    [TestMethod]
    [Description("Single positional List<int> with multiple values")]
    public void Parse_PositionalListInt_MultipleValues_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalIntCollectionOptions>("1 2 3 4 5");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(5, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, result.Value.Numbers);
    }

    [TestMethod]
    [Description("Single positional List<int> with single value")]
    public void Parse_PositionalListInt_SingleValue_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalIntCollectionOptions>("42");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(1, result.Value.Numbers.Count);
        Assert.AreEqual(42, result.Value.Numbers[0]);
    }

    #endregion

    #region Scalar + Variadic Positional Collection (FindLastPositionalProperty)

    [TestMethod]
    [Description("Scalar positional followed by variadic BindingList - basic case")]
    public void Parse_ScalarThenVariadicBindingList_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<CommandWithPositionalCollectionOptions>("add file1.txt file2.txt file3.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("add", result.Value.Command);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
    }

    [TestMethod]
    [Description("Scalar positional followed by variadic BindingList - single file")]
    public void Parse_ScalarThenVariadicBindingList_SingleFile_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<CommandWithPositionalCollectionOptions>("commit file.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("commit", result.Value.Command);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(1, result.Value.Files.Count);
        Assert.AreEqual("file.txt", result.Value.Files[0]);
    }

    [TestMethod]
    [Description("Scalar positional followed by variadic BindingList - only command, no files")]
    public void Parse_ScalarThenVariadicBindingList_OnlyCommand_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<CommandWithPositionalCollectionOptions>("status");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("status", result.Value.Command);
        // Files should be null since no file arguments were given
        Assert.IsNull(result.Value.Files);
    }

    [TestMethod]
    [Description("Scalar positional followed by variadic List<int> - multiple numbers")]
    public void Parse_ScalarThenVariadicListInt_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<CommandWithPositionalIntCollectionOptions>("sum 10 20 30 40");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("sum", result.Value.Operation);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(4, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 10, 20, 30, 40 }, result.Value.Numbers);
    }

    [TestMethod]
    [Description("Scalar positional followed by variadic List<int> - IEnumerable input")]
    public void Parse_ScalarThenVariadicListInt_EnumerableInput_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<CommandWithPositionalIntCollectionOptions>(
            new[] { "avg", "100", "200", "300" });

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("avg", result.Value.Operation);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(3, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 100, 200, 300 }, result.Value.Numbers);
    }

    #endregion

    #region Positional Collection with Named Options

    [TestMethod]
    [Description("Positional collection followed by named boolean flag")]
    public void Parse_PositionalCollectionThenNamedFlag_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionWithNamedOptions>("file1.txt file2.txt --verbose");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.IsTrue(result.Value.Verbose);
    }

    [TestMethod]
    [Description("Positional collection with named string option")]
    public void Parse_PositionalCollectionWithNamedString_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionWithNamedOptions>("a.txt b.txt --output result.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    [Description("Named flag before positional collection")]
    public void Parse_NamedFlagBeforePositionalCollection_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalCollectionWithNamedOptions>("--verbose file1.txt file2.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
    }

    [TestMethod]
    [Description("Full mixed: scalar + variadic collection + named options")]
    public void Parse_FullMixed_ScalarVariadicNamed_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<FullMixedPositionalCollectionOptions>("add file1.txt file2.txt -r");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("add", result.Value.Command);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.IsTrue(result.Value.Recursive);
    }

    [TestMethod]
    [Description("Full mixed: scalar + variadic collection + multiple named options")]
    public void Parse_FullMixed_AllOptions_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<FullMixedPositionalCollectionOptions>(
            "commit file1.txt file2.txt file3.txt -r -m \"initial commit\"");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("commit", result.Value.Command);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.IsTrue(result.Value.Recursive);
        Assert.AreEqual("initial commit", result.Value.Message);
    }

    [TestMethod]
    [Description("Full mixed: named options interspersed - named flag before positional files")]
    public void Parse_FullMixed_NamedBeforePositionalFiles_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<FullMixedPositionalCollectionOptions>(
            "add -r file1.txt file2.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("add", result.Value.Command);
        Assert.IsTrue(result.Value.Recursive);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
    }

    [TestMethod]
    [Description("Full mixed: only scalar positional, no collection items")]
    public void Parse_FullMixed_OnlyScalar_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<FullMixedPositionalCollectionOptions>("status");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("status", result.Value.Command);
        Assert.IsNull(result.Value.Files);
        Assert.IsFalse(result.Value.Recursive);
    }

    [TestMethod]
    [Description("Full mixed with IEnumerable<string> input")]
    public void Parse_FullMixed_EnumerableInput_ParsesCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<FullMixedPositionalCollectionOptions>(
            new[] { "add", "a.txt", "b.txt", "c.txt", "--recursive", "--message", "test msg" });

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("add", result.Value.Command);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
        Assert.IsTrue(result.Value.Recursive);
        Assert.AreEqual("test msg", result.Value.Message);
    }

    #endregion

    #region Error Handling

    [TestMethod]
    [Description("Positional int collection with invalid value should fail")]
    public void Parse_PositionalIntCollection_InvalidValue_ReturnsError()
    {
        var parser = new Parser();

        var result = parser.Parse<PositionalIntCollectionOptions>("1 2 abc 4");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
        Assert.IsTrue(result.Errors.Count > 0);
    }

    [TestMethod]
    [Description("Extra positional args beyond collection with IgnoreUnknownArguments=false on non-collection type")]
    public void Parse_ExtraPositionalArgs_NoCollection_ReturnsError()
    {
        // CloneCommandOptions has 2 scalar positional args, no collection
        var parser = new Parser(new ParserSettings { IgnoreUnknownArguments = false });

        var result = parser.Parse<CloneCommandOptions>("clone url extra_arg");

        Assert.AreEqual(ParserResultType.NotParsed, result.Result);
    }

    #endregion

    #region Existing Instance

    [TestMethod]
    [Description("Positional collection parsed into existing instance")]
    public void Parse_PositionalCollection_ExistingInstance_ParsesCorrectly()
    {
        var parser = new Parser();
        var instance = new CommandWithPositionalCollectionOptions();

        var result = parser.Parse("add file1.txt file2.txt", instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("add", instance.Command);
        Assert.IsNotNull(instance.Files);
        Assert.AreEqual(2, instance.Files.Count);
        Assert.AreEqual("file1.txt", instance.Files[0]);
        Assert.AreEqual("file2.txt", instance.Files[1]);
    }

    [TestMethod]
    [Description("Positional collection parsed into existing instance with IEnumerable args")]
    public void Parse_PositionalCollection_ExistingInstance_EnumerableArgs_ParsesCorrectly()
    {
        var parser = new Parser();
        var instance = new FullMixedPositionalCollectionOptions();

        var result = parser.Parse(new[] { "commit", "a.txt", "b.txt", "-r" }, instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.AreEqual("commit", instance.Command);
        Assert.IsNotNull(instance.Files);
        Assert.AreEqual(2, instance.Files.Count);
        Assert.IsTrue(instance.Recursive);
    }

    #endregion
}
