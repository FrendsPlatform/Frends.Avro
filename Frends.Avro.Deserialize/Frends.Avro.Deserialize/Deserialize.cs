using System.ComponentModel;
using System.Threading;
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
    /// Deserialize Avro file to JSON string.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
    /// </summary>
    /// <param name="input">Input parameters</param>
    /// <param name="CancellationToken">CancellationToken from Frends</param>
    /// <returns>Object { string OutputPath }</returns>
    public static Result Deserialize([PropertyTab] Input input, CancellationToken CancellationToken)
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
            if(CancellationToken.IsCancellationRequested){
                break;
            }
        }
        return new Result { Json = result };
    }
}
