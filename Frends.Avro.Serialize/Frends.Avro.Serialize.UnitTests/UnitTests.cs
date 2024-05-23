using Frends.Avro.Serialize.Definitions;
using NUnit.Framework;

namespace Frends.Avro.Serialize.Tests;

[TestFixture]
public class UnitTests
{
    [Test]
    public void SerializeWithSchema()
    {
        var input = new Input()
        {
            Json = "[{ \"name\": \"John Doe\", \"age\": 42, \"city\": \"New York\" }]",
            Schema = @"{
                ""type"":""record"",
                ""name"":""Person"",
                ""fields"":[
                    {""name"":""name"",""type"":""string""},
                    {""name"":""age"",""type"":""int""},
                    {""name"":""city"",""type"":""string""}
                ]
            }"
        };

        var result = Avro.Serialize(input, default).Result;
        Assert.IsNotNull(result.Avro);
    }
}