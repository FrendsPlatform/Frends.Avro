using System;

namespace Frends.Avro.Serialize.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a file that already exists.
/// Used in Avro serialization to prevent accidental file overwrites.
/// </summary>
public class FileAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the FileAlreadyExistsException class with the specified file name.
    /// </summary>
    /// <param name="fileName">The name of the file that already exists.</param>
    /// <example>
    /// throw new FileAlreadyExistsException("C:\\data\\output.avro");
    /// </example>
    public FileAlreadyExistsException(string fileName)
        : base($"{fileName} File already exists") { }
}
