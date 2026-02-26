namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class TypeInfoTests
{
    [TestMethod]
    public void GetTypeInfo_CachesTypeInfo()
    {
        // Act
        var info1 = Parser.GetTypeInfo<BasicOptions>();
        var info2 = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.AreSame(info1, info2);
    }

    [TestMethod]
    public void TypeInfo_Properties_ReturnsOptionProperties()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.AreEqual(3, info.Properties.Length);
    }

    [TestMethod]
    public void TypeInfo_IsCommandLineObject_ReturnsTrueForOptionsClass()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.IsTrue(info.IsCommandLineObject);
    }

    [TestMethod]
    public void TypeInfo_FindShortProperty_FindsProperty()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindShortProperty("v", ignoreCase: false);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual("verbose", property.Attribute.LongName);
    }

    [TestMethod]
    public void TypeInfo_FindLongProperty_FindsProperty()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindLongProperty("verbose", ignoreCase: false);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual("v", property.Attribute.ShortName);
    }

    [TestMethod]
    public void TypeInfo_FindProperty_CaseInsensitive()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var property = info.FindLongProperty("VERBOSE", ignoreCase: true);

        // Assert
        Assert.IsNotNull(property);
    }

    [TestMethod]
    public void TypeInfo_DefaultObject_CreatesDefault()
    {
        // Act
        var info = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.IsNotNull(info.DefaultObject);
        Assert.IsInstanceOfType(info.DefaultObject, typeof(BasicOptions));
    }

    #region FindLastPositionalProperty Tests

    [TestMethod]
    [Description("FindLastPositionalProperty returns collection positional property when index exceeds defined positional count")]
    public void FindLastPositionalProperty_WithCollectionPositional_ReturnsCollectionProperty()
    {
        // Arrange - CommandWithPositionalCollectionOptions has:
        //   Index=0: string Command (scalar)
        //   Index=1: BindingList<string> Files (collection)
        var info = Parser.GetTypeInfo<CommandWithPositionalCollectionOptions>();

        // Act - index 2 has no exact match, should fall back to the collection at index 1
        var property = info.FindLastPositionalProperty(2);

        // Assert
        Assert.IsNotNull(property, "Should find the collection positional property at index 1");
        Assert.AreEqual(1, property.Attribute.Index);
        Assert.IsTrue(property.IsArray, "Found property should be a collection type");
        Assert.AreEqual("FILES", property.Attribute.MetaName);
    }

    [TestMethod]
    [Description("FindLastPositionalProperty returns collection property for higher indices too")]
    public void FindLastPositionalProperty_HigherIndex_StillReturnsCollectionProperty()
    {
        var info = Parser.GetTypeInfo<CommandWithPositionalCollectionOptions>();

        // Act - even at index 5, should still find the collection at index 1
        var property = info.FindLastPositionalProperty(5);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual(1, property.Attribute.Index);
        Assert.IsTrue(property.IsArray);
    }

    [TestMethod]
    [Description("FindLastPositionalProperty returns null when no collection positional exists before the index")]
    public void FindLastPositionalProperty_NoCollectionBefore_ReturnsNull()
    {
        // Arrange - CloneCommandOptions has:
        //   Index=0: string Command (scalar)
        //   Index=1: string Url (scalar)
        //   Neither is a collection type
        var info = Parser.GetTypeInfo<CloneCommandOptions>();

        // Act - index 2 has no match, and no collection positional exists
        var property = info.FindLastPositionalProperty(2);

        // Assert
        Assert.IsNull(property, "Should return null when no collection positional exists");
    }

    [TestMethod]
    [Description("FindLastPositionalProperty returns null for index 0 (no property with index < 0)")]
    public void FindLastPositionalProperty_IndexZero_ReturnsNull()
    {
        var info = Parser.GetTypeInfo<CommandWithPositionalCollectionOptions>();

        // Act - index 0 means we need a collection with index < 0, which doesn't exist
        var property = info.FindLastPositionalProperty(0);

        // Assert
        Assert.IsNull(property);
    }

    [TestMethod]
    [Description("FindLastPositionalProperty skips scalar positional properties")]
    public void FindLastPositionalProperty_SkipsScalarPositional_ReturnsNull()
    {
        // Arrange - MixedOptions has two scalar positional properties (no collection)
        var info = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var property = info.FindLastPositionalProperty(2);

        // Assert
        Assert.IsNull(property, "Should skip scalar positional properties");
    }

    [TestMethod]
    [Description("FindLastPositionalProperty with single collection at index 0")]
    public void FindLastPositionalProperty_SingleCollectionAtZero_ReturnsIt()
    {
        // Arrange - PositionalCollectionOptions has only:
        //   Index=0: BindingList<string> Files (collection)
        var info = Parser.GetTypeInfo<PositionalCollectionOptions>();

        // Act - index 1 should fall back to collection at index 0
        var property = info.FindLastPositionalProperty(1);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual(0, property.Attribute.Index);
        Assert.IsTrue(property.IsArray);
    }

    [TestMethod]
    [Description("FindLastPositionalProperty picks the highest-indexed collection before the given index")]
    public void FindLastPositionalProperty_PicksHighestIndexCollection()
    {
        // Arrange - FullMixedPositionalCollectionOptions has:
        //   Index=0: string Command (scalar)
        //   Index=1: BindingList<string> Files (collection)
        var info = Parser.GetTypeInfo<FullMixedPositionalCollectionOptions>();

        // Act - index 3 should find collection at index 1 (skips scalar at index 0)
        var property = info.FindLastPositionalProperty(3);

        // Assert
        Assert.IsNotNull(property);
        Assert.AreEqual(1, property.Attribute.Index);
        Assert.IsTrue(property.IsArray);
    }

    #endregion
}
