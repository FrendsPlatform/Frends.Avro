using System;
using Frends.Avro.Serialize.Definitions;

namespace Frends.Avro.Serialize.Helpers;

/// <summary>
/// Static utility class for handling errors in Avro serialization operations.
/// Provides centralized error handling logic based on configuration options.
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Handles exceptions that occur during Avro serialization based on the provided configuration options.
    /// Either re-throws the exception or returns a Result object with error details.
    /// </summary>
    /// <param name="exception">The exception that occurred during the serialization process.</param>
    /// <param name="options">Configuration options that determine how errors should be handled.</param>
    /// <returns>
    /// A Result object containing error information if ThrowErrorOnFailure is false.
    /// If ThrowErrorOnFailure is true, the method re-throws the original exception.
    /// </returns>
    /// <exception cref="Exception">Re-throws the original exception if ThrowErrorOnFailure option is set to true.</exception>
    /// <example>
    /// var result = ErrorHandler.Handle(new ArgumentException("Invalid data"), options);
    /// if (!result.Success)
    /// {
    ///     Console.WriteLine($"Error: {result.Error.Message}");
    /// }
    /// </example>
    public static Result Handle(Exception exception, Options options)
    {
        if (options.ThrowErrorOnFailure)
            throw exception;

        var errorMessage = string.IsNullOrEmpty(options.ErrorMessageOnFailure)
            ? exception.Message
            : options.ErrorMessageOnFailure;

        return new Result
        {
            Success = false,
            FilePath = "",
            Error = new Error
            {
                Message = errorMessage,
                AdditionalInfo = new { ExceptionType = exception.GetType().Name, StackTrace = exception.StackTrace }
            }
        };
    }
}
