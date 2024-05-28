using System;

namespace Frends.Avro.Serialize.Exceptions;

#pragma warning disable CS1591 // self explanatory

public class FileAlreadyExistsException : Exception
{
    public FileAlreadyExistsException(string fileName)
        : base($"{fileName} File already exists") { }
}
#pragma warning restore CS1591 // self explanatory
