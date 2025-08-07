using System;
using Frends.Avro.Serialize.Definitions;
using Frends.Avro.Serialize.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frends.Avro.Serialize.Tests.tests;

[TestClass]
public class ErrorHandlerTests
{
    [TestMethod]
    public void Handle_ThrowErrorOnFailureTrue_RethrowsException()
    {
        // Arrange
        var exception = new ArgumentException("Test exception");
        var options = new Options { ThrowErrorOnFailure = true };

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => ErrorHandler.Handle(exception, options));
    }

    [TestMethod]
    public void Handle_ThrowErrorOnFailureFalse_ReturnsResultWithError()
    {
        // Arrange
        var exception = new ArgumentException("Test exception");
        var options = new Options { ThrowErrorOnFailure = false };

        // Act
        var result = ErrorHandler.Handle(exception, options);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("Test exception", result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void Handle_WithCustomErrorMessage_UsesCustomMessage()
    {
        // Arrange
        var exception = new ArgumentException("Original exception message");
        var customMessage = "Custom error message";
        var options = new Options
        {
            ThrowErrorOnFailure = false,
            ErrorMessageOnFailure = customMessage
        };

        // Act
        var result = ErrorHandler.Handle(exception, options);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual(customMessage, result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void Handle_WithEmptyCustomErrorMessage_UsesOriginalMessage()
    {
        // Arrange
        var exception = new ArgumentException("Original exception message");
        var options = new Options
        {
            ThrowErrorOnFailure = false,
            ErrorMessageOnFailure = ""
        };

        // Act
        var result = ErrorHandler.Handle(exception, options);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("Original exception message", result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void Handle_WithNullCustomErrorMessage_UsesOriginalMessage()
    {
        // Arrange
        var exception = new ArgumentException("Original exception message");
        var options = new Options
        {
            ThrowErrorOnFailure = false,
            ErrorMessageOnFailure = null
        };

        // Act
        var result = ErrorHandler.Handle(exception, options);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("", result.FilePath);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("Original exception message", result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    [TestMethod]
    public void Handle_AdditionalInfoContainsExceptionDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var options = new Options { ThrowErrorOnFailure = false };

        // Act
        var result = ErrorHandler.Handle(exception, options);

        // Assert
        Assert.IsNotNull(result.Error?.AdditionalInfo);
        var additionalInfo = result.Error.AdditionalInfo as dynamic;
        Assert.IsNotNull(additionalInfo);
        Assert.AreEqual("InvalidOperationException", additionalInfo.ExceptionType);
        Assert.IsNotNull(additionalInfo.StackTrace);
    }
}
