using Xunit;
using System.Linq;
using System;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class ErrorTests
    {
        [Fact]
        public void HasErrorOf_SingleExpectedError_ReturnsTrue()
        {
            // Arrange
            var error = Errors.New("Test error");

            // Act & Assert
            Assert.True(error.HasErrorOf<ExpectedError>());
            Assert.False(error.HasErrorOf<UnexpectedError>());
        }

        [Fact]
        public void HasErrorOf_SingleUnexpectedError_ReturnsTrue()
        {
            // Arrange
            var error = Errors.New(new Exception(), "Test error");

            // Act & Assert
            Assert.True(error.HasErrorOf<UnexpectedError>());
            Assert.False(error.HasErrorOf<ExpectedError>());
        }

        [Fact]
        public void HasErrorOf_ErrorCollection_FindsExpectedError()
        {
            // Arrange
            var error1 = Errors.New("Expected error");
            var error2 = Errors.New(new Exception(), "Unexpected error");
            var collection = error1 + error2;

            // Act & Assert
            Assert.True(collection.HasErrorOf<ExpectedError>());
            Assert.True(collection.HasErrorOf<UnexpectedError>());
        }

        [Fact]
        public void HasErrorOf_EmptyCollection_ReturnsFalse()
        {
            // Arrange
            var collection = Errors.EmptyError();

            // Act & Assert
            Assert.False(collection.HasErrorOf<ExpectedError>());
            Assert.False(collection.HasErrorOf<UnexpectedError>());
        }

        [Fact]
        public void Exception_ExpectedError_ReturnsNull()
        {
            // Arrange
            var error = Errors.New("Test error");

            // Act & Assert
            Assert.Null(error.Exception);
        }

        [Fact]
        public void Exception_UnexpectedError_ReturnsException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var error = Errors.New(exception);

            // Act & Assert
            Assert.Same(exception, error.Exception);
        }

        [Fact]
        public void Exception_ErrorCollection_ReturnsAggregateException()
        {
            // Arrange
            var exception1 = new Exception("Test exception 1");
            var exception2 = new Exception("Test exception 2");
            var error1 = Errors.New(exception1);
            var error2 = Errors.New(exception2);
            var collection = error1 + error2;

            // Act
            var aggregateException = Assert.IsType<AggregateException>(collection.Exception);

            // Assert
            Assert.Collection(aggregateException.InnerExceptions,
                ex => Assert.Same(exception1, ex),
                ex => Assert.Same(exception2, ex));
        }

        [Fact]
        public void Exception_ErrorCollectionWithoutExceptions_ReturnsNull()
        {
            // Arrange
            var error1 = Errors.New("Test error 1");
            var error2 = Errors.New("Test error 2");
            var collection = error1 + error2;

            // Act & Assert
            Assert.Null(collection.Exception);
        }
        [Fact]
        public void New_CreatesExpectedError()
        {
            // Arrange
            var message = "Test error";
            var code = "TEST001";

            // Act
            var error = Errors.New(message, code);

            // Assert
            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.Code);
            Assert.True(error.IsExpected);
            Assert.Equal(1, error.Count);
        }

        [Fact]
        public void EmptyError_ReturnsEmptyError()
        {
            // Act
            var error = Errors.EmptyError();

            // Assert
            Assert.Equal("[]", error.Message);
            Assert.Equal("[]", error.Code);
            Assert.True(error.IsExpected);
            Assert.Equal(0, error.Count);
        }

        [Fact]
        public void NewUnexpected_CreatesUnexpectedError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var message = "Test error";
            var code = "TEST001";

            // Act
            var error = Errors.New(exception, message, code);

            // Assert
            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.Code);
            Assert.False(error.IsExpected);
            Assert.Equal(1, error.Count);
        }

        [Fact]
        public void ErrorCombination_CombinesTwoErrors()
        {
            // Arrange
            var error1 = Errors.New("Error 1", "E1");
            var error2 = Errors.New("Error 2", "E2");

            // Act
            var combined = error1 + error2;

            // Assert
            Assert.Equal(2, combined.Count);
            var errors = combined.AsEnumerable().ToList();
            Assert.Contains(error1, errors);
            Assert.Contains(error2, errors);
        }

        [Fact]
        public void ErrorEnumeration_EnumeratesAllErrors()
        {
            // Arrange
            var error1 = Errors.New("Error 1", "E1");
            var error2 = Errors.New("Error 2", "E2");
            var combined = error1 + error2;

            // Act & Assert
            var errors = combined.AsEnumerable().ToList();
            Assert.Equal(2, errors.Count);
            Assert.Contains(error1, errors);
            Assert.Contains(error2, errors);
        }
    }
}
