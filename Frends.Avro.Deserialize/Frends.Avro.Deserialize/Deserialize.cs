using System.ComponentModel;
using Avro;
using Avro.File;
using Avro.Generic;
using Frends.Avro.Deserialize.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize;

/// <summary>
/// Avro task.
/// </summary>
public class Avro
{
    /// <summary>
    /// Deserialize Avro file to Json string.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
    /// </summary>
    /// <param name="input">Input parameters</param>
    /// <returns>Object { string OutputPath }</returns>
    public static Result Deserialize([PropertyTab] Input input)
    {
        using var dataFileReader = DataFileReader<GenericRecord>.OpenReader(input.AvroFilePath);
        var result = new JArray();

        foreach (var record in dataFileReader.NextEntries)
        {
            var obj = new JObject();
            foreach (var field in record.Schema.Fields)
            {
                var value = record.GetValue(field.Pos);
                var token = value is null ? null : JToken.FromObject(value);
                obj.Add(field.Name, token);
            }

            result.Add(obj);
        }
        return new Result { Json = result };
    }
}
