#nullable enable

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the serialization operation was successful
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Path to the file with result
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    public string FilePath { get; init; } = string.Empty;

    /// <summary>
    /// Error information when task fails and ThrowErrorOnFailure is false
    /// </summary>
    public Error? Error { get; init; }
}
