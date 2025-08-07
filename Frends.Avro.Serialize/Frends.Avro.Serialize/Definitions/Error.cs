#nullable enable

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Detailed error information when the Avro serialization task fails.
/// </summary>
public class Error
{
    /// <summary>
    /// Primary error message describing what went wrong during serialization.
    /// Can be either the original exception message or a custom message if specified in options.
    /// </summary>
    /// <example>"Field 'name' is missing in the JSON."</example>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Full exception details including stack trace if available.
    /// Useful for debugging serialization issues.
    /// </summary>
    /// <example>"System.ArgumentException: Field 'name' is missing in the JSON.\n   at Frends.Avro.Serialize..."</example>
    public string? Exception { get; init; }

    /// <summary>
    /// Additional contextual information about the error such as exception type and stack trace.
    /// Contains structured data that can be useful for automated error handling and logging.
    /// </summary>
    /// <example>{ "ExceptionType": "ArgumentException", "StackTrace": "..." }</example>
    public object? AdditionalInfo { get; init; }
}
