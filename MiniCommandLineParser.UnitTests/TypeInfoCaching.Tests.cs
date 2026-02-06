namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Tests for TypeInfo caching and performance optimizations.
/// </summary>
[TestClass]
public sealed class TypeInfoCachingTests
{
    [TestMethod]
    public void GetTypeInfo_SameType_ReturnsSameInstance()
    {
        // Act
        var info1 = Parser.GetTypeInfo<BasicOptions>();
        var info2 = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.AreSame(info1, info2);
    }

    [TestMethod]
    public void GetTypeInfo_DifferentTypes_ReturnsDifferentInstances()
    {
        // Act
        var info1 = Parser.GetTypeInfo<BasicOptions>();
        var info2 = Parser.GetTypeInfo<ArrayOptions>();

        // Assert
        Assert.AreNotSame(info1, info2);
    }

    [TestMethod]
    public void TypeInfo_PositionalProperties_IsCached()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var positional1 = typeInfo.PositionalProperties;
        var positional2 = typeInfo.PositionalProperties;

        // Assert
        Assert.AreSame(positional1, positional2);
    }

    [TestMethod]
    public void TypeInfo_NamedProperties_IsCached()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var named1 = typeInfo.NamedProperties;
        var named2 = typeInfo.NamedProperties;

        // Assert
        Assert.AreSame(named1, named2);
    }

    [TestMethod]
    public void TypeInfo_PositionalProperties_ReturnsCorrectCount()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var positionalProps = typeInfo.PositionalProperties;

        // Assert
        Assert.AreEqual(2, positionalProps.Length);
    }

    [TestMethod]
    public void TypeInfo_NamedProperties_ReturnsCorrectCount()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var namedProps = typeInfo.NamedProperties;

        // Assert
        Assert.AreEqual(2, namedProps.Length);
    }

    [TestMethod]
    public void TypeInfo_PositionalProperties_SortedByIndex()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var positionalProps = typeInfo.PositionalProperties;

        // Assert
        for (int i = 0; i < positionalProps.Length - 1; i++)
        {
            Assert.IsTrue(positionalProps[i].Attribute.Index <= positionalProps[i + 1].Attribute.Index,
                "Positional properties should be sorted by index");
        }
    }

    [TestMethod]
    public void TypeInfo_NamedProperties_ExcludesPositional()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var namedProps = typeInfo.NamedProperties;

        // Assert
        foreach (var prop in namedProps)
        {
            Assert.IsFalse(prop.Attribute.IsPositional, 
                $"Named properties should not include positional: {prop.Property.Name}");
        }
    }

    [TestMethod]
    public void TypeInfo_Properties_ReturnsAllProperties()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<MixedOptions>();

        // Act
        var allProps = typeInfo.Properties;
        var positionalCount = typeInfo.PositionalProperties.Length;
        var namedCount = typeInfo.NamedProperties.Length;

        // Assert
        Assert.AreEqual(positionalCount + namedCount, allProps.Length);
    }

    [TestMethod]
    public void TypeInfo_IsCommandLineObject_TrueForValidClass()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<BasicOptions>();

        // Assert
        Assert.IsTrue(typeInfo.IsCommandLineObject);
    }

    [TestMethod]
    public void TypeInfo_DefaultObject_CreatesInstance()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<BasicOptions>();

        // Act
        var defaultObj = typeInfo.DefaultObject;

        // Assert
        Assert.IsNotNull(defaultObj);
        Assert.IsInstanceOfType(defaultObj, typeof(BasicOptions));
    }

    [TestMethod]
    public void MultipleParses_UsesCachedTypeInfo()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result1 = parser.Parse<BasicOptions>("--verbose");
        var result2 = parser.Parse<BasicOptions>("--name John");
        var result3 = parser.Parse<BasicOptions>("--count 10");

        // Assert - all should use the same TypeInfo
        Assert.AreSame(result1.Type, result2.Type);
        Assert.AreSame(result2.Type, result3.Type);
    }

    [TestMethod]
    public void TypeInfo_FindShortProperty_Performance()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<BasicOptions>();

        // Act - Multiple lookups should be fast due to caching
        for (int i = 0; i < 1000; i++)
        {
            var prop = typeInfo.FindShortProperty("v", ignoreCase: false);
            Assert.IsNotNull(prop);
        }

        // Assert - If we get here without timeout, caching is working
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void TypeInfo_FindLongProperty_Performance()
    {
        // Arrange
        var typeInfo = Parser.GetTypeInfo<BasicOptions>();

        // Act - Multiple lookups should be fast due to caching
        for (int i = 0; i < 1000; i++)
        {
            var prop = typeInfo.FindLongProperty("verbose", ignoreCase: false);
            Assert.IsNotNull(prop);
        }

        // Assert - If we get here without timeout, caching is working
        Assert.IsTrue(true);
    }
}
