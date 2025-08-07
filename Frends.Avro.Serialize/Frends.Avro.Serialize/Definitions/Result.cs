namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Path to the file with result
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    public string FilePath { get; init; }

    /// <summary>
    /// Error message when task fails and ThrowErrorOnFailure is false
    /// </summary>
    /// <example>Invalid JSON format</example>
    public string ErrorMessage { get; init; } = "";
}
