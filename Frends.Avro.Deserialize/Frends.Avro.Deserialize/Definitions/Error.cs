namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Error information for failed operations.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets additional information about the error.
    /// </summary>
    public dynamic AdditionalInfo { get; set; }
}
