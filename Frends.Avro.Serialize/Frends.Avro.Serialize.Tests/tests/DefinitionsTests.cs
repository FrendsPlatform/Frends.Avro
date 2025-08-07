using Frends.Avro.Serialize.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frends.Avro.Serialize.Tests.tests;

[TestClass]
public class DefinitionsTests
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void Options_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var options = new Options();

            // Assert
            Assert.IsTrue(options.ThrowErrorOnFailure);
            Assert.IsNull(options.ErrorMessageOnFailure);
        }

        [TestMethod]
        public void Options_CanSetCustomValues()
        {
            // Arrange
            var customErrorMessage = "Custom error message";

            // Act
            var options = new Options
            {
                ThrowErrorOnFailure = false,
                ErrorMessageOnFailure = customErrorMessage
            };

            // Assert
            Assert.IsFalse(options.ThrowErrorOnFailure);
            Assert.AreEqual(customErrorMessage, options.ErrorMessageOnFailure);
        }
    }

    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void Result_CanCreateSuccessResult()
        {
            // Arrange & Act
            var result = new Result
            {
                Success = true,
                FilePath = "/path/to/file.avro",
                Error = null
            };

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("/path/to/file.avro", result.FilePath);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void Result_CanCreateFailureResult()
        {
            // Arrange
            var error = new Error
            {
                Message = "Test error",
                Exception = "Exception details",
                AdditionalInfo = new { TestInfo = "test" }
            };

            // Act
            var result = new Result
            {
                Success = false,
                FilePath = "",
                Error = error
            };

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("", result.FilePath);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual("Test error", result.Error.Message);
        }

        [TestMethod]
        public void Result_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var result = new Result();

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(string.Empty, result.FilePath);
            Assert.IsNull(result.Error);
        }
    }

    [TestClass]
    public class ErrorTests
    {
        [TestMethod]
        public void Error_CanCreateWithAllProperties()
        {
            // Arrange
            var message = "Test error message";
            var exception = "Exception details";
            var additionalInfo = new { Property1 = "value1", Property2 = 42 };

            // Act
            var error = new Error
            {
                Message = message,
                Exception = exception,
                AdditionalInfo = additionalInfo
            };

            // Assert
            Assert.AreEqual(message, error.Message);
            Assert.AreEqual(exception, error.Exception);
            Assert.AreEqual(additionalInfo, error.AdditionalInfo);
        }

        [TestMethod]
        public void Error_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var error = new Error();

            // Assert
            Assert.AreEqual(string.Empty, error.Message);
            Assert.IsNull(error.Exception);
            Assert.IsNull(error.AdditionalInfo);
        }

        [TestMethod]
        public void Error_CanCreateWithMinimalProperties()
        {
            // Arrange & Act
            var error = new Error
            {
                Message = "Simple error message"
            };

            // Assert
            Assert.AreEqual("Simple error message", error.Message);
            Assert.IsNull(error.Exception);
            Assert.IsNull(error.AdditionalInfo);
        }
    }
}
