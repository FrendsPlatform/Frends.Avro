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

    protected string Schema =
        @"{
  ""type"": ""record"",
  ""name"": ""Record"",
  ""fields"": [
    {
      ""name"": ""name"",
      ""type"": ""string""
    },
    {
      ""name"": ""isHuman"",
      ""type"": ""boolean""
    },
    {
      ""name"": ""age"",
      ""type"": [""null"", ""long""]
    },
    {
      ""name"": ""height"",
      ""type"": ""double""
    },
    {
      ""name"": ""city"",
      ""type"": ""string""
    },
    {
      ""name"": ""balance"",
      ""type"": ""double""
    },
    {
      ""name"": ""children"",
      ""type"": {
        ""type"": ""array"",
        ""items"": ""string""
      }
    },
    {
      ""name"": ""partner"",
      ""type"": {
        ""name"": ""Partner"",
        ""type"": ""record"",
        ""fields"": [
          {
            ""name"": ""name"",
            ""type"": ""string""
          },
          {
            ""name"": ""age"",
            ""type"": ""long""
          }
        ]
      }
    }
  ]
}";
    protected string JsonWithArray =
        @"[{
            ""name"": ""John Doe"",
            ""isHuman"": true,
            ""age"": null,
            ""height"": 172.5,
            ""city"": ""New York"",
            ""balance"": 10241.31,
            ""children"": [""Junior"", ""Junior II""],
            ""partner"": {
                ""name"": ""Marry Jane"",
                ""age"": 20
            }
        }]";
    protected string JsonWithoutName => JsonWithArray.Replace(@"""name"": ""John Doe"",", "");
    protected string JsonWithObject => JsonWithArray.TrimStart('[').TrimEnd(']');
}
