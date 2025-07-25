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
        Assert.ThrowsException<OverflowException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
                new Options(),
                CancellationToken.None
            );
        });
    }

    [TestMethod]
    public void DoNotThrowIfFileDoesNotExistsAndThrowErrorOnFailureIsFalse()
    {
        var result = Avro.Deserialize(
            new Input
            {
                FilePath = Path.Combine(testFileParentPath, "ThisFileShouldNotExist.avro")
            },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsTrue(result.Json.ToString().Contains("Message"));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("FileNotFoundException", result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void UseCustomErrorMessageWhenProvided()
    {
        var customMessage = "Custom error occurred";
        var result = Avro.Deserialize(
            new Input
            {
                FilePath = Path.Combine(testFileParentPath, "ThisFileShouldNotExist.avro")
            },
            new Options { ThrowErrorOnFailure = false, ErrorMessageOnFailure = customMessage },
            CancellationToken.None
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsTrue(result.Json.ToString().Contains(customMessage));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(customMessage, result.Error.Message);
    }

    [TestMethod]
    public void DoNotThrowIfFileIsCorruptedAndThrowErrorOnFailureIsFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsTrue(result.Json.ToString().Contains("Message"));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("OverflowException", result.Error.AdditionalInfo);
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

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsTrue(result.Json.ToString().Contains(customMessage));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(customMessage, result.Error.Message);
    }

    [TestMethod]
    public void DefaultOptionsThrowErrorOnFailureIsTrue()
    {
        var options = new Options();

        // Verify default behavior throws exception
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
    public void EmptyFilePathWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = "" },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("ArgumentException", result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void NullFilePathWithThrowErrorOnFailureFalse()
    {
        var result = Avro.Deserialize(
            new Input { FilePath = null },
            new Options { ThrowErrorOnFailure = false },
            CancellationToken.None
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("ArgumentNullException", result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void CancellationTokenIsRespected()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        Assert.ThrowsException<OperationCanceledException>(() =>
        {
            Avro.Deserialize(
                new Input { FilePath = Path.Combine(testFileParentPath, "test.avro") },
                new Options(),
                cts.Token
            );
        });
    }

    [TestMethod]
    public void CancellationTokenWithThrowErrorOnFailureFalse()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var result = Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test.avro") },
            new Options { ThrowErrorOnFailure = false },
            cts.Token
        );

        Assert.IsNotNull(result.Json);
        Assert.IsTrue(result.Json.ToString().Contains("Error"));
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("OperationCanceledException", result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void SuccessfulDeserializationWithDefaultOptions()
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
}
