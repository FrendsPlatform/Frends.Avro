using System;

namespace Frends.Avro.Serialize.Exceptions;

/// <summary>
/// Exception thrown when file already exists in container.
/// </summary>
public class FileAlreadyExistsException : Exception
{
    /// <summary>
    /// Exceptions constructor
    /// </summary>
    public FileAlreadyExistsException(string fileName)
        : base($"{fileName} already exists in container") { }
}
