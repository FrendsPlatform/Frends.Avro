namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Input parameters for Avro deserialization operation.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets the path to the Avro file you want to deserialize.
    /// The file must be a valid Avro format file.
    /// </summary>
    /// <value>The full path to the Avro file.</value>
    /// <example>C:\results\myfile.avro</example>
    public string FilePath { get; init; }
}
