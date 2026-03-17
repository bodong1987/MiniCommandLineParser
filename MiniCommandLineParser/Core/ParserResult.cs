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
/// Represents the type of parsing error that occurred.
/// </summary>
public enum ParseErrorType
{
    /// <summary>
    /// A required option was not provided.
    /// </summary>
    MissingRequired,

    /// <summary>
    /// An unknown option was encountered.
    /// </summary>
    UnknownOption,

    /// <summary>
    /// Failed to convert a value to the expected type.
    /// </summary>
    InvalidValue,

    /// <summary>
    /// A general parsing error occurred.
    /// </summary>
    General
}

/// <summary>
/// Represents a single parsing error with structured information.
/// </summary>
public class ParseError
{
    /// <summary>
    /// Gets the name of the option that caused the error.
    /// </summary>
    public string OptionName { get; }

    /// <summary>
    /// Gets the type of error that occurred.
    /// </summary>
    public ParseErrorType Type { get; }

    /// <summary>
    /// Gets the error message describing what went wrong.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseError"/> class.
    /// </summary>
    /// <param name="optionName">The name of the option that caused the error.</param>
    /// <param name="type">The type of error.</param>
    /// <param name="message">The error message.</param>
    public ParseError(string optionName, ParseErrorType type, string message)
    {
        OptionName = optionName;
        Type = type;
        Message = message;
    }

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    public override string ToString() => Message;
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
    /// Gets the list of structured parsing errors.
    /// </summary>
    /// <value>A read-only list of <see cref="ParseError"/> objects.</value>
    IReadOnlyList<ParseError> Errors { get; }

    /// <summary>
    /// Gets all arguments encountered during parsing, including both recognized
    /// and unrecognized ones. Keys are normalized argument names without leading dashes
    /// (e.g., "username" from "--username"). Values are the corresponding argument values,
    /// or <c>null</c> for boolean flags without explicit values.
    /// </summary>
    /// <value>A read-only dictionary of all parsed arguments.</value>
    IReadOnlyDictionary<string, string?> Arguments { get; }

    /// <summary>
    /// Tries to retrieve the value of a named argument by its name (without dashes).
    /// </summary>
    /// <param name="name">The argument name without leading dashes (e.g., "p4-assets-username").</param>
    /// <param name="value">When this method returns, contains the argument value if found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the argument was present in the parsed command line; otherwise, <c>false</c>.</returns>
    bool TryGetArgumentValue(string name, out string? value);

    /// <summary>
    /// Checks whether a named argument was present in the parsed command line.
    /// </summary>
    /// <param name="name">The argument name without leading dashes (e.g., "verbose").</param>
    /// <returns><c>true</c> if the argument was present; otherwise, <c>false</c>.</returns>
    bool HasArgument(string name);

    /// <summary>
    /// Appends an error message to the error collection.
    /// </summary>
    /// <param name="message">The error message to append.</param>
    void AppendError(string message);

    /// <summary>
    /// Appends a structured error to the error collection.
    /// </summary>
    /// <param name="optionName">The name of the option that caused the error.</param>
    /// <param name="type">The type of error.</param>
    /// <param name="message">The error message.</param>
    void AppendError(string optionName, ParseErrorType type, string message);

    /// <summary>
    /// Records an argument key-value pair encountered during parsing.
    /// This is called internally by the parser.
    /// </summary>
    /// <param name="name">The argument name without leading dashes.</param>
    /// <param name="value">The argument value, or <c>null</c> for boolean flags.</param>
    internal void SetArgument(string name, string? value);
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
    private readonly StringBuilder _errorMessages = new();

    /// <summary>
    /// The collection of structured errors accumulated during parsing.
    /// </summary>
    private readonly List<ParseError> _errors = [];

    /// <summary>
    /// Gets the parsed options object as a non-generic reference.
    /// </summary>
    /// <value>The populated options object.</value>
    object? IParserResult.Value => Value;

    /// <summary>
    /// Gets the accumulated error messages as a single string.
    /// </summary>
    /// <value>A string containing all error messages separated by newlines.</value>
    public string ErrorMessage => _errorMessages.ToString();

    /// <summary>
    /// Gets the list of structured parsing errors.
    /// </summary>
    /// <value>A read-only list of <see cref="ParseError"/> objects.</value>
    public IReadOnlyList<ParseError> Errors => _errors;

    /// <summary>
    /// The dictionary of all arguments encountered during parsing (both recognized and unrecognized).
    /// Keys are case-insensitive.
    /// </summary>
    private readonly Dictionary<string, string?> _arguments = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public IReadOnlyDictionary<string, string?> Arguments => _arguments;

    /// <inheritdoc />
    public bool TryGetArgumentValue(string name, out string? value)
        => _arguments.TryGetValue(name, out value);

    /// <inheritdoc />
    public bool HasArgument(string name)
        => _arguments.ContainsKey(name);

    /// <inheritdoc />
    void IParserResult.SetArgument(string name, string? value)
        => _arguments[name] = value;

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
        _errorMessages.AppendLine(message);
        _errors.Add(new ParseError("", ParseErrorType.General, message));
    }

    /// <summary>
    /// Appends a structured error to the error collection.
    /// </summary>
    /// <param name="optionName">The name of the option that caused the error.</param>
    /// <param name="type">The type of error.</param>
    /// <param name="message">The error message.</param>
    public void AppendError(string optionName, ParseErrorType type, string message)
    {
        _errorMessages.AppendLine(message);
        _errors.Add(new ParseError(optionName, type, message));
    }
}