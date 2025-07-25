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
/// Provides functionality for deserializing Avro files to JSON format.
/// </summary>
public class Avro
{
    /// <summary>
    /// Deserializes an Avro file to JSON format.
    /// Reads all records from the specified Avro file and converts them to a JSON array.
    /// Each record in the Avro file becomes a JSON object in the resulting array.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
    /// </summary>
    /// <param name="input">Input parameters containing the file path to the Avro file.</param>
    /// <param name="options">Configuration options for the deserialization operation.</param>
    /// <param name="CancellationToken">Cancellation token from Frends platform for operation cancellation.</param>
    /// <returns>A Result object containing the deserialized JSON data, success status, and error information if applicable.</returns>
    /// <exception cref="Exception">Thrown when ThrowErrorOnFailure is true and deserialization fails.</exception>
    /// <example>
    /// var input = new Input { FilePath = @"C:\data\sample.avro" };
    /// var options = new Options { ThrowErrorOnFailure = true };
    /// var result = Avro.Deserialize(input, options, CancellationToken.None);
    /// </example>
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
