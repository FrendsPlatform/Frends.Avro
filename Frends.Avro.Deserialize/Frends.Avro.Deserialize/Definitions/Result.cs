using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Represents the result of an Avro deserialization operation.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets or sets the deserialized JSON data as a dynamic object.
    /// Contains a JArray with the deserialized Avro records when successful, or error information when failed.
    /// </summary>
    /// <value>A dynamic object containing either the deserialized data or error information.</value>
    /// <example>[{"foo": "bar", "number": 123}]</example>
    public dynamic Json { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the deserialization operation was successful.
    /// </summary>
    /// <value>true if the operation succeeded; false if it failed.</value>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Gets or sets the error information when the deserialization operation fails.
    /// This property is null when the operation is successful.
    /// </summary>
    /// <value>An Error object containing failure details, or null if the operation succeeded.</value>
    /// <example>null</example>
    public Error Error { get; init; }
}
