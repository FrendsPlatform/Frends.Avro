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
            new Input { AvroFilePath = Path.Combine(testFileParentPath, "test.avro") },
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
                AvroFilePath = Path.Combine(testFileParentPath, "ThisFileShoulNotExist.avro")
            },
            CancellationToken.None
        );
    }

    [TestMethod]
    [ExpectedException(typeof(OverflowException))]
    public void ThrowIfFileIsCorrupted()
    {
        Avro.Deserialize(
            new Input { AvroFilePath = Path.Combine(testFileParentPath, "test-invalid.avro") },
            CancellationToken.None
        );
    }
}
