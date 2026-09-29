using System.Collections;
using Avro.Generic;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Helpers;

internal static class JsonHandler
{
    internal static JObject ToJsonObject(GenericRecord record)
    {
        var obj = new JObject();
        AddRecordFields(obj, record, null);

        return obj;
    }

    private static void AddRecordFields(JObject obj, GenericRecord record, string prefix)
    {
        foreach (var field in record.Schema.Fields)
        {
            var fieldName = prefix is null ? field.Name : $"{prefix}.{field.Name}";
            var value = record.GetValue(field.Pos);

            if (value is GenericRecord nestedRecord)
            {
                AddRecordFields(obj, nestedRecord, fieldName);
            }
            else
            {
                obj.Add(fieldName, ToJsonToken(value));
            }
        }
    }

    private static JToken ToJsonToken(object value)
    {
        switch (value)
        {
            case null:
                return JValue.CreateNull();
            case GenericRecord record:
                return ToJsonObject(record);
            case IDictionary dictionary:
                {
                    var obj = new JObject();
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        obj.Add(entry.Key.ToString() ?? "", ToJsonToken(entry.Value));
                    }

                    return obj;
                }
        }

        if (value is not (IEnumerable values and not string and not byte[])) return JToken.FromObject(value);

        var array = new JArray();
        foreach (var item in values)
        {
            array.Add(ToJsonToken(item));
        }

        return array;
    }
}
