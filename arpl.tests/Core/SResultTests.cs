using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class SResultTests
    {
        [Fact(DisplayName = "Error - Creates Error result with correct properties")]
        public void Error_CreatesErrorResult()
        {
            // Arrange
            var error = Errors.New("Test error");

            // Act
            var result = SResult<int>.Error(error);

            // Assert
            Assert.True(result.IsFail);
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.ErrorValue);
            Assert.Equal(default(int), result.SuccessValue);
        }

        [Fact(DisplayName = "Success - Creates Success result with correct properties")]
        public void Success_CreatesSuccessResult()
        {
            // Arrange
            var value = 42;

            // Act
            var result = SResult<int>.Success(value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFail);
            Assert.Equal(value, result.SuccessValue);
            Assert.Null(result.ErrorValue);
        }

        [Fact(DisplayName = "Implicit Conversion - To Either preserves Success value")]
        public void ImplicitConversion_ToEither_PreservesSuccess()
        {
            // Arrange
            var value = 42;
            var result = SResult<int>.Success(value);

            // Act
            Either<Error, int> either = result;

            // Assert
            Assert.True(either.IsRight);
            Assert.Equal(value, either.RightValue);
        }

        [Fact(DisplayName = "Implicit Conversion - To Either preserves Error value")]
        public void ImplicitConversion_ToEither_PreservesError()
        {
            // Arrange
            var error = Errors.New("Test error");
            var result = SResult<int>.Error(error);

            // Act
            Either<Error, int> either = result;

            // Assert
            Assert.True(either.IsLeft);
            Assert.Equal(error, either.LeftValue);
        }
    }
}
