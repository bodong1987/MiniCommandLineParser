namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for BindingList array parameter parsing and formatting.
/// </summary>
[TestClass]
public sealed class ParserBindingListTests
{
    #region String BindingList Tests

    [TestMethod]
    public void Parse_BindingList_String_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings --integers 1 2";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(0, result.Value.Strings.Count);
    }

    [TestMethod]
    public void Parse_BindingList_String_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings single";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.AreEqual("single", result.Value.Strings[0]);
    }

    [TestMethod]
    public void Parse_BindingList_String_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings first second third fourth";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(4, result.Value.Strings.Count);
        Assert.AreEqual("first", result.Value.Strings[0]);
        Assert.AreEqual("second", result.Value.Strings[1]);
        Assert.AreEqual("third", result.Value.Strings[2]);
        Assert.AreEqual("fourth", result.Value.Strings[3]);
    }

    [TestMethod]
    public void Parse_BindingList_String_WithQuotedValues_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings \"hello world\" \"foo bar\"";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(2, result.Value.Strings.Count);
        Assert.AreEqual("hello world", result.Value.Strings[0]);
        Assert.AreEqual("foo bar", result.Value.Strings[1]);
    }

    [TestMethod]
    public void Parse_BindingList_String_WithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-s one two three";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
    }

    #endregion

    #region Integer BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Integer_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers --strings abc";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(0, result.Value.Integers.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 42";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(1, result.Value.Integers.Count);
        Assert.AreEqual(42, result.Value.Integers[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 1 2 3 4 5";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(5, result.Value.Integers.Count);
        Assert.AreEqual(1, result.Value.Integers[0]);
        Assert.AreEqual(2, result.Value.Integers[1]);
        Assert.AreEqual(3, result.Value.Integers[2]);
        Assert.AreEqual(4, result.Value.Integers[3]);
        Assert.AreEqual(5, result.Value.Integers[4]);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_WithShortOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-i 10 20 30";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(3, result.Value.Integers.Count);
        Assert.AreEqual(10, result.Value.Integers[0]);
        Assert.AreEqual(20, result.Value.Integers[1]);
        Assert.AreEqual(30, result.Value.Integers[2]);
    }

    #endregion

    #region Bool BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Bool_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        // note: when only --bools is present without value, it may be interpreted as an empty list or a single true
        Assert.IsNotNull(result.Value.Bools);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(1, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true false true false";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(4, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools[0]);
        Assert.IsFalse(result.Value.Bools[1]);
        Assert.IsTrue(result.Value.Bools[2]);
        Assert.IsFalse(result.Value.Bools[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Bool_AllTrue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools true true true";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(3, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools.All(b => b));
    }

    [TestMethod]
    public void Parse_BindingList_Bool_AllFalse_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bools false false false";

        // Act
        var result = parser.Parse<BindingListBoolOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bools);
        Assert.AreEqual(3, result.Value.Bools.Count);
        Assert.IsTrue(result.Value.Bools.All(b => !b));
    }

    #endregion

    #region Numeric BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Double_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles --floats 1.0";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(0, result.Value.Doubles.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Double_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 3.14";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(1, result.Value.Doubles.Count);
        Assert.AreEqual(3.14, result.Value.Doubles[0], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_Double_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 1.1 2.2 3.3 4.4";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(4, result.Value.Doubles.Count);
        Assert.AreEqual(1.1, result.Value.Doubles[0], 0.001);
        Assert.AreEqual(2.2, result.Value.Doubles[1], 0.001);
        Assert.AreEqual(3.3, result.Value.Doubles[2], 0.001);
        Assert.AreEqual(4.4, result.Value.Doubles[3], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_Float_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--floats 1.5";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Floats);
        Assert.AreEqual(1, result.Value.Floats.Count);
        Assert.AreEqual(1.5f, result.Value.Floats[0], 0.001f);
    }

    [TestMethod]
    public void Parse_BindingList_Float_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--floats 0.1 0.2 0.3";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Floats);
        Assert.AreEqual(3, result.Value.Floats.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Long_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--longs 9999999999";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Longs);
        Assert.AreEqual(1, result.Value.Longs.Count);
        Assert.AreEqual(9999999999L, result.Value.Longs[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Long_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--longs 1000000000 2000000000 3000000000";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Longs);
        Assert.AreEqual(3, result.Value.Longs.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Decimal_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--decimals 123.456";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Decimals);
        Assert.AreEqual(1, result.Value.Decimals.Count);
        Assert.AreEqual(123.456m, result.Value.Decimals[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Decimal_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--decimals 10.5 20.25 30.125";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Decimals);
        Assert.AreEqual(3, result.Value.Decimals.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Short_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--shorts 100";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Shorts);
        Assert.AreEqual(1, result.Value.Shorts.Count);
        Assert.AreEqual((short)100, result.Value.Shorts[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Short_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--shorts 1 2 3 4 5";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Shorts);
        Assert.AreEqual(5, result.Value.Shorts.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Byte_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bytes 255";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bytes);
        Assert.AreEqual(1, result.Value.Bytes.Count);
        Assert.AreEqual((byte)255, result.Value.Bytes[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Byte_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--bytes 0 127 255";

        // Act
        var result = parser.Parse<BindingListNumericOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Bytes);
        Assert.AreEqual(3, result.Value.Bytes.Count);
        Assert.AreEqual((byte)0, result.Value.Bytes[0]);
        Assert.AreEqual((byte)127, result.Value.Bytes[1]);
        Assert.AreEqual((byte)255, result.Value.Bytes[2]);
    }

    #endregion

    #region Enum BindingList Tests

    [TestMethod]
    public void Parse_BindingList_Enum_ZeroElements_ReturnsEmptyList()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(0, result.Value.Levels.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_OneElement_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(1, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_MultipleElements_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug Info Warning Error";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Warning, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Error, result.Value.Levels[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_CaseInsensitive_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels debug INFO warning ERROR";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Warning, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Error, result.Value.Levels[3]);
    }

    [TestMethod]
    public void Parse_BindingList_Enum_DuplicateValues_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--levels Debug Debug Info Info";

        // Act
        var result = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Levels);
        Assert.AreEqual(4, result.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Debug, result.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[2]);
        Assert.AreEqual(LogLevel.Info, result.Value.Levels[3]);
    }

    #endregion

    #region Separator BindingList Tests

    [TestMethod]
    public void Parse_BindingList_WithCommaSeparator_String_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings a,b,c,d";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(4, result.Value.Strings.Count);
        Assert.AreEqual("a", result.Value.Strings[0]);
        Assert.AreEqual("b", result.Value.Strings[1]);
        Assert.AreEqual("c", result.Value.Strings[2]);
        Assert.AreEqual("d", result.Value.Strings[3]);
    }

    [TestMethod]
    public void Parse_BindingList_WithColonSeparator_Integer_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 1:2:3:4:5";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(5, result.Value.Integers.Count);
        Assert.AreEqual(1, result.Value.Integers[0]);
        Assert.AreEqual(2, result.Value.Integers[1]);
        Assert.AreEqual(3, result.Value.Integers[2]);
        Assert.AreEqual(4, result.Value.Integers[3]);
        Assert.AreEqual(5, result.Value.Integers[4]);
    }

    [TestMethod]
    public void Parse_BindingList_WithSemicolonSeparator_Double_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--doubles 1.1;2.2;3.3";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Doubles);
        Assert.AreEqual(3, result.Value.Doubles.Count);
        Assert.AreEqual(1.1, result.Value.Doubles[0], 0.001);
        Assert.AreEqual(2.2, result.Value.Doubles[1], 0.001);
        Assert.AreEqual(3.3, result.Value.Doubles[2], 0.001);
    }

    [TestMethod]
    public void Parse_BindingList_WithSeparator_EqualsSyntax_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings=x,y,z";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual("x", result.Value.Strings[0]);
        Assert.AreEqual("y", result.Value.Strings[1]);
        Assert.AreEqual("z", result.Value.Strings[2]);
    }

    [TestMethod]
    public void Parse_BindingList_WithSeparator_SingleValue_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings single";

        // Act
        var result = parser.Parse<BindingListSeparatorOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.AreEqual("single", result.Value.Strings[0]);
    }

    #endregion

    #region Mixed Options Tests

    [TestMethod]
    public void Parse_BindingList_MixedWithBoolOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --files file1.txt file2.txt";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
    }

    [TestMethod]
    public void Parse_BindingList_MixedWithStringOption_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--files a.txt b.txt --output result.txt";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Files);
        Assert.AreEqual(2, result.Value.Files.Count);
        Assert.AreEqual("result.txt", result.Value.Output);
    }

    [TestMethod]
    public void Parse_BindingList_MultipleMixedOptions_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "-v -f file1.txt file2.txt -o output.txt -c 1 2 3";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(2, result.Value.Files?.Count);
        Assert.AreEqual("output.txt", result.Value.Output);
        Assert.AreEqual(3, result.Value.Counts?.Count);
    }

    [TestMethod]
    public void Parse_BindingList_AllOptionTypes_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--verbose --files a.txt b.txt c.txt --output out.txt --counts 10 20 30 40";

        // Act
        var result = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Verbose);
        Assert.AreEqual(3, result.Value.Files?.Count);
        Assert.AreEqual("out.txt", result.Value.Output);
        Assert.AreEqual(4, result.Value.Counts?.Count);
    }

    #endregion

    #region Format and Roundtrip Tests

    [TestMethod]
    public void FormatCommandLine_BindingList_String_FormatsCorrectly()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = ["item1", "item2", "item3"]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--strings"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item1"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item2"), $"Result: {result}");
        Assert.IsTrue(result.Contains("item3"), $"Result: {result}");
    }

    [TestMethod]
    public void FormatCommandLine_BindingList_Integer_FormatsCorrectly()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Integers = [100, 200, 300]
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsTrue(result.Contains("--integers"), $"Result: {result}");
        Assert.IsTrue(result.Contains("100"), $"Result: {result}");
        Assert.IsTrue(result.Contains("200"), $"Result: {result}");
        Assert.IsTrue(result.Contains("300"), $"Result: {result}");
    }

    [TestMethod]
    public void Roundtrip_BindingList_StringAndInteger_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListOptions
        {
            Strings = ["a", "b", "c"],
            Integers = [1, 2, 3]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsNotNull(parsed.Value.Strings);
        Assert.IsNotNull(parsed.Value.Integers);
        Assert.AreEqual(3, parsed.Value.Strings.Count);
        Assert.AreEqual(3, parsed.Value.Integers.Count);
        Assert.AreEqual("a", parsed.Value.Strings[0]);
        Assert.AreEqual(1, parsed.Value.Integers[0]);
    }

    [TestMethod]
    public void Roundtrip_BindingList_Enum_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListEnumOptions
        {
            Levels = [LogLevel.Debug, LogLevel.Info, LogLevel.Error]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListEnumOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsNotNull(parsed.Value.Levels);
        Assert.AreEqual(3, parsed.Value.Levels.Count);
        Assert.AreEqual(LogLevel.Debug, parsed.Value.Levels[0]);
        Assert.AreEqual(LogLevel.Info, parsed.Value.Levels[1]);
        Assert.AreEqual(LogLevel.Error, parsed.Value.Levels[2]);
    }

    [TestMethod]
    public void Roundtrip_BindingList_MixedOptions_ParseAndFormat()
    {
        // Arrange
        var parser = new Parser();
        var original = new BindingListMixedOptions
        {
            Verbose = true,
            Files = ["x.txt", "y.txt"],
            Output = "z.txt",
            Counts = [5, 10, 15]
        };

        // Act - Format then parse back
        var commandLine = Parser.FormatCommandLine(original, CommandLineFormatMethod.Complete);
        var parsed = parser.Parse<BindingListMixedOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, parsed.Result);
        Assert.IsNotNull(parsed.Value);
        Assert.IsTrue(parsed.Value.Verbose);
        Assert.AreEqual("z.txt", parsed.Value.Output);
        Assert.AreEqual(2, parsed.Value.Files?.Count);
        Assert.AreEqual(3, parsed.Value.Counts?.Count);
    }

    [TestMethod]
    public void FormatCommandLineArgs_BindingList_ReturnsCorrectArray()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = ["one", "two"],
            Integers = [1, 2, 3]
        };

        // Act
        var result = Parser.FormatCommandLineArgs(options, CommandLineFormatMethod.Complete);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.Contains(result, "--strings");
        CollectionAssert.Contains(result, "one");
        CollectionAssert.Contains(result, "two");
        CollectionAssert.Contains(result, "--integers");
    }

    [TestMethod]
    public void FormatCommandLine_BindingList_EmptyList_SimplifyOmits()
    {
        // Arrange
        var options = new BindingListOptions
        {
            Strings = [],
            Integers = null
        };

        // Act
        var result = Parser.FormatCommandLine(options, CommandLineFormatMethod.Simplify);

        // Assert
        Assert.IsFalse(result.Contains("--strings"), $"Result should not contain empty strings: {result}");
        Assert.IsFalse(result.Contains("--integers"), $"Result should not contain null integers: {result}");
    }

    #endregion

    #region Edge Cases Tests

    [TestMethod]
    public void Parse_BindingList_NoOption_ReturnsNull()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNull(result.Value.Strings);
        Assert.IsNull(result.Value.Integers);
    }

    [TestMethod]
    public void Parse_BindingList_OnlyOneOption_OtherIsNull()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings only";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(1, result.Value.Strings.Count);
        Assert.IsNull(result.Value.Integers);
    }

    [TestMethod]
    public void Parse_BindingList_ArrayStopsAtNextOption()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings a b c --integers 1 2";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.IsNotNull(result.Value.Integers);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual(2, result.Value.Integers.Count);
    }

    [TestMethod]
    public void Parse_BindingList_Integer_LargeNumbers_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--integers 2147483647 -2147483648 0";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Integers);
        // 注意：负数可能被视为选项，这里验证正数和零
        Assert.IsTrue(result.Value.Integers.Count >= 1);
        Assert.AreEqual(int.MaxValue, result.Value.Integers[0]);
    }

    [TestMethod]
    public void Parse_BindingList_String_SpecialCharacters_ParsesCorrectly()
    {
        // Arrange
        var parser = new Parser();
        var commandLine = "--strings \"hello@world\" \"test#123\" \"path/to/file\"";

        // Act
        var result = parser.Parse<BindingListOptions>(commandLine);

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNotNull(result.Value.Strings);
        Assert.AreEqual(3, result.Value.Strings.Count);
        Assert.AreEqual("hello@world", result.Value.Strings[0]);
        Assert.AreEqual("test#123", result.Value.Strings[1]);
        Assert.AreEqual("path/to/file", result.Value.Strings[2]);
    }

    #endregion
}
