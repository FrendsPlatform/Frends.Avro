using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Avro;
using Avro.File;
using Avro.Generic;
using Avro.IO;
using Frends.Avro.Deserialize.Definitions;
using Frends.Avro.Deserialize.Helpers;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize;

/// <summary>
/// Provides functionality for deserializing Avro files and raw Avro payloads to JSON format.
/// </summary>
public static class Avro
{
    /// <summary>
    /// Deserializes an Avro object container file or a raw Avro binary file to JSON format.
    /// Reads all records from the file and converts them to a JSON array.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
    /// </summary>
    /// <param name="input">Input parameters containing an Avro file path and an optional schema for raw Avro data.</param>
    /// <param name="options">Configuration options for the deserialization operation.</param>
    /// <param name="cancellationToken">Cancellation token from Frends platform for operation cancellation.</param>
    /// <returns>A Result object containing the deserialized JSON data, success status, and error information if applicable.</returns>
    public static Result Deserialize([PropertyTab] Input input, [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            using var fileStream = File.OpenRead(input.FilePath);
            var isObjectContainer = AvroHandler.IsObjectContainer(fileStream);

            if (isObjectContainer)
            {
                var readerSchema = string.IsNullOrWhiteSpace(input.SchemaJson)
                    ? null
                    : Schema.Parse(input.SchemaJson);
                return AvroHandler.DeserializeObjectContainer(fileStream, readerSchema, cancellationToken);
            }

            if (string.IsNullOrWhiteSpace(input.SchemaJson))
            {
                throw new InvalidDataException(
                    "The file contains raw Avro binary data. SchemaJson is required because raw Avro data does not contain a schema.");
            }

            return AvroHandler.DeserializeRawFile(fileStream, Schema.Parse(input.SchemaJson), cancellationToken);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options);
        }
    }
}
