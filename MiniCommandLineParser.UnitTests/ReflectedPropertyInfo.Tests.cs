namespace MiniCommandLineParser.UnitTests;

[TestClass]
public sealed class ReflectedPropertyInfoTests
{
    [TestMethod]
    public void ReflectedPropertyInfo_IsArray_DetectsListType()
    {
        // Arrange
        var info = Parser.GetTypeInfo<ArrayOptions>();

        // Act
        var filesProperty = info.FindLongProperty("files", ignoreCase: false);

        // Assert
        Assert.IsNotNull(filesProperty);
        Assert.IsTrue(filesProperty.IsArray);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_IsFlags_DetectsFlagsEnum()
    {
        // Arrange
        var info = Parser.GetTypeInfo<EnumOptions>();

        // Act
        var accessProperty = info.FindLongProperty("access", ignoreCase: false);

        // Assert
        Assert.IsNotNull(accessProperty);
        Assert.IsTrue(accessProperty.IsFlags);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_GetElementType_ReturnsElementType()
    {
        // Arrange
        var info = Parser.GetTypeInfo<ArrayOptions>();
        var filesProperty = info.FindLongProperty("files", ignoreCase: false);

        // Act
        var elementType = filesProperty?.GetElementType();

        // Assert
        Assert.IsNotNull(elementType);
        Assert.AreEqual(typeof(string), elementType);
    }

    [TestMethod]
    public void ReflectedPropertyInfo_GetSetValue_WorksCorrectly()
    {
        // Arrange
        var info = Parser.GetTypeInfo<BasicOptions>();
        var nameProperty = info.FindLongProperty("name", ignoreCase: false);
        var target = new BasicOptions();

        // Act
        nameProperty!.SetValue(target, "TestName");
        var value = nameProperty.GetValue(target);

        // Assert
        Assert.AreEqual("TestName", value);
        Assert.AreEqual("TestName", target.Name);
    }
}
