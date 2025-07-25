using System.ComponentModel;

namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Options for Avro deserialization.
/// </summary>
public class Options
{
    /// <summary>
    /// Determines whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Custom error message to use when failure occurs and ThrowErrorOnFailure is false.
    /// </summary>
    /// <example></example>
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = "";
}
