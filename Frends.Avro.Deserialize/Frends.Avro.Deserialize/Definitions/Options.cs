using System.ComponentModel;

namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Configuration options for Avro deserialization operation.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception when deserialization fails.
    /// When set to true, exceptions will be thrown on failure. When set to false, errors will be returned in the Result object.
    /// </summary>
    /// <value>true to throw exceptions on failure; false to return error information in the result.</value>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets a custom error message to use when deserialization fails and ThrowErrorOnFailure is false.
    /// If empty, the original exception message will be used.
    /// </summary>
    /// <value>The custom error message, or empty string to use the default exception message.</value>
    /// <example>Custom error occurred during Avro deserialization</example>
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = "";
}
