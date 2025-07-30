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
