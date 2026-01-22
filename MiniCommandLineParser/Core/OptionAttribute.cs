using MiniCommandLineParser.Internals;

namespace MiniCommandLineParser;

/// <summary>
/// Marks a property as a command-line option with optional short and long names.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Property)]
public class OptionAttribute : Attribute
{
    /// <summary>
    /// Gets the long name of the option (e.g., "help" for "--help").
    /// </summary>
    /// <value>The long name, or <c>null</c> if not specified.</value>
    public string? LongName { get; internal set; }
    /// <summary>
    /// Gets the short name of the option (e.g., "h" for "-h").
    /// </summary>
    /// <value>The short name, or <c>null</c> if not specified.</value>
    public string? ShortName { get; }
        
    /// <summary>
    /// Gets or sets the description text displayed in help output.
    /// </summary>
    /// <value>The help text description.</value>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string HelpText { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether this option must be provided.
    /// </summary>
    /// <value><c>true</c> if the option is required; otherwise, <c>false</c>.</value>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public bool Required { get; set; } = false;

    /// <summary>
    /// Gets or sets the separator character used to split a single argument value into multiple values for array-type options.
    /// </summary>
    /// <remarks>
    /// When this property is set to a non-null character, a single argument value containing this separator
    /// will be split into multiple values. For example, if <see cref="Separator"/> is set to <c>','</c>,
    /// then <c>--files a.txt,b.txt,c.txt</c> will be parsed as three separate values: "a.txt", "b.txt", and "c.txt".
    /// If this property is <c>null</c>, no splitting will be performed.
    /// </remarks>
    /// <value>The separator character, or <c>null</c> to disable value splitting.</value>
    public char Separator { get; set; } = ';';

    /// <summary>
    /// Gets or sets the zero-based index position for positional (value) arguments.
    /// </summary>
    /// <remarks>
    /// When <see cref="Index"/> is set to a non-negative value, this option becomes a positional argument
    /// that does not require a name prefix (e.g., <c>-</c> or <c>--</c>).
    /// For example, in <c>app.exe clone http://example.com</c>, "clone" could be at index 0
    /// and "http://example.com" at index 1.
    /// The default value of -1 means this is a named option, not a positional argument.
    /// </remarks>
    /// <value>The zero-based index position, or -1 if this is a named option.</value>
    public int Index { get; set; } = -1;

    /// <summary>
    /// Gets or sets the meta name used in help text to describe the expected value.
    /// </summary>
    /// <remarks>
    /// This is particularly useful for positional arguments to provide a descriptive name
    /// in help output (e.g., "URL", "FILE", "COMMAND").
    /// </remarks>
    /// <value>The meta name for help text display.</value>
    public string MetaName { get; set; } = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class with both short and long names.
    /// </summary>
    /// <param name="shortName">The short name (e.g., "h" for "-h").</param>
    /// <param name="longName">The long name (e.g., "help" for "--help").</param>
    private OptionAttribute(string shortName, string longName)
    {
        ShortName = shortName;
        LongName = longName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
    /// The property name will be used as the long name.
    /// </summary>
    public OptionAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class with a long name only.
    /// </summary>
    /// <param name="longName">The long name (e.g., "help" for "--help").</param>
    public OptionAttribute(string longName) :
        this(string.Empty, longName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute" /> class with both short and long names.
    /// </summary>
    /// <param name="shortName">The short name character (e.g., 'h' for "-h").</param>
    /// <param name="longName">The long name (e.g., "help" for "--help").</param>
    public OptionAttribute(char shortName, string longName) :
        this(new string(shortName,1), longName)
    {
    }

    /// <summary>
    /// Gets a value indicating whether this option is a positional (value) argument.
    /// </summary>
    /// <value><c>true</c> if <see cref="Index"/> is non-negative; otherwise, <c>false</c>.</value>
    public bool IsPositional => Index >= 0;

    /// <summary>
    /// Returns a string representation of this option in command-line format.
    /// </summary>
    /// <returns>
    /// For positional arguments: "&lt;MetaName&gt;" or "Value[Index]".
    /// For named options: "-h --help" or "--help" depending on configured names.
    /// </returns>
    public override string ToString()
    {
        // For positional arguments, show meta name or index
        if (IsPositional)
        {
            return MetaName.IsNotNullOrEmpty() ? $"<{MetaName}>" : $"Value[{Index}]";
        }

        if(ShortName.IsNotNullOrEmpty())
        {
            return LongName.IsNotNullOrEmpty() ? $"-{ShortName} --{LongName}" : $"-{ShortName}";
        }

        return LongName.IsNotNullOrEmpty() ? $"--{LongName}" : "";
    }
}