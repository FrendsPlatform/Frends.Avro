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
    public JArray Json { get; init; }
}
