using System.ComponentModel;

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Configuration options for Avro serialization operation.
/// </summary>
public class Options
{
    /// <summary>
    /// Determines error handling behavior. If true, throws an exception when serialization fails.
    /// If false, returns error information in the Result object instead of throwing.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Custom error message to use when ThrowErrorOnFailure is false and an error occurs.
    /// If null or empty, the original exception message will be used.
    /// This allows for user-friendly error messages in automated workflows.
    /// </summary>
    /// <example>"Failed to serialize data to Avro format"</example>
    [DefaultValue(null)]
    public string? ErrorMessageOnFailure { get; set; }
}
