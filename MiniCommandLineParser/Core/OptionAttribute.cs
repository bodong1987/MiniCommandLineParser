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
    /// Returns a string representation of this option in command-line format.
    /// </summary>
    /// <returns>A string like "-h --help" or "--help" depending on configured names.</returns>
    public override string ToString()
    {
        if(ShortName.IsNotNullOrEmpty())
        {
            return LongName.IsNotNullOrEmpty() ? $"-{ShortName} --{LongName}" : $"-{ShortName}";
        }

        return LongName.IsNotNullOrEmpty() ? $"--{LongName}" : "";
    }
}