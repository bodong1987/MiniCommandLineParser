using System.Text;

// ReSharper disable once CheckNamespace
namespace MiniCommandLineParser;

/// <summary>
/// Represents the result status of a command-line parsing operation.
/// </summary>
public enum ParserResultType
{
    /// <summary>
    /// Parsing completed successfully.
    /// </summary>
    Parsed,

    /// <summary>
    /// Parsing failed due to invalid arguments or other errors.
    /// </summary>
    NotParsed
}

/// <summary>
/// Defines the contract for command-line parsing results.
/// </summary>
public interface IParserResult
{
    /// <summary>
    /// Gets or sets the parsing result status.
    /// </summary>
    /// <value>The parsing result type indicating success or failure.</value>
    ParserResultType Result { get; internal set; }

    /// <summary>
    /// Gets the type information of the parsed options class.
    /// </summary>
    /// <value>The <see cref="TypeInfo"/> containing metadata about the options class.</value>
    // ReSharper disable once UnusedMemberInSuper.Global
    TypeInfo? Type { get; }

    /// <summary>
    /// Gets the parsed options object.
    /// </summary>
    /// <value>The populated options object, or <c>null</c> if parsing failed.</value>
    object? Value { get; }

    /// <summary>
    /// Gets the accumulated error messages from parsing.
    /// </summary>
    /// <value>A string containing all error messages, or empty if no errors occurred.</value>
    string ErrorMessage { get; }

    /// <summary>
    /// Appends an error message to the error collection.
    /// </summary>
    /// <param name="message">The error message to append.</param>
    void AppendError(string message);
}

/// <summary>
/// Contains the result of parsing command-line arguments into a strongly-typed options object.
/// </summary>
/// <typeparam name="T">The type of the options class.</typeparam>
public class ParserResult<T> : IParserResult
{
    /// <summary>
    /// Gets or sets the parsing result status.
    /// </summary>
    /// <value>The parsing result type indicating success or failure.</value>
    public ParserResultType Result { get; set; } = ParserResultType.NotParsed;
    /// <summary>
    /// Gets the parsed options object.
    /// </summary>
    /// <value>The populated options object of type <typeparamref name="T"/>.</value>
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the type information of the parsed options class.
    /// </summary>
    /// <value>The <see cref="TypeInfo"/> containing metadata about the options class.</value>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public TypeInfo? Type { get; set; }

    /// <summary>
    /// The collection of error messages accumulated during parsing.
    /// </summary>
    public readonly StringBuilder Errors = new();

    /// <summary>
    /// Gets the parsed options object as a non-generic reference.
    /// </summary>
    /// <value>The populated options object.</value>
    object? IParserResult.Value => Value;

    /// <summary>
    /// Gets the accumulated error messages as a single string.
    /// </summary>
    /// <value>A string containing all error messages separated by newlines.</value>
    public string ErrorMessage => Errors.ToString();

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserResult{T}"/> class.
    /// </summary>
    /// <param name="type">The type information for the options class.</param>
    public ParserResult(TypeInfo? type)
    {
        Type = type;
    }

    /// <summary>
    /// Appends an error message to the error collection.
    /// </summary>
    /// <param name="message">The error message to append.</param>
    public void AppendError(string message)
    {
        Errors.AppendLine(message);
    }
}