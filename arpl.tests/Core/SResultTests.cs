using Xunit;
using Arpl;
using Arpl.Core;
using System;
using System.Threading.Tasks;

namespace Arpl.Tests.Core
{
    public class SResultTests
    {
        [Fact]
        public void Map_TransformsSuccessValue()
        {
            var result = SResult<int>.Success(5);
            var mapped = result.Map(i => i * 10);
            Assert.True(mapped.IsSuccess);
            Assert.Equal(50, mapped.SuccessValue);
        }

        [Fact]
        public void Map_DoesNotTransformFail()
        {
            var error = Errors.New("fail");
            var result = SResult<int>.Error(error);
            var mapped = result.Map(i => i * 10);
            Assert.True(mapped.IsFail);
            Assert.Equal(error, mapped.ErrorValue);
        }

        [Fact]
        public async Task MatchAsync_ResolvesSuccessAsync()
        {
            var result = SResult<int>.Success(7);
            var value = await result.MatchAsync(
                fail => Task.FromResult(-1),
                success => Task.FromResult(success * 2));
            Assert.Equal(14, value);
        }

        [Fact]
        public async Task MatchAsync_ResolvesFailAsync()
        {
            var error = Errors.New("fail");
            var result = SResult<int>.Error(error);
            var value = await result.MatchAsync(
                fail => Task.FromResult(-1),
                success => Task.FromResult(success * 2));
            Assert.Equal(-1, value);
        }
        [Fact]
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
          
        }

        [Fact]
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
        

       
        [Fact]
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

        [Fact]
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
