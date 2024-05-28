using System.Threading;
using Frends.Avro.Serialize.Definitions;
using Frends.AzureDataLake.DownloadFiles.Tests.tests;

namespace Frends.Avro.Serialize.Tests;

[TestClass]
public class Tests : TestsBase
{
    [TestMethod]
    public void Serialize()
    {
        var input = new Input
        {
            Json = "[{ \"name\": \"John Doe\", \"age\": 42, \"city\": \"New York\" }]",
            Schema =
                @"{
                ""type"":""record"",
                ""name"":""Person"",
                ""fields"":[
                    {""name"":""name"",""type"":""string""},
                    {""name"":""age"",""type"":""int""},
                    {""name"":""city"",""type"":""string""}
                ]
            }",
            OutputDir = testDirectory,
            OutputFile = "file.avro"
        };

        var result = Avro.Serialize(input, CancellationToken.None);
    }

    [TestMethod]
    public void ThrowIfDirectoryNofFound() { }

    [TestMethod]
    public void ThrowIfFileAlreadyExists() { }

    [TestMethod]
    public void ThrowIfSchemaIsInvalid() { }

    [TestMethod]
    public void ThrowIfJsonIsInvalid() { }
}
