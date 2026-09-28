using System.ComponentModel.DataAnnotations;

namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Input parameters for Avro deserialization operation.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets the path to the Avro file you want to deserialize.
    /// The file can be an Avro object container file, or raw Avro binary data when SchemaJson is provided.
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string FilePath { get; init; }

    /// <summary>
    /// Gets or sets the Avro schema JSON.
    /// This is required for raw Avro binary data and optional as a reader schema for Avro object container files.
    /// </summary>
    /// <example>{"type":"record","name":"Event","fields":[]}</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string SchemaJson { get; init; }
}
