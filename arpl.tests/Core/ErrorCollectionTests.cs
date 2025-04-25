using Xunit;
using System.Linq;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class ErrorCollectionTests
    {
        [Fact]
        public void Add_WithValidError_AddsErrorToCollection()
        {
            // Arrange
            var collection = Errors.EmptyError();
            var error = Errors.New("Test error", "TEST001");

            // Act
            collection.Add(error);

            // Assert
            Assert.Single(collection.Errors);
            Assert.Equal(error, collection.Errors.First());
            Assert.Equal(1, collection.Count);
            Assert.Equal("[TEST001]", collection.Code);
            Assert.Equal("[Test error]", collection.Message);
        }

        [Fact]
        public void Add_WithNullError_IgnoresTheError()
        {
            // Arrange
            var collection = new ErrorCollection();

            // Act
            collection.Add(null);

            // Assert
            Assert.Empty(collection.Errors);
            Assert.Equal(0, collection.Count);
            Assert.Equal("[]", collection.Code);
            Assert.Equal("[]", collection.Message);
        }

        [Fact]
        public void Add_MultipleErrors_CombinesErrorsCorrectly()
        {
            // Arrange
            var collection = new ErrorCollection();
            var error1 = Errors.New("Error 1", "E1");
            var error2 = Errors.New("Error 2", "E2");

            // Act
            collection.Add(error1);
            collection.Add(error2);

            // Assert
            Assert.Equal(2, collection.Errors.Count);
            Assert.Equal(2, collection.Count);
            Assert.Equal("[E1, E2]", collection.Code);
            Assert.Equal("[Error 1, Error 2]", collection.Message);
            Assert.Contains(error1, collection.Errors);
            Assert.Contains(error2, collection.Errors);
        }

        [Fact]
        public void Add_WithMixedExpectedStatus_UpdatesIsExpectedCorrectly()
        {
            // Arrange
            var collection = new ErrorCollection();
            var expectedError = Errors.New("Expected error", "E1");
            var unexpectedError = Errors.New(new System.Exception(), "Unexpected error", "E2");

            // Act
            collection.Add(expectedError);
            collection.Add(unexpectedError);

            // Assert
            Assert.False(collection.IsExpected);
            Assert.Equal(2, collection.Count);
            Assert.Equal("[E1, E2]", collection.Code);
            Assert.Equal("[Expected error, Unexpected error]", collection.Message);
        }
    }
}
