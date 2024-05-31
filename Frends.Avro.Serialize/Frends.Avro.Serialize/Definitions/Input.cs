namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Input JSON string that can be both signle objects and arrays of objects.
    /// </summary>
    /// <example>{ "foo": "bar" }</example>
    public string Json { get; init; }

    /// <summary>
    /// Avro Schema to use
    /// </summary>
    /// <example>
    /// {
    ///     "type": "record",
    ///     "name":"Person",
    ///     "fields":[
    ///         { "name":"name", "type":"string" },
    ///         { "name":"age","type":"int" },
    ///         { "name":"city","type":"string" }
    ///     ]
    /// }
    /// </example>
    public string Schema { get; init; }

    /// <summary>
    /// Path to the file where you want to store result
    /// </summary>
    /// <example>C:\results\myfile.avro</example>
    public string OutputPath { get; init; }
}
