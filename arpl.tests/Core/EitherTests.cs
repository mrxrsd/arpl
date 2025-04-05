using Xunit;
using Arpl.Core;
using System.Threading.Tasks;

namespace Arpl.Tests.Core
{
    public class EitherTests
    {
        [Fact]
        public void Map_TransformsRightValue()
        {
            var either = Either<string, int>.Right(10);
            var mapped = either.Map(i => i * 2);
            Assert.True(mapped.IsRight);
            Assert.Equal(20, mapped.RightValue);
        }

        [Fact]
        public void Map_DoesNotTransformLeftValue()
        {
            var either = Either<string, int>.Left("fail");
            var mapped = either.Map(i => i * 2);
            Assert.True(mapped.IsLeft);
            Assert.Equal("fail", mapped.LeftValue);
        }

        [Fact]
        public async Task MatchAsync_ResolvesRightAsync()
        {
            var either = Either<string, int>.Right(5);
            var result = await either.MatchAsync(
                left => Task.FromResult(-1),
                right => Task.FromResult(right * 3));
            Assert.Equal(15, result);
        }

        [Fact]
        public async Task MatchAsync_ResolvesLeftAsync()
        {
            var either = Either<string, int>.Left("fail");
            var result = await either.MatchAsync(
                left => Task.FromResult(-1),
                right => Task.FromResult(right * 3));
            Assert.Equal(-1, result);
        }
        [Fact]
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

        [Fact]
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

       

        [Fact]
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

        [Fact]
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
