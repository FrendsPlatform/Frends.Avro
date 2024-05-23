namespace Frends.Avro.Serialize.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Operation complete without errors.
    /// </summary>
    /// <example>true</example>
    public byte[] Avro { get; private set; }

    internal Result(byte[] avro)
    {
        Avro = avro;
    }
}