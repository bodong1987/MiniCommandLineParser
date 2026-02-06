namespace MiniCommandLineParser.UnitTests;

/// <summary>
/// Test options class with environment variable support.
/// Each test uses a unique GUID suffix to avoid parallel test interference.
/// </summary>
public class EnvironmentVariableOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_NAME", HelpText = "Name from env or command line")]
    public string? Name { get; set; }

    [Option('c', "count", EnvironmentVariable = "TEST_COUNT", HelpText = "Count from env or command line")]
    public int Count { get; set; }

    [Option('v', "verbose", EnvironmentVariable = "TEST_VERBOSE", HelpText = "Verbose from env or command line")]
    public bool Verbose { get; set; }

    [Option('f', "files", EnvironmentVariable = "TEST_FILES", HelpText = "Files from env or command line")]
    public List<string>? Files { get; set; }
}

/// <summary>
/// Test options class with environment variable and Required combined.
/// </summary>
public class RequiredEnvironmentVariableOptions
{
    [Option('i', "input", Required = true, EnvironmentVariable = "TEST_INPUT", HelpText = "Required input")]
    public string? Input { get; set; }

    [Option('o', "output", HelpText = "Optional output")]
    public string? Output { get; set; }
}

// Test-specific options classes with unique environment variable names to avoid parallel test interference

public class EnvTestFallbackOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_FALLBACK_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }
}

public class EnvTestPrecedenceOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_PRECEDENCE_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }
}

public class EnvTestIntOptions
{
    [Option('c', "count", EnvironmentVariable = "TEST_INT_COUNT", HelpText = "Count from env")]
    public int Count { get; set; }
}

public class EnvTestBoolOptions
{
    [Option('v', "verbose", EnvironmentVariable = "TEST_BOOL_VERBOSE", HelpText = "Verbose from env")]
    public bool Verbose { get; set; }
}

public class EnvTestArrayOptions
{
    [Option('f', "files", EnvironmentVariable = "TEST_ARRAY_FILES", HelpText = "Files from env")]
    public List<string>? Files { get; set; }
}

public class EnvTestRequiredOptions
{
    [Option('i', "input", Required = true, EnvironmentVariable = "TEST_REQUIRED_INPUT", HelpText = "Required input")]
    public string? Input { get; set; }
}

public class EnvTestDefaultOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_DEFAULT_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }

    [Option('c', "count", EnvironmentVariable = "TEST_DEFAULT_COUNT", HelpText = "Count from env")]
    public int Count { get; set; }

    [Option('v', "verbose", EnvironmentVariable = "TEST_DEFAULT_VERBOSE", HelpText = "Verbose from env")]
    public bool Verbose { get; set; }
}

public class EnvTestMultipleOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_MULTIPLE_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }

    [Option('c', "count", EnvironmentVariable = "TEST_MULTIPLE_COUNT", HelpText = "Count from env")]
    public int Count { get; set; }

    [Option('v', "verbose", EnvironmentVariable = "TEST_MULTIPLE_VERBOSE", HelpText = "Verbose from env")]
    public bool Verbose { get; set; }
}

public class EnvTestPartialOptions
{
    [Option('n', "name", EnvironmentVariable = "TEST_PARTIAL_NAME", HelpText = "Name from env")]
    public string? Name { get; set; }

    [Option('c', "count", HelpText = "Count from command line only")]
    public int Count { get; set; }
}

/// <summary>
/// Tests for environment variable support in option parsing.
/// Each test uses unique environment variable names to avoid interference during parallel execution.
/// </summary>
[TestClass]
public sealed class EnvironmentVariableTests
{
    [TestMethod]
    public void Parse_EnvironmentVariable_FallsBackToEnvWhenNotProvided()
    {
        // Arrange - use unique env var name
        const string envVarName = "TEST_FALLBACK_NAME";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "EnvValue");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestFallbackOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("EnvValue", result.Value.Name);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_CommandLineTakesPrecedence()
    {
        // Arrange
        const string envVarName = "TEST_PRECEDENCE_NAME";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "EnvValue");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestPrecedenceOptions>("--name CommandLineValue");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("CommandLineValue", result.Value.Name);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_IntValue_ParsesCorrectly()
    {
        // Arrange
        const string envVarName = "TEST_INT_COUNT";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "42");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestIntOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(42, result.Value.Count);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_BoolValue_ParsesCorrectly()
    {
        // Arrange
        const string envVarName = "TEST_BOOL_VERBOSE";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "true");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestBoolOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Verbose);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_ArrayValue_SplitsByComma()
    {
        // Arrange
        const string envVarName = "TEST_ARRAY_FILES";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "file1.txt,file2.txt,file3.txt");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestArrayOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.IsNotNull(result.Value.Files);
            Assert.AreEqual(3, result.Value.Files.Count);
            Assert.AreEqual("file1.txt", result.Value.Files[0]);
            Assert.AreEqual("file2.txt", result.Value.Files[1]);
            Assert.AreEqual("file3.txt", result.Value.Files[2]);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_Required_SatisfiedByEnv()
    {
        // Arrange
        const string envVarName = "TEST_REQUIRED_INPUT";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "EnvInput");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestRequiredOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("EnvInput", result.Value.Input);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_NotSet_UsesDefault()
    {
        // Arrange - ensure env vars are NOT set (use unique names that shouldn't be set)
        const string envVarNameName = "TEST_DEFAULT_NAME";
        const string envVarNameCount = "TEST_DEFAULT_COUNT";
        const string envVarNameVerbose = "TEST_DEFAULT_VERBOSE";
        
        // Clear any existing values
        Environment.SetEnvironmentVariable(envVarNameName, null);
        Environment.SetEnvironmentVariable(envVarNameCount, null);
        Environment.SetEnvironmentVariable(envVarNameVerbose, null);
        
        var parser = new Parser();

        // Act
        var result = parser.Parse<EnvTestDefaultOptions>("");

        // Assert
        Assert.AreEqual(ParserResultType.Parsed, result.Result);
        Assert.IsNotNull(result.Value);
        Assert.IsNull(result.Value.Name);
        Assert.AreEqual(0, result.Value.Count);
        Assert.IsFalse(result.Value.Verbose);
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_MultipleOptions_ParsesAll()
    {
        // Arrange
        const string envVarNameName = "TEST_MULTIPLE_NAME";
        const string envVarNameCount = "TEST_MULTIPLE_COUNT";
        const string envVarNameVerbose = "TEST_MULTIPLE_VERBOSE";
        try
        {
            Environment.SetEnvironmentVariable(envVarNameName, "EnvName");
            Environment.SetEnvironmentVariable(envVarNameCount, "100");
            Environment.SetEnvironmentVariable(envVarNameVerbose, "true");
            var parser = new Parser();

            // Act
            var result = parser.Parse<EnvTestMultipleOptions>("");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("EnvName", result.Value.Name);
            Assert.AreEqual(100, result.Value.Count);
            Assert.IsTrue(result.Value.Verbose);
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarNameName, null);
            Environment.SetEnvironmentVariable(envVarNameCount, null);
            Environment.SetEnvironmentVariable(envVarNameVerbose, null);
        }
    }

    [TestMethod]
    public void Parse_EnvironmentVariable_PartialCommandLine_UsesBothSources()
    {
        // Arrange
        const string envVarName = "TEST_PARTIAL_NAME";
        try
        {
            Environment.SetEnvironmentVariable(envVarName, "EnvName");
            var parser = new Parser();

            // Act - provide count via command line, name via environment
            var result = parser.Parse<EnvTestPartialOptions>("--count 50");

            // Assert
            Assert.AreEqual(ParserResultType.Parsed, result.Result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("EnvName", result.Value.Name); // from env
            Assert.AreEqual(50, result.Value.Count); // from command line
        }
        finally
        {
            Environment.SetEnvironmentVariable(envVarName, null);
        }
    }

    [TestMethod]
    public void OptionAttribute_EnvironmentVariable_Property_StoresValue()
    {
        // Arrange
        var attr = new OptionAttribute("test") { EnvironmentVariable = "MY_VAR" };

        // Assert
        Assert.AreEqual("MY_VAR", attr.EnvironmentVariable);
    }

    [TestMethod]
    public void OptionAttribute_EnvironmentVariable_DefaultIsNull()
    {
        // Arrange
        var attr = new OptionAttribute("test");

        // Assert
        Assert.IsNull(attr.EnvironmentVariable);
    }
}
