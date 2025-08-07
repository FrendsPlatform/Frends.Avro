namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Error information when task fails.
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Exception details if available
    /// </summary>
    public string? Exception { get; init; }
}
