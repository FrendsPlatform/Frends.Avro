using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Task result.
/// </summary>
public class Result
{
    /// <summary>
    /// JArray with deserialized data
    /// </summary>
    /// <example>[{"foo": "bar"}]</example>
    public dynamic Json { get; init; }

    /// <summary>
    /// Indicates whether the deserialization operation was successful
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Error information when the operation fails
    /// </summary>
    /// <example>null</example>
    public Error Error { get; init; }
}
