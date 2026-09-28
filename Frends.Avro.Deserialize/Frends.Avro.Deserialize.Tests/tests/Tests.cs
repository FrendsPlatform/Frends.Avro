using System;
using System.IO;
using System.Threading;
using Frends.Avro.Deserialize.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Tests.tests;

[TestClass]
public class Tests : TestsBase
{
    [TestMethod]
    public void RawFileWithSchemaIsDeserialized()
    {
        const string payload =
            "EEpvaG4gRG9lAQAAAAAAAJBlQBBOZXcgWW9ya+F6FK6nAMRAEEpvaG4gRG9lAQAAAAAAAJBlQBBOZXcgWW9ya+F6FK6nAMRA";
        const string schemaJson = """
        {
          "type": "record",
          "name": "Record",
          "fields": [
            { "name": "name", "type": "string" },
            { "name": "isHuman", "type": "boolean" },
            { "name": "age", "type": ["null", "long"] },
            { "name": "height", "type": "double" },
            { "name": "city", "type": "string" },
            { "name": "balance", "type": "double" }
          ]
        }
        """;
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.avro");

        try
        {
            File.WriteAllBytes(tempPath, Convert.FromBase64String(payload));

            var result = Avro.Deserialize(
                new Input { FilePath = tempPath, SchemaJson = schemaJson },
                new Options(),
                CancellationToken.None
            );

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Error);
            Assert.AreEqual(2, result.Json.Count);
            Assert.AreEqual("John Doe", result.Json[0]["name"].ToString());
            Assert.AreEqual(true, (bool)result.Json[0]["isHuman"]);
            Assert.AreEqual(JTokenType.Null, result.Json[0]["age"].Type);
            Assert.AreEqual(172.5, (double)result.Json[0]["height"]);
            Assert.AreEqual("New York", result.Json[0]["city"].ToString());
            Assert.AreEqual(10241.31, (double)result.Json[0]["balance"]);
            Assert.AreEqual("John Doe", result.Json[1]["name"].ToString());
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [TestMethod]
    public void RawFileWithoutSchemaReturnsError()
    {
        const string payload =
            "EEpvaG4gRG9lAQAAAAAAAJBlQBBOZXcgWW9ya+F6FK6nAMRA";
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.avro");

        try
        {
            File.WriteAllBytes(tempPath, Convert.FromBase64String(payload));

            var result = Avro.Deserialize(
                new Input { FilePath = tempPath },
                new Options { ThrowErrorOnFailure = false },
                CancellationToken.None
            );

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(
                "The file contains raw Avro binary data. SchemaJson is required because raw Avro data does not contain a schema.",
                result.Error.Message
            );
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [TestMethod]
    public void ObjectContainerFileWithReaderSchemaUsesDefaultForMissingField()
    {
        const string readerSchema = """
        {
          "type": "record",
          "name": "Record",
          "fields": [
            { "name": "name", "type": "string" },
            { "name": "newField", "type": ["null", "string"], "default": null }
          ]
        }
        """;

        var result = Avro.Deserialize(
            new Input
            {
                FilePath = Path.Combine(testFileParentPath, "test.avro"),
                SchemaJson = readerSchema
            },
            new Options(),
            CancellationToken.None
        );

        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Error);
        Assert.AreEqual("John Doe", result.Json[0]["name"].ToString());
        Assert.AreEqual(JTokenType.Null, result.Json[0]["newField"].Type);

        const string incompatibleReaderSchema = """
        {
          "type": "record",
          "name": "Record",
          "fields": [
            { "name": "name", "type": "int" }
          ]
        }
        """;

        var incompatibleResult = Avro.Deserialize(
            new Input
            {
                FilePath = Path.Combine(testFileParentPath, "test.avro"),
                SchemaJson = incompatibleReaderSchema
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(incompatibleResult.Success);
        Assert.IsNotNull(incompatibleResult.Error);
        Assert.IsFalse(string.IsNullOrWhiteSpace(incompatibleResult.Error.Message));
    }

    [TestMethod]
    public void ObjectContainerFileWithoutSchemaUsesEmbeddedSchema()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test.avro") },
            new Options(),
            CancellationToken.None
        );

        Assert.AreEqual(ExpectedResult.ToString(), result.Json.ToString());
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Error);
    }

    [TestMethod]
    public void ThrowIfFileDoesNotExists()
    {
        Assert.ThrowsException<FileNotFoundException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = Path.Combine(testFileParentPath, "ThisFileShouldNotExist.avro") },
                new Options(),
                CancellationToken.None
            );
        });
    }

    [TestMethod]
    public void ThrowIfFileIsCorrupted()
    {
        var sourcePath = Path.Combine(testFileParentPath, "test-invalid.avro");
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".avro");
        File.Copy(sourcePath, tempFile, overwrite: true);

        try
        {
            Assert.ThrowsException<OverflowException>(() =>
            {
                Avro.Deserialize(
                    new Input { FilePath = tempFile },
                    new Options(),
                    CancellationToken.None
                );
            });
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete temp file: {ex.Message}");
                }
            }
        }
    }

    [TestMethod]
    public void DefaultOptionsThrowErrorOnFailureIsTrue()
    {
        var options = new Options();

        Assert.ThrowsException<FileNotFoundException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = Path.Combine(testFileParentPath, "NonExistentFile.avro") },
                options,
                CancellationToken.None
            );
        });
    }

    [TestMethod]
    public void EmptyFilePathThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = "" },
                new Options(),
                CancellationToken.None
            );
        });
    }

    [TestMethod]
    public void NullFilePathThrowsException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = null },
                new Options(),
                CancellationToken.None
            );
        });
    }

    [TestMethod]
    public void SuccessfulDeserializationWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test.avro") },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.AreEqual(ExpectedResult.ToString(), result.Json.ToString());
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Error);
    }

    [TestMethod]
    public void ErrorMessageOnFailureIsIgnoredOnSuccess()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test.avro") },
            new Options
            {
                ThrowErrorOnFailure = false,
                ErrorMessageOnFailure = "This should be ignored"
            },
            CancellationToken.None
        );

        Assert.AreEqual(ExpectedResult.ToString(), result.Json.ToString());
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Error);
    }

    [TestMethod]
    public void DoNotThrowIfFileIsCorruptedAndThrowErrorOnFailureIsFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("Arithmetic operation resulted in an overflow.", result.Error.Message);
    }

    [TestMethod]
    public void UseCustomErrorMessageForCorruptedFile()
    {
        var customMessage = "File is corrupted and cannot be processed";
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
            new Options
            {
                ThrowErrorOnFailure = false,
                ErrorMessageOnFailure = customMessage
            },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        StringAssert.Contains(result.Error.Message, customMessage);
    }

    [TestMethod]
    public void EmptyFilePathWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = "" },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("ArgumentException", result.Error.AdditionalInfo);
    }
}
