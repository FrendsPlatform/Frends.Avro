using System;
using System.IO;

namespace Frends.Avro.Serialize.Tests.asserts;

public static class FileAsserts
{
    public static void FileExists(this Assert assert, string filePath)
    {
        Assert.IsTrue(File.Exists(filePath));
    }

    public static void FileIsNotEmpty(this Assert assert, string filePath)
    {
        var fileContent = File.ReadAllBytes(filePath);
        Assert.AreNotEqual(Array.Empty<byte>(), fileContent);
    }
}
