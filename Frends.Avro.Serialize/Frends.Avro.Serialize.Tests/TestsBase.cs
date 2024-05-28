using System;
using System.IO;

namespace Frends.AzureDataLake.DownloadFiles.Tests.tests;

[TestClass]
public abstract class TestsBase
{
    protected string testDirectory;
    protected static readonly string file1 = "foobar1.avro";

    [TestInitialize]
    public void TestSetup()
    {
        testDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"test-{Guid.NewGuid()}");
        Directory.CreateDirectory(testDirectory);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Directory.Delete(testDirectory, true);
    }
}
