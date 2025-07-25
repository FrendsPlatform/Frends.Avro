namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Represents error information for failed Avro deserialization operations.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets the primary error message describing what went wrong.
    /// This can be either the original exception message or a custom message specified in options.
    /// </summary>
    /// <value>A string containing the error description.</value>
    /// <example>File not found: C:\data\missing.avro</example>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets additional contextual information about the error.
    /// Typically contains the exception type name or other diagnostic details.
    /// </summary>
    /// <value>Additional error context information.</value>
    /// <example>FileNotFoundException</example>
    public dynamic AdditionalInfo { get; set; }
}
