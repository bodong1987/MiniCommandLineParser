using MiniCommandLineParser.Internals;
using System.Text;

namespace MiniCommandLineParser;

/// <summary>
/// Defines the contract for formatting help text output.
/// </summary>
public interface IFormatter
{
    /// <summary>
    /// Appends a formatted option entry to the help text builder.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="name">The option name (e.g., "-h, --help").</param>
    /// <param name="attribute">The option attributes (e.g., "Optional", "Required").</param>
    /// <param name="helpText">The description of the option.</param>
    /// <param name="usage">The usage example for the option.</param>
    void Append(
        StringBuilder builder, 
        string name, 
        string attribute, 
        string helpText, 
        string usage
    );
}

/// <summary>
/// Default implementation of <see cref="IFormatter"/> that formats help text with configurable indentation and spacing.
/// </summary>
/// <seealso cref="IFormatter" />
public class Formatter : IFormatter
{
    /// <summary>
    /// The number of spaces for left indentation.
    /// </summary>
    private readonly int _indent;

    /// <summary>
    /// The minimum width allocated for option names before help text begins.
    /// </summary>
    private readonly int _blank;

    /// <summary>
    /// Initializes a new instance of the <see cref="Formatter"/> class.
    /// </summary>
    /// <param name="indent">The number of spaces for left indentation.</param>
    /// <param name="blank">The minimum width allocated for option names.</param>
    public Formatter(int indent, int blank)
    {
        _indent = indent;
        _blank = blank;
    }

    /// <summary>
    /// Appends a formatted option entry to the help text builder.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="name">The option name (e.g., "-h, --help").</param>
    /// <param name="attribute">The option attributes (e.g., "Optional", "Required").</param>
    /// <param name="helpText">The description of the option.</param>
    /// <param name="usage">The usage example for the option.</param>
    public void Append(
        StringBuilder builder, 
        string name, 
        string attribute, 
        string helpText, 
        string usage
    )
    {
        var blank = name.Length < _blank ? _blank - name.Length : 1;
        var attr = attribute.IsNotNullOrEmpty() ? $"[{attribute}]" : "";
        var dot = !helpText.EndsWith('.') ? ". " : " ";

        builder.AppendLine($"{BuildBlankText(_indent)}{name}{BuildBlankText(blank)}{attr}{helpText}{dot}{(usage.IsNotNullOrEmpty()?"usage:":"")}{usage}");
    }

    /// <summary>
    /// Creates a string of spaces with the specified length.
    /// </summary>
    /// <param name="count">The number of spaces.</param>
    /// <returns>A string containing the specified number of spaces.</returns>
    private static string BuildBlankText(int count)
    {
        return new string(' ', count);
    }
}