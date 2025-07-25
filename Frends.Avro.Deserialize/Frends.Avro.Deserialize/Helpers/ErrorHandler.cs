using System;
using Frends.Avro.Deserialize.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Helpers;

/// <summary>
/// Provides centralized error handling functionality for Avro deserialization operations.
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Handles exceptions that occur during Avro deserialization based on the configured options.
    /// Either throws the exception or returns a Result object with error information.
    /// </summary>
    /// <param name="ex">The exception that occurred during deserialization.</param>
    /// <param name="options">Configuration options that determine how errors should be handled.</param>
    /// <returns>A Result object containing error information when ThrowErrorOnFailure is false.</returns>
    /// <exception cref="Exception">Thrown when ThrowErrorOnFailure is true, either the original exception or a wrapped exception with custom message.</exception>
    /// <remarks>
    /// When ThrowErrorOnFailure is true, this method will throw either the original exception or wrap it with a custom message if provided.
    /// When ThrowErrorOnFailure is false, this method returns a Result object with Success=false and populated Error information.
    /// </remarks>
    public static Result Handle(Exception ex, Options options)
    {
        if (options.ThrowErrorOnFailure)
        {
            if (!string.IsNullOrEmpty(options.ErrorMessageOnFailure))
            {
                throw new Exception(options.ErrorMessageOnFailure, ex);
            }
            throw ex;
        }

        var errorMessage = string.IsNullOrEmpty(options.ErrorMessageOnFailure)
            ? ex.Message
            : options.ErrorMessageOnFailure;

        var error = new Error
        {
            Message = errorMessage,
            AdditionalInfo = ex.GetType().Name
        };

        return new Result { Json = JToken.FromObject(new { Error = error }), Success = false, Error = error };
    }
}
