using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Avro;
using Avro.File;
using Avro.Generic;
using Frends.Avro.Serialize.Definitions;
using Frends.Avro.Serialize.Exceptions;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Serialize;

/// <summary>
/// Avro task.
/// </summary>
public class Avro
{
    /// <summary>
    /// Serialize JSON into Avro.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Serialize)
    /// </summary>
    /// <param name="input">Input parameters</param>
    /// <returns>Object { string OutputPath }</returns>
    public static Result Serialize([PropertyTab] Input input)
    {
        ValidateInputParameters(input);

        var jToken = JToken.Parse(input.Json);
        if (jToken is not JArray)
            jToken = new JArray(jToken);
        var avroSchema = (RecordSchema)Schema.Parse(input.Schema);

        WriteAvroFile(input.OutputPath, avroSchema, jToken);

        return new Result { FilePath = input.OutputPath };
    }

    private static void ValidateInputParameters(Input input)
    {
        var fileInfo = new FileInfo(input.OutputPath);
        if (!fileInfo.Directory.Exists)
            throw new DirectoryNotFoundException();
        if (fileInfo.Exists)
            throw new FileAlreadyExistsException(input.OutputPath);
    }

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
