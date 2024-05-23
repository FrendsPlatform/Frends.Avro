using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Input JSON string
    /// </summary>
    /// <example>{ "foo": "bar" }</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Json { get; set; }

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
    public string Schema { get; set; }
}