using System;
using System.ComponentModel;
using System.Threading;
using Avro;
using Avro.File;
using Avro.Generic;
using Frends.Avro.Deserialize.Definitions;
using Frends.Avro.Deserialize.Helpers;
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
    /// <param name="options">Options for deserialization</param>
    /// <param name="CancellationToken">CancellationToken from Frends</param>
    /// <returns>Object { dynamic Json }</returns>
    public static Result Deserialize([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken CancellationToken)
    {
        try
        {
            using var dataFileReader = DataFileReader<GenericRecord>.OpenReader(input.FilePath);
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
                if (CancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
            return new Result { Json = result, Success = true, Error = null };
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options);
        }
    }
}
