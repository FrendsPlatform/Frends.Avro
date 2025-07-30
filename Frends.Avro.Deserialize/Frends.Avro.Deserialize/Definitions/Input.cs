using System.ComponentModel.DataAnnotations;

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
    /// <example>C:\results\myfile.avro</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string FilePath { get; init; }
}
