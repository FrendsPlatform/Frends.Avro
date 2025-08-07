using System.ComponentModel;

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Options for Avro serialization.
/// </summary>
public class Options
{
    /// <summary>
    /// If true, throws an exception on failure. If false, returns error message in result.
    /// </summary>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Custom error message to return when ThrowErrorOnFailure is false.
    /// </summary>
    [DefaultValue(null)]
    public string? ErrorMessageOnFailure { get; set; }
}
