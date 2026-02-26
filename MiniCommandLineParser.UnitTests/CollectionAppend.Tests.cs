namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for the collection append behavior in ProcessValue().
/// When a named collection option appears multiple times in the command line,
/// values should be appended to the existing collection rather than replacing it.
/// </summary>
[TestClass]
public sealed class CollectionAppendTests
{
    #region Named Collection Repeated - List<T>

    [TestMethod]
    [Description("Repeated --files option should append values to the same List<string>")]
    public void Parse_RepeatedListOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("--files file1.txt --files file2.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
    }

    [TestMethod]
    [Description("Repeated --files option with multiple values each time should append all")]
    public void Parse_RepeatedListOption_MultipleValuesEachTime_AppendsAll()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("--files file1.txt file2.txt --files file3.txt file4.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(4, result.Value.Files.Count);
        Assert.AreEqual("file1.txt", result.Value.Files[0]);
        Assert.AreEqual("file2.txt", result.Value.Files[1]);
        Assert.AreEqual("file3.txt", result.Value.Files[2]);
        Assert.AreEqual("file4.txt", result.Value.Files[3]);
    }

    [TestMethod]
    [Description("Repeated --numbers int option should append values")]
    public void Parse_RepeatedIntListOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("--numbers 1 2 --numbers 3 4 5");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(5, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, result.Value.Numbers);
    }

    [TestMethod]
    [Description("Repeated short option -f should append values")]
    public void Parse_RepeatedShortOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("-f a.txt -f b.txt -f c.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    [TestMethod]
    [Description("Mix of short and long option names for same collection should append")]
    public void Parse_MixShortAndLongOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("-f a.txt --files b.txt c.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    #endregion

    #region Named Collection Repeated - BindingList<T>

    [TestMethod]
    [Description("Repeated --strings BindingList option should append values")]
    public void Parse_RepeatedBindingListOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<BindingListOptions>("--strings alpha --strings beta --strings gamma");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual("alpha", result.Value.Strings[0]);
        Assert.AreEqual("beta", result.Value.Strings[1]);
        Assert.AreEqual("gamma", result.Value.Strings[2]);
    }

    [TestMethod]
    [Description("Repeated --integers BindingList option should append values")]
    public void Parse_RepeatedBindingListIntOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<BindingListOptions>("--integers 10 20 --integers 30 40");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(4, result.Value.Integers.Count);
        Assert.AreEqual(10, result.Value.Integers[0]);
        Assert.AreEqual(20, result.Value.Integers[1]);
        Assert.AreEqual(30, result.Value.Integers[2]);
        Assert.AreEqual(40, result.Value.Integers[3]);
    }

    [TestMethod]
    [Description("Repeated BindingList option with short name should append")]
    public void Parse_RepeatedBindingListShortOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<BindingListOptions>("-s hello -s world");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(2, result.Value.Strings.Count);
        Assert.AreEqual("hello", result.Value.Strings[0]);
        Assert.AreEqual("world", result.Value.Strings[1]);
    }

    #endregion

    #region Collection Append with Mixed Non-Collection Options

    [TestMethod]
    [Description("Repeated collection with non-collection options interspersed")]
    public void Parse_RepeatedCollectionWithMixedOptions_AppendsCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<MixedArrayOptions>(
            "--files a.txt --verbose --files b.txt --output out.txt --files c.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("out.txt", result.Value.Output);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    [TestMethod]
    [Description("Repeated BindingList collection with non-collection options interspersed")]
    public void Parse_RepeatedBindingListWithMixedOptions_AppendsCorrectly()
    {
        var parser = new Parser();

        var result = parser.Parse<BindingListMixedOptions>(
            "-f x.txt -v -f y.txt -o out.txt -f z.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual("out.txt", result.Value.Output);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("x.txt", result.Value.Files[0]);
        Assert.AreEqual("y.txt", result.Value.Files[1]);
        Assert.AreEqual("z.txt", result.Value.Files[2]);
    }

    [TestMethod]
    [Description("Multiple different collections each repeated should append independently")]
    public void Parse_MultipleRepeatedCollections_AppendsIndependently()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>(
            "--files a.txt --numbers 1 2 --files b.txt c.txt --numbers 3");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.IsNotNull(result.Value.Numbers);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
        Assert.AreEqual(3, result.Value.Numbers.Count);
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, result.Value.Numbers);
    }

    #endregion

    #region Collection Append with Existing Instance

    [TestMethod]
    [Description("Repeated collection parsed into existing instance should append")]
    public void Parse_RepeatedCollection_ExistingInstance_AppendsValues()
    {
        var parser = new Parser();
        var instance = new ArrayOptions();

        var result = parser.Parse("--files a.txt --files b.txt --files c.txt", instance);

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(instance.Files);
        Assert.AreEqual(3, instance.Files.Count);
        Assert.AreEqual("a.txt", instance.Files[0]);
        Assert.AreEqual("b.txt", instance.Files[1]);
        Assert.AreEqual("c.txt", instance.Files[2]);
    }

    [TestMethod]
    [Description("Repeated collection with IEnumerable input should append")]
    public void Parse_RepeatedCollection_EnumerableInput_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>(
            new[] { "--files", "a.txt", "--files", "b.txt", "c.txt" });

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    #endregion

    #region Collection Append with Separator

    [TestMethod]
    [Description("Repeated collection with separator should append across occurrences")]
    public void Parse_RepeatedSeparatorCollection_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<SeparatorArrayOptions>("--files a.txt,b.txt --files c.txt,d.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(4, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
        Assert.AreEqual("d.txt", result.Value.Files[3]);
    }

    [TestMethod]
    [Description("Repeated BindingList with separator should append across occurrences")]
    public void Parse_RepeatedBindingListSeparatorOption_AppendsValues()
    {
        var parser = new Parser();

        var result = parser.Parse<BindingListSeparatorOptions>("--strings a,b --strings c,d,e");

        Assert.AreEqual(ParserResultType.Parsed, result.Result,
            $"Expected Parsed but got {result.Result}. Error: {result.ErrorMessage}");
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(5, result.Value.Strings.Count);
        Assert.AreEqual("a", result.Value.Strings[0]);
        Assert.AreEqual("b", result.Value.Strings[1]);
        Assert.AreEqual("c", result.Value.Strings[2]);
        Assert.AreEqual("d", result.Value.Strings[3]);
        Assert.AreEqual("e", result.Value.Strings[4]);
    }

    #endregion

    #region Edge Cases

    [TestMethod]
    [Description("Single occurrence of collection option should still work (no append needed)")]
    public void Parse_SingleCollectionOccurrence_StillWorks()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("--files a.txt b.txt c.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
    }

    [TestMethod]
    [Description("Three repetitions of same collection option should accumulate all values")]
    public void Parse_ThreeRepetitions_AccumulatesAll()
    {
        var parser = new Parser();

        var result = parser.Parse<ArrayOptions>("--files a.txt --files b.txt --files c.txt");

        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(3, result.Value.Files.Count);
        Assert.AreEqual("a.txt", result.Value.Files[0]);
        Assert.AreEqual("b.txt", result.Value.Files[1]);
        Assert.AreEqual("c.txt", result.Value.Files[2]);
    }

    #endregion
}
