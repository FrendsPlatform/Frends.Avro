using System;
using Frends.Avro.Deserialize.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Helpers;

/// <summary>
/// Static helper class for handling errors in Avro deserialization operations.
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Handles exceptions based on the provided options.
    /// </summary>
    /// <param name="ex">The exception that occurred</param>
    /// <param name="options">Options that determine error handling behavior</param>
    /// <returns>Result object with error information if not throwing, otherwise throws the exception</returns>
    public static Result Handle(Exception ex, Options options)
    {
        if (options.ThrowErrorOnFailure)
        {
            if (!string.IsNullOrEmpty(options.ErrorMessageOnFailure))
            {
                throw new Exception(options.ErrorMessageOnFailure, ex);
            }
            throw;
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
