#nullable enable

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Result of the Avro serialization operation containing success status, file path, and error information.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the serialization operation completed successfully.
    /// True if the JSON data was successfully serialized to Avro format, false if an error occurred.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Full path to the created Avro file when serialization is successful.
    /// Empty string when serialization fails.
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    public string FilePath { get; init; } = string.Empty;

    /// <summary>
    /// Error information when the serialization task fails and ThrowErrorOnFailure option is set to false.
    /// Contains detailed error message and additional debugging information.
    /// Null when the operation succeeds or when ThrowErrorOnFailure is true.
    /// </summary>
    /// <example>null</example>
    public Error? Error { get; init; }
}
