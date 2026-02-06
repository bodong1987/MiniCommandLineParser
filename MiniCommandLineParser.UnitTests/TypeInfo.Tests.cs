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
}
