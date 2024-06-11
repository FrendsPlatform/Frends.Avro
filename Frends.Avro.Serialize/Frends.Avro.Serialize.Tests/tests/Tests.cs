using System;
using System.IO;
using Frends.Avro.Serialize.Definitions;
using Frends.Avro.Serialize.Exceptions;
using Frends.Avro.Serialize.Tests.asserts;
using Frends.AzureDataLake.DownloadFiles.Tests.tests;
using Newtonsoft.Json;

namespace Frends.Avro.Serialize.Tests.tests;

[TestClass]
public class Tests : TestsBase
{
    [TestMethod]
    public void SerializeJArray()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
        Assert.That.FileExists(result.FilePath);
        Assert.That.FileIsNotEmpty(result.FilePath);
    }

    [TestMethod]
    public void SerializeJObject()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithObject,
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
        Assert.That.FileExists(result.FilePath);
        Assert.That.FileIsNotEmpty(result.FilePath);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowIfRequiredFieldIsMissing()
    {
        Avro.Serialize(
            new Input
            {
                Json = JsonWithoutName,
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(DirectoryNotFoundException))]
    public void ThrowIfDirectoryNofFound()
    {
        Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "InvalidDirectory", "test.avro"),
            }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(FileAlreadyExistsException))]
    public void ThrowIfFileAlreadyExists()
    {
        using var file = File.Create(Path.Combine(testDirectory, "test.avro"));
        Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(JsonReaderException))]
    public void ThrowIfSchemaIsInvalid()
    {
        Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = "InvalidSchema",
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(JsonReaderException))]
    public void ThrowIfJsonIsInvalid()
    {
        Avro.Serialize(
            new Input
            {
                Json = "InvalidJson",
                Schema = Schema,
                OutputPath = Path.Combine(testDirectory, "test.avro"),
            }
        );
    }
}
