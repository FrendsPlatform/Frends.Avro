using System;
using Frends.Avro.Serialize.Definitions;

namespace Frends.Avro.Serialize.Helpers;

/// <summary>
/// Static class for handling errors in Avro serialization operations.
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Handles exceptions based on the provided options.
    /// </summary>
    /// <param name="exception">The exception that occurred during serialization.</param>
    /// <param name="options">Options that determine how errors should be handled.</param>
    /// <returns>A Result object with error information if ThrowErrorOnFailure is false, otherwise re-throws the exception.</returns>
    /// <exception cref="Exception">Re-throws the original exception if ThrowErrorOnFailure is true.</exception>
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
