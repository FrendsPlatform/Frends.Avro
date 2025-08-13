namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Input parameters for Avro serialization operation.
/// </summary>
public class Input
{
    /// <summary>
    /// Input JSON string that can be both single objects and arrays of objects.
    /// The JSON data will be serialized according to the provided Avro schema.
    /// </summary>
    /// <example>
    /// Single object: { "name": "Jerry", "age": 30, "city": "New York" }
    /// Array of objects: [{ "name": "John", "age": 30 }, { "name": "Jane", "age": 25 }]
    /// </example>
    public string Json { get; init; }

    /// <summary>
    /// Avro Schema definition in JSON format that defines the structure of the data to be serialized.
    /// Must be a valid Avro schema that matches the structure of the input JSON data.
    /// </summary>
    /// <example>
    /// {
    ///     "type": "record",
    ///     "name": "Person",
    ///     "fields": [
    ///         { "name": "name", "type": "string" },
    ///         { "name": "age", "type": "int" },
    ///         { "name": "city", "type": "string" }
    ///     ]
    /// }
    /// </example>
    public string Schema { get; init; }

    /// <summary>
    /// Full path to the target file where the serialized Avro data will be stored.
    /// The directory must exist and the file must not already exist (unless overwrite is enabled).
    /// </summary>
    /// <example>
    /// Windows: C:\data\output\myfile.avro
    /// Linux/Mac: /home/user/data/myfile.avro
    /// </example>
    public string TargetFilePath { get; init; }
}
