using System;
using System.IO;
using Frends.Avro.Deserialize.Definitions;

namespace Frends.Avro.Deserialize.Tests.tests;

[TestClass]
public class Tests : TestsBase
{
    [TestMethod]
    public void Deserialize()
    {
        var result = Avro.Deserialize(
            new Input { AvroFilePath = Path.Combine(testFileParentPath, "test.avro") }
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
            }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(OverflowException))]
    public void ThrowIfFileIsCorrupted()
    {
        Avro.Deserialize(
            new Input { AvroFilePath = Path.Combine(testFileParentPath, "test-invalid.avro") }
        );
    }
}
