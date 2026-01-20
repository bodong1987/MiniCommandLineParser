// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace MiniCommandLineParser;

/// <summary>
/// Configuration settings for the command-line parser.
/// </summary>
public class ParserSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether option name matching is case-sensitive.
    /// </summary>
    /// <value><c>true</c> if case-sensitive matching is enabled; otherwise, <c>false</c>. Default is <c>false</c>.</value>
    public bool CaseSensitive { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether unknown arguments should be ignored during parsing.
    /// </summary>
    /// <value><c>true</c> to silently ignore unknown arguments; <c>false</c> to report them as errors. Default is <c>true</c>.</value>
    public bool IgnoreUnknownArguments { get; set; } = true;
}