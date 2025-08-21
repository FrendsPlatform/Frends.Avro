using System;
using System.IO;
using System.Threading;
using Frends.Avro.Serialize.Definitions;
using Frends.Avro.Serialize.Exceptions;
using Frends.Avro.Serialize.Tests.asserts;
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
            new Options(),
            CancellationToken.None
        );
        Assert.IsTrue(result.Success);
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
            new Options(),
            CancellationToken.None
        );
        Assert.IsTrue(result.Success);
        Assert.That.FileExists(result.FilePath);
        Assert.That.FileIsNotEmpty(result.FilePath);
    }

    [TestMethod]
    public void ThrowIfRequiredFieldIsMissing()
    {
        try
        {
            Avro.Serialize(
                new Input
                {
                    Json = JsonWithoutName,
                    Schema = Schema,
                    TargetFilePath = Path.Combine(testDirectory, "test.avro"),
                },
                new Options { ThrowErrorOnFailure = true },
                CancellationToken.None
            );

            Assert.Fail("Expected an exception, but none was thrown.");
        }
        catch (Exception ex)
        {

            Assert.IsTrue(ex.Message.Contains("Field 'name' is missing in the JSON."));

            Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Expected ArgumentException as inner exception.");
        }
    }

    [TestMethod]
    public void ThrowIfDirectoryNotFound()
    {
        try
        {
            Avro.Serialize(
                new Input
                {
                    Json = JsonWithArray,
                    Schema = Schema,
                    TargetFilePath = Path.Combine(testDirectory, "InvalidDirectory", "test.avro"),
                },
                new Options { ThrowErrorOnFailure = true },
                CancellationToken.None
            );

            Assert.Fail("Expected an exception, but none was thrown.");
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(DirectoryNotFoundException), "Expected DirectoryNotFoundException as inner exception.");
        }
    }

    [TestMethod]
    public void ThrowIfFileAlreadyExists()
    {
        try
        {
            using var file = File.Create(Path.Combine(testDirectory, "test.avro"));
            Avro.Serialize(
                new Input
                {
                    Json = JsonWithArray,
                    Schema = Schema,
                    TargetFilePath = Path.Combine(testDirectory, "test.avro"),
                },
                new Options { ThrowErrorOnFailure = true },
                CancellationToken.None
            );

            Assert.Fail("Expected an exception, but none was thrown.");
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(FileAlreadyExistsException), "Expected FileAlreadyExistsException as inner exception.");
        }
    }

    [TestMethod]
    public void ThrowIfSchemaIsInvalid()
    {
        try
        {
            Avro.Serialize(
                new Input
                {
                    Json = JsonWithArray,
                    Schema = "InvalidSchema",
                    TargetFilePath = Path.Combine(testDirectory, "test.avro"),
                },
                new Options { ThrowErrorOnFailure = true },
                CancellationToken.None
            );

            Assert.Fail("Expected an exception, but none was thrown.");
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(JsonReaderException), "Expected JsonReaderException as inner exception.");
        }
    }

    [TestMethod]
    public void ThrowIfJsonIsInvalid()
    {
        try
        {
            Avro.Serialize(
                new Input
                {
                    Json = "InvalidJson",
                    Schema = Schema,
                    TargetFilePath = Path.Combine(testDirectory, "test.avro"),
                },
                new Options { ThrowErrorOnFailure = true },
                CancellationToken.None
            );

            Assert.Fail("Expected an exception, but none was thrown.");
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(JsonReaderException), "Expected JsonReaderException as inner exception.");
        }
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
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
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
            },
            CancellationToken.None

        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(result.Error.Message.Contains(customErrorMessage));
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void HandleFileAlreadyExistsWithThrowErrorOnFailureFalse()
    {
        using var file = File.Create(Path.Combine(testDirectory, "test.avro"));
        file.Close();

        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(result.Error.Message.Contains("already exists"));
    }

    [TestMethod]
    public void HandleDirectoryNotFoundWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "InvalidDirectory", "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);

    }

    [TestMethod]
    public void HandleMissingRequiredFieldWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithoutName,
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsNotNull(result.Error.Message);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
    }

    [TestMethod]
    public void HandleInvalidSchemaWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = "InvalidSchema",
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsNotNull(result.Error.Message);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
    }

    [TestMethod]
    public void OptionsCustomValues()
    {
        var customErrorMessage = "Custom error message";
        var options = new Options
        {
            ThrowErrorOnFailure = false,
            ErrorMessageOnFailure = customErrorMessage
        };

        Assert.IsFalse(options.ThrowErrorOnFailure);
        Assert.AreEqual(customErrorMessage, options.ErrorMessageOnFailure);
    }

    [TestMethod]
    public void ResultSuccessProperties()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = JsonWithArray,
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options(),
            CancellationToken.None
        );

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.FilePath);
        Assert.IsTrue(result.FilePath.EndsWith("test.avro"));
        Assert.IsNull(result.Error);
    }

    [TestMethod]
    public void ResultFailureProperties()
    {
        var result = Avro.Serialize(
            new Input
            {
                Json = "InvalidJson",
                Schema = Schema,
                TargetFilePath = Path.Combine(testDirectory, "test.avro"),
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.IsNotNull(result.Error.Message);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
    }

    [TestMethod]
    public void SerializeWithEmptyCustomErrorMessage()
    {
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
                ErrorMessageOnFailure = ""
            },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void SerializeWithNullCustomErrorMessage()
    {
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
                ErrorMessageOnFailure = null
            },
            CancellationToken.None
        );

        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Error.Message));
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }
}
