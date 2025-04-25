using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class EitherTests
    {
        [Fact(DisplayName = "Left - Creates Left value with correct properties")]
        public void Left_CreatesLeftValue()
        {
            // Arrange
            var value = "left";

            // Act
            var either = Either<string, int>.Left(value);

            // Assert
            Assert.True(either.IsLeft);
            Assert.False(either.IsRight);
            Assert.Equal(value, either.LeftValue);
            Assert.Equal(default(int), either.RightValue);
        }

        [Fact(DisplayName = "Right - Creates Right value with correct properties")]
        public void Right_CreatesRightValue()
        {
            // Arrange
            var value = 42;

            // Act
            var either = Either<string, int>.Right(value);

            // Assert
            Assert.True(either.IsRight);
            Assert.False(either.IsLeft);
            Assert.Equal(value, either.RightValue);
            Assert.Equal(default(string), either.LeftValue);
        }

        [Fact(DisplayName = "Implicit Conversion - To SResult preserves Right value")]
        public void ImplicitConversion_ToSResult_PreservesRight()
        {
            // Arrange
            var value = 42;
            var either = Either<Error, int>.Right(value);

            // Act
            SResult<int> result = either;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(value, result.SuccessValue);
        }

        [Fact(DisplayName = "Implicit Conversion - To SResult preserves Left value")]
        public void ImplicitConversion_ToSResult_PreservesLeft()
        {
            // Arrange
            var error = Errors.New("Test error");
            var either = Either<Error, int>.Left(error);

            // Act
            SResult<int> result = either;

            // Assert
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }
    }
}
