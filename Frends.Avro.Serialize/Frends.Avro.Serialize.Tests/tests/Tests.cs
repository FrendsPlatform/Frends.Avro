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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "InvalidDirectory", "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
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
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options()
        );
    }

    [TestMethod]
    public void DoNotThrowErrorWhenThrowErrorOnFailureIsFalse()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = "InvalidJson",
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false }
        );
        
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
    }

    [TestMethod]
    public void UseCustomErrorMessageWhenProvided()
    {
        var customErrorMessage = "Custom error occurred";
        var result = Avro.Serialize(
            new Input
            {
                Json = "InvalidJson",
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options 
            { 
                ThrowErrorOnFailure = false,
                ErrorMessageOnFailure = customErrorMessage
            }
        );
        
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(customErrorMessage, result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }
}
