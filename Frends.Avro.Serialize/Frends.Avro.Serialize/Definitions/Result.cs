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
}
