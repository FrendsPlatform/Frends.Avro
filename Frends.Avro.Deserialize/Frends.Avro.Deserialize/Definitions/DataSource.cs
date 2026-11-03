namespace Frends.Avro.Deserialize.Definitions;

/// <summary>
/// Defines how the Avro data is provided to the Task.
/// </summary>
public enum DataSource
{
    /// <summary>
    /// Avro data is read from the file defined in FilePath.
    /// </summary>
    FilePath,

    /// <summary>
    /// Avro data is provided directly as a byte array in FileBytes.
    /// </summary>
    FileBytes,
}
