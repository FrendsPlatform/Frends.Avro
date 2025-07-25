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
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void ThrowIfFileDoesNotExists()
    {
        Avro.Deserialize(
            new Input
            {
                FilePath = Path.Combine(testFileParentPath, "ThisFileShouldNotExist.avro")
            },
            new Options(),
            CancellationToken.None
        );
    }

    [TestMethod]
    [ExpectedException(typeof(OverflowException))]
    public void ThrowIfFileIsCorrupted()
    {
        Avro.Deserialize(
            new Input { FilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
            new Options(),
            CancellationToken.None
        );
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
        Assert.IsTrue(result.Json.ToString().Contains(customMessage));
    }
}
