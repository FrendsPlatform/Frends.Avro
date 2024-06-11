namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Path to the Avro file you want to deserialize
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    public string AvroFilePath { get; init; }
}
