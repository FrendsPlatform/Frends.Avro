using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Avro;
using Avro.File;
using Avro.Generic;
using Frends.Avro.Serialize.Definitions;
using Frends.Avro.Serialize.Exceptions;
using Frends.Avro.Serialize.Helpers;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Serialize;

/// <summary>
/// Main class containing Avro serialization functionality.
/// Provides methods to serialize JSON data into Avro binary format using Apache Avro library.
/// </summary>
public class Avro
{
    /// <summary>
    /// Serializes JSON data into Avro binary format and saves it to a file.
    /// Supports both single JSON objects and arrays of objects. The JSON structure must match the provided Avro schema.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Serialize)
    /// </summary>
    /// <param name="input">Input parameters containing the JSON data to serialize, Avro schema definition, and target file path.</param>
    /// <param name="options">Configuration options for error handling behavior and custom error messages.</param>
    /// <returns>
    /// A Result object containing:
    /// - Success: Boolean indicating if serialization completed successfully
    /// - FilePath: Path to the created Avro file (empty on failure)
    /// - Error: Detailed error information if serialization failed and ThrowErrorOnFailure is false
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when required fields are missing from JSON data.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the target directory does not exist.</exception>
    /// <exception cref="FileAlreadyExistsException">Thrown when the target file already exists.</exception>
    /// <exception cref="Newtonsoft.Json.JsonReaderException">Thrown when JSON or schema is invalid.</exception>
    /// <example>
    /// var input = new Input
    /// {
    ///     Json = @"{ ""name"": ""John"", ""age"": 30 }",
    ///     Schema = @"{ ""type"": ""record"", ""name"": ""Person"", ""fields"": [...] }",
    ///     TargetFilePath = @"C:\output\data.avro"
    /// };
    /// var options = new Options { ThrowErrorOnFailure = false };
    /// var result = Avro.Serialize(input, options);
    /// </example>
    public static Result Serialize([PropertyTab] Input input, [PropertyTab] Options options)
    {
        try
        {
            ValidateInputParameters(input);

            var jToken = JToken.Parse(input.Json);
            if (jToken is not JArray)
                jToken = new JArray(jToken);
            var avroSchema = (RecordSchema)Schema.Parse(input.Schema);

            WriteAvroFile(input.TargetFilePath, avroSchema, jToken);

            return new Result { Success = true, FilePath = input.TargetFilePath };
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options);
        }
    }

    /// <summary>
    /// Validates input parameters to ensure the target directory exists and file doesn't already exist.
    /// </summary>
    /// <param name="input">Input parameters to validate.</param>
    /// <exception cref="DirectoryNotFoundException">Thrown when target directory doesn't exist.</exception>
    /// <exception cref="FileAlreadyExistsException">Thrown when target file already exists.</exception>
    private static void ValidateInputParameters(Input input)
    {
        var fileInfo = new FileInfo(input.TargetFilePath);
        if (!fileInfo.Directory.Exists)
            throw new DirectoryNotFoundException();
        if (fileInfo.Exists)
            throw new FileAlreadyExistsException(input.TargetFilePath);
    }

    /// <summary>
    /// Writes JSON data to an Avro file using the specified schema.
    /// </summary>
    /// <param name="dstPath">Destination file path for the Avro file.</param>
    /// <param name="schema">Avro record schema to use for serialization.</param>
    /// <param name="json">JSON data to serialize (can be single object or array).</param>
    private static void WriteAvroFile(string dstPath, RecordSchema schema, JToken json)
    {
        using var fileWriter = DataFileWriter<GenericRecord>.OpenWriter(
            new GenericWriter<GenericRecord>(schema),
            dstPath
        );
        foreach (var recordJToken in json)
        {
            var record = JTokenToGenericRecord(recordJToken, schema);
            fileWriter.Append(record);
        }
    }

    /// <summary>
    /// Converts a JSON token to an Avro GenericRecord based on the provided schema.
    /// Recursively handles nested record structures.
    /// </summary>
    /// <param name="jToken">JSON token to convert.</param>
    /// <param name="avroSchema">Avro record schema defining the structure.</param>
    /// <returns>GenericRecord containing the converted data.</returns>
    /// <exception cref="ArgumentException">Thrown when required fields are missing from JSON.</exception>
    static GenericRecord JTokenToGenericRecord(JToken jToken, RecordSchema avroSchema)
    {
        var genericRecord = new GenericRecord(avroSchema);
        foreach (var field in avroSchema.Fields)
        {
            var fieldName = field.Name;
            var fieldSchema = field.Schema;

            if (fieldSchema is RecordSchema recordSchema)
            {
                var fieldValue = JTokenToGenericRecord(jToken[fieldName], recordSchema);
                genericRecord.Add(fieldName, fieldValue);
            }
            else
            {
                if (jToken[fieldName] == null)
                    throw new ArgumentException($"Field '{fieldName}' is missing in the JSON.");
                var csharpType = AvroTypeToCSharpType(fieldSchema.Tag);
                var fieldValue = jToken[fieldName].ToObject(csharpType);
                genericRecord.Add(fieldName, fieldValue);
            }
        }
        return genericRecord;
    }

    /// <summary>
    /// Maps Avro schema types to corresponding C# types for JSON deserialization.
    /// </summary>
    /// <param name="avroType">Avro schema type to convert.</param>
    /// <returns>Corresponding C# type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported Avro type is encountered.</exception>
    private static Type AvroTypeToCSharpType(Schema.Type avroType) =>
        avroType switch
        {
            Schema.Type.Null => typeof(object),
            Schema.Type.Boolean => typeof(bool),
            Schema.Type.Int => typeof(int),
            Schema.Type.Long => typeof(long),
            Schema.Type.Float => typeof(float),
            Schema.Type.Double => typeof(double),
            Schema.Type.Bytes => typeof(byte[]),
            Schema.Type.String => typeof(string),
            Schema.Type.Array => typeof(object[]),
            Schema.Type.Map => typeof(Dictionary<string, object>),
            Schema.Type.Record => typeof(GenericRecord),
            Schema.Type.Enumeration => typeof(string),
            Schema.Type.Fixed => typeof(byte[]),
            Schema.Type.Union => typeof(object),
            _ => throw new ArgumentOutOfRangeException(nameof(avroType), avroType, null)
        };
}
