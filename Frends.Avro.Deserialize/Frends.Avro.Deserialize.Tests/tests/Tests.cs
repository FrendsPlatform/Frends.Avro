using System;
using System.IO;
using System.Threading;
using Frends.Avro.Deserialize.Definitions;

namespace Frends.Avro.Deserialize.Tests.tests;

[TestClass]
public class Tests : TestsBase
{
    [TestMethod]
    public void Deserialize()
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
                new Input
                {
                    FilePath = Path.Combine(testFileParentPath, "ThisFileShouldNotExist.avro")
                },
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
            new Options { ThrowErrorOnFailure = false, ErrorMessageOnFailure = "This should be ignored" },
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
            new Options { ThrowErrorOnFailure = false, ErrorMessageOnFailure = customMessage },
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
