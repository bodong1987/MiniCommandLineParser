using System.Collections.Generic;
using System.Text;

namespace MiniCommandLineParser;

/// <summary>
/// Describes the kind of help text section being rendered.
/// </summary>
public enum HelpSectionKind
{
    /// <summary>Positional arguments section.</summary>
    PositionalArguments,

    /// <summary>Named options section.</summary>
    Options
}

/// <summary>
/// Contains all structured data for a single option entry in help text.
/// Passed to <see cref="IFormatter.AppendOption"/> so the formatter has
/// full access to raw metadata and can render it however it chooses.
/// </summary>
public sealed class OptionHelpInfo
{
    /// <summary>The formatted display name (e.g., "-p, --project" or "&lt;PATHS&gt; (-f, --files)").</summary>
    public string DisplayName { get; set; } = "";

    /// <summary>The short option name without dash (e.g., "p"), or <c>null</c> if none.</summary>
    public string? ShortName { get; set; }

    /// <summary>The long option name without dashes (e.g., "project"), or <c>null</c> if none.</summary>
    public string? LongName { get; set; }

    /// <summary>Whether this option is required.</summary>
    public bool IsRequired { get; set; }

    /// <summary>Whether this is a positional argument.</summary>
    public bool IsPositional { get; set; }

    /// <summary>The positional index, or -1 if not positional.</summary>
    public int Index { get; set; } = -1;

    /// <summary>Whether the property type is a collection (array/list).</summary>
    public bool IsArray { get; set; }

    /// <summary>Whether the property type is a [Flags] enum.</summary>
    public bool IsFlags { get; set; }

    /// <summary>Whether the property type is an enum (non-flags).</summary>
    public bool IsEnum { get; set; }

    /// <summary>The CLR type of the property.</summary>
    public System.Type PropertyType { get; set; } = typeof(object);

    /// <summary>
    /// The default value as a string, or <c>null</c> if no meaningful default exists.
    /// Trivial defaults ("0", "False", collection type names) are pre-filtered to <c>null</c>.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>The environment variable name, or <c>null</c> if not configured.</summary>
    public string? EnvironmentVariable { get; set; }

    /// <summary>The user-facing description text from <see cref="OptionAttribute.HelpText"/>.</summary>
    public string HelpText { get; set; } = "";

    /// <summary>The usage example string (e.g., "&lt;PATHS&gt;" or "--level Debug Info Warning"), or empty.</summary>
    public string Usage { get; set; } = "";

    /// <summary>
    /// The pre-formatted attribute tags (e.g., ["Required", "Index: 0", "Array"]).
    /// Provided for convenience; formatters can also build their own from the raw fields above.
    /// </summary>
    public IReadOnlyList<string> AttributeTags { get; set; } = new List<string>();
}

/// <summary>
/// Defines the contract for formatting help text output using a visitor pattern.
/// <para>
/// The <see cref="Parser"/> calls these methods in order while traversing the type metadata:
/// <list type="number">
///   <item><see cref="BeginSection"/> — once per section (positional args, options)</item>
///   <item><see cref="AppendOption"/> — once per option within the section</item>
///   <item><see cref="EndSection"/> — closes the section</item>
/// </list>
/// After all sections are visited, <see cref="Build"/> is called to produce the final string.
/// </para>
/// </summary>
public interface IFormatter
{
    /// <summary>
    /// Called when a new section begins (e.g., "Positional Arguments" or "Options").
    /// </summary>
    /// <param name="kind">The kind of section being started.</param>
    void BeginSection(HelpSectionKind kind);

    /// <summary>
    /// Called for each option entry within the current section.
    /// </summary>
    /// <param name="info">The structured data for this option.</param>
    void AppendOption(OptionHelpInfo info);

    /// <summary>
    /// Called when the current section ends.
    /// </summary>
    /// <param name="kind">The kind of section being ended.</param>
    void EndSection(HelpSectionKind kind);

    /// <summary>
    /// Produces the final formatted help text string.
    /// Called once after all sections have been visited.
    /// </summary>
    /// <returns>The complete help text.</returns>
    string Build();
}

/// <summary>
/// Default implementation of <see cref="IFormatter"/> that produces clean,
/// column-aligned help text with configurable indentation and spacing.
/// <para>
/// Output format per option:
/// <code>
///     -p, --project                              [Optional]
///                                                Project root directory.
///                                                usage: &lt;DESCRIPTOR&gt;
/// </code>
/// </para>
/// </summary>
public class Formatter : IFormatter
{
    private readonly int _indent;
    private readonly int _blank;
    private readonly StringBuilder _builder = new StringBuilder();

    /// <summary>
    /// Initializes a new instance of the <see cref="Formatter"/> class.
    /// </summary>
    /// <param name="indent">The number of spaces for left indentation.</param>
    /// <param name="blank">The minimum width allocated for option names before attributes begin.</param>
    public Formatter(int indent, int blank)
    {
        _indent = indent;
        _blank = blank;
    }

    /// <inheritdoc />
    public virtual void BeginSection(HelpSectionKind kind)
    {
        var title = "";

        switch (kind)
        {
            case HelpSectionKind.PositionalArguments:
                title = "Positional Arguments:";
                break;
            case HelpSectionKind.Options:
                title = "Options:";
                break;
        }

        if (!string.IsNullOrEmpty(title))
        {
            _builder.AppendLine(title);
        }
    }

    /// <inheritdoc />
    public virtual void AppendOption(OptionHelpInfo info)
    {
        var indentStr = Pad(_indent);
        var blank = info.DisplayName.Length < _blank ? _blank - info.DisplayName.Length : 1;
        var attr = info.AttributeTags.Count > 0 ? $"[{string.Join(", ", info.AttributeTags)}]" : "";

        // Line 1: name + attributes
        _builder.AppendLine($"{indentStr}{info.DisplayName}{Pad(blank)}{attr}");

        // Line 2: description
        if (!string.IsNullOrEmpty(info.HelpText))
        {
            _builder.AppendLine($"{indentStr}{Pad(_blank)}{info.HelpText}");
        }

        // Line 3: usage example
        if (!string.IsNullOrEmpty(info.Usage))
        {
            _builder.AppendLine($"{indentStr}{Pad(_blank)}usage: {info.Usage}");
        }
    }

    /// <inheritdoc />
    public virtual void EndSection(HelpSectionKind kind)
    {
        // Add a blank line after positional arguments section to separate from options
        if (kind == HelpSectionKind.PositionalArguments)
        {
            _builder.AppendLine();
        }
    }

    /// <inheritdoc />
    public virtual string Build() => _builder.ToString();

    /// <summary>Creates a string of <paramref name="count"/> spaces.</summary>
    protected static string Pad(int count) => count > 0 ? new string(' ', count) : "";
}
