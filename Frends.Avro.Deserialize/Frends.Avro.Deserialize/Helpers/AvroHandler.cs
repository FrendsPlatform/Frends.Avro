using System.IO;
using System.Linq;
using System.Threading;
using Avro;
using Avro.File;
using Avro.Generic;
using Avro.IO;
using Frends.Avro.Deserialize.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Helpers;

internal static class AvroHandler
{
    internal static Result DeserializeObjectContainer(
        Stream stream,
        Schema readerSchema,
        CancellationToken cancellationToken)
    {
        using var dataFileReader = readerSchema is null
            ? DataFileReader<GenericRecord>.OpenReader(stream, leaveOpen: true)
            : DataFileReader<GenericRecord>.OpenReader(stream, readerSchema, leaveOpen: true);
        var result = new JArray();

        foreach (var record in dataFileReader.NextEntries)
        {
            result.Add(ToJsonObject(record));
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }

        return new Result
        {
            Json = result,
            Success = true,
            Error = null
        };
    }

    internal static Result DeserializeRawFile(
        Stream stream,
        Schema schema,
        CancellationToken cancellationToken)
    {
        var reader = new GenericReader<GenericRecord>(schema, schema);
        var decoder = new BinaryDecoder(stream);
        var result = new JArray();

        while (stream.Position < stream.Length)
        {
            var record = reader.Read(null, decoder);
            result.Add(ToJsonObject(record));

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }

        return new Result
        {
            Json = result,
            Success = true,
            Error = null
        };
    }

    internal static bool IsObjectContainer(FileStream stream)
    {
        var header = new byte[DataFileConstants.Magic.Length];
        var bytesRead = stream.Read(header, 0, header.Length);
        stream.Position = 0;

        return bytesRead == DataFileConstants.Magic.Length &&
               header.SequenceEqual(DataFileConstants.Magic);
    }

    private static JObject ToJsonObject(GenericRecord record)
    {
        var obj = new JObject();
        foreach (var field in record.Schema.Fields)
        {
            var value = record.GetValue(field.Pos);
            obj.Add(field.Name, value is null ? null : JToken.FromObject(value));
        }

        return obj;
    }
}
