using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Avro;
using Frends.Avro.Deserialize.Definitions;
using Frends.Avro.Deserialize.Helpers;

namespace Frends.Avro.Deserialize;

/// <summary>
/// Provides functionality for deserializing Avro files and raw Avro payloads to JSON format.
/// </summary>
public static class Avro
{
    /// <summary>
    /// Deserializes an Avro object container file or raw Avro binary data to JSON format.
    /// The data can be read from a file path or provided directly as a byte array.
    /// Reads all records and converts them to a JSON array.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
    /// </summary>
    /// <param name="input">Input parameters containing the Avro data source (file path or byte array) and an optional schema for raw Avro data.</param>
    /// <param name="options">Configuration options for the deserialization operation.</param>
    /// <param name="cancellationToken">Cancellation token from Frends platform for operation cancellation.</param>
    /// <returns>A Result object containing the deserialized JSON data, success status, and error information if applicable.</returns>
    public static Result Deserialize([PropertyTab] Input input, [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            using var fileStream = OpenInputStream(input);
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

    private static Stream OpenInputStream(Input input)
    {
        return input.DataSource switch
        {
            DataSource.FilePath => File.OpenRead(input.FilePath),
            DataSource.FileBytes => new MemoryStream(
                input.FileBytes ?? throw new ArgumentNullException(nameof(input.FileBytes)),
                writable: false),
            _ => throw new ArgumentOutOfRangeException(nameof(input.DataSource), input.DataSource,
                "Unsupported data source."),
        };
    }
}
