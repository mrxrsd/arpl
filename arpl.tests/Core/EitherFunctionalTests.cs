using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class EitherFunctionalTests
    {
        [Fact(DisplayName = "Match - When Either is Left - Should execute left function")]
        public void Match_WhenLeft_ShouldExecuteLeftFunction()
        {
            // Arrange
            var either = Either<string, int>.Left("error");
            
            // Act
            var result = either.Match(
                left => $"Left: {left}",
                right => $"Right: {right}");
            
            // Assert
            Assert.Equal("Left: error", result);
        }

        [Fact(DisplayName = "Match - When Either is Right - Should execute right function")]
        public void Match_WhenRight_ShouldExecuteRightFunction()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = either.Match(
                left => $"Left: {left}",
                right => $"Right: {right}");
            
            // Assert
            Assert.Equal("Right: 42", result);
        }

        [Fact(DisplayName = "MatchAsync - When Either is Left - Should execute left function")]
        public async Task MatchAsync_WhenLeft_ShouldExecuteLeftFunction()
        {
            // Arrange
            var either = Either<string, int>.Left("error");
            
            // Act
            var result = await either.MatchAsync(
                async left => { await Task.Delay(1); return $"Left: {left}"; },
                async right => { await Task.Delay(1); return $"Right: {right}"; });
            
            // Assert
            Assert.Equal("Left: error", result);
        }

        [Fact(DisplayName = "Map - When Either is Right - Should transform value")]
        public void Map_WhenRight_ShouldTransformValue()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = either.Map(x => x * 2);
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(84, result.RightValue);
        }

        [Fact(DisplayName = "Map - When Either is Left - Should preserve left value")]
        public void Map_WhenLeft_ShouldPreserveLeftValue()
        {
            // Arrange
            var error = "error";
            var either = Either<string, int>.Left(error);
            
            // Act
            var result = either.Map(x => x * 2);
            
            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal(error, result.LeftValue);
        }

        [Fact(DisplayName = "MapAsync - When Either is Right - Should transform value")]
        public async Task MapAsync_WhenRight_ShouldTransformValue()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = await either.MapAsync(async x => 
            {
                await Task.Delay(1);
                return x * 2;
            });
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(84, result.RightValue);
        }

        [Fact(DisplayName = "Apply - Should allow chaining transformations")]
        public void Apply_ShouldAllowChainingTransformations()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = either.Apply(
                left => Either<string, string>.Left(left),
                right => Either<string, string>.Right($"Value: {right}"));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Value: 42", result.RightValue);
        }

        [Fact(DisplayName = "ApplyAsync - Should allow async chaining transformations")]
        public async Task ApplyAsync_ShouldAllowAsyncChainingTransformations()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = await either.ApplyAsync(
                async left => { await Task.Delay(1); return Either<string, string>.Left(left); },
                async right => { await Task.Delay(1); return Either<string, string>.Right($"Value: {right}"); });
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Value: 42", result.RightValue);
        }

        [Fact(DisplayName = "Bind - Should allow chaining operations")]
        public void Bind_ShouldAllowChaining()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = either
                .Bind(x => Either<string, int>.Right(x * 2))
                .Bind(x => Either<string, string>.Right($"Final: {x}"));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Final: 84", result.RightValue);
        }

        [Fact(DisplayName = "BindAsync - Should allow async chaining operations")]
        public async Task BindAsync_ShouldAllowAsyncChaining()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = await either
                .BindAsync(async x => 
                {
                    await Task.Delay(1);
                    return Either<string, int>.Right(x * 2);
                })
                .BindAsync(async x =>
                {
                    await Task.Delay(1);
                    return Either<string, string>.Right($"Final: {x}");
                });
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Final: 84", result.RightValue);
        }

        [Fact(DisplayName = "BindAsync - Should allow chaining without await")]
        public async Task BindAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = Either<string, int>.Right(42);
            
            // Act
            var final = await result
                .BindAsync(x => Task.FromResult(Either<string, int>.Right(x * 2)))
                .BindAsync(x => Task.FromResult(Either<string, string>.Right($"Final: {x}")));
            
            // Assert
            Assert.True(final.IsRight);
            Assert.Equal("Final: 84", final.RightValue);
        }

        [Fact(DisplayName = "MapAsync - Should allow chaining without await")]
        public async Task MapAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = Either<string, int>.Right(42);
            
            // Act
            var final = await result
                .MapAsync(x => Task.FromResult(x * 2))
                .MapAsync(x => Task.FromResult($"Final: {x}"));
            
            // Assert
            Assert.True(final.IsRight);
            Assert.Equal("Final: 84", final.RightValue);
        }

        [Fact(DisplayName = "ApplyAsync - Should allow chaining without await")]
        public async Task ApplyAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = Either<string, int>.Right(42);
            
            // Act
            var final = await result
                .ApplyAsync(
                    left => Task.FromResult(Either<string, int>.Left(left)),
                    right => Task.FromResult(Either<string, int>.Right(right * 2)))
                .ApplyAsync(
                    left => Task.FromResult(Either<string, string>.Left(left)),
                    right => Task.FromResult(Either<string, string>.Right($"Final: {right}")));
            
            // Assert
            Assert.True(final.IsRight);
            Assert.Equal("Final: 84", final.RightValue);
        }
    }
}
