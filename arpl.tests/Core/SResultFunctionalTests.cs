using Xunit;
using System.Threading.Tasks;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class SResultFunctionalTests
    {
        [Fact(DisplayName = "Match - When SResult is Success - Should execute success function")]
        public void Match_WhenSuccess_ShouldExecuteSuccessFunction()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = result.Match(
                fail => $"Error: {fail}",
                success => $"Success: {success}");
            
            // Assert
            Assert.Equal("Success: 42", transformed);
        }

        [Fact(DisplayName = "Match - When SResult is Fail - Should execute fail function")]
        public void Match_WhenFail_ShouldExecuteFailFunction()
        {
            // Arrange
            var error = Errors.New("Test error");
            var result = SResult<int>.Error(error);
            
            // Act
            var transformed = result.Match(
                fail => $"Error: {fail}",
                success => $"Success: {success}");
            
            // Assert
            Assert.Equal($"Error: {error}", transformed);
        }

        [Fact(DisplayName = "MatchAsync - When SResult is Success - Should execute async success function")]
        public async Task MatchAsync_WhenSuccess_ShouldExecuteSuccessFunction()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = await result.MatchAsync(
                async fail => { await Task.Delay(1); return $"Error: {fail}"; },
                async success => { await Task.Delay(1); return $"Success: {success}"; });
            
            // Assert
            Assert.Equal("Success: 42", transformed);
        }

        [Fact(DisplayName = "Map - When SResult is Success - Should transform value")]
        public void Map_WhenSuccess_ShouldTransformValue()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = result.Map(x => x * 2);
            
            // Assert
            Assert.True(transformed.IsSuccess);
            Assert.Equal(84, transformed.SuccessValue);
        }

        [Fact(DisplayName = "Map - When SResult is Fail - Should preserve error")]
        public void Map_WhenFail_ShouldPreserveError()
        {
            // Arrange
            var error = Errors.New("Test error");
            var result = SResult<int>.Error(error);
            
            // Act
            var transformed = result.Map(x => x * 2);
            
            // Assert
            Assert.True(transformed.IsFail);
            Assert.Equal(error, transformed.ErrorValue);
        }

        [Fact(DisplayName = "MapAsync - When SResult is Success - Should transform value")]
        public async Task MapAsync_WhenSuccess_ShouldTransformValue()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = await result.MapAsync(async x => 
            {
                await Task.Delay(1);
                return x * 2;
            });
            
            // Assert
            Assert.True(transformed.IsSuccess);
            Assert.Equal(84, transformed.SuccessValue);
        }

        [Fact(DisplayName = "Apply - Should allow chaining transformations")]
        public void Apply_ShouldAllowChainingTransformations()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = result.Apply(
                fail => SResult<string>.Error(fail),
                success => SResult<string>.Success($"Value: {success}"));
            
            // Assert
            Assert.True(transformed.IsSuccess);
            Assert.Equal("Value: 42", transformed.SuccessValue);
        }

        [Fact(DisplayName = "ApplyAsync - Should allow async chaining transformations")]
        public async Task ApplyAsync_ShouldAllowAsyncChainingTransformations()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var transformed = await result.ApplyAsync(
                async fail => { await Task.Delay(1); return SResult<string>.Error(fail); },
                async success => { await Task.Delay(1); return SResult<string>.Success($"Value: {success}"); });
            
            // Assert
            Assert.True(transformed.IsSuccess);
            Assert.Equal("Value: 42", transformed.SuccessValue);
        }

        [Fact(DisplayName = "Bind - Should allow chaining operations")]
        public void Bind_ShouldAllowChaining()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = result
                .Bind(x => SResult<int>.Success(x * 2))
                .Bind(x => SResult<string>.Success($"Final: {x}"));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Final: 84", final.SuccessValue);
        }

        [Fact(DisplayName = "BindAsync - Should allow async chaining operations")]
        public async Task BindAsync_ShouldAllowAsyncChaining()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .BindAsync(async x =>
                {
                    await Task.Delay(1);
                    return SResult<int>.Success(x * 2);
                })
                .BindAsync(async x =>
                {
                    await Task.Delay(1);
                    return SResult<string>.Success($"Final: {x}");
                });
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Final: 84", final.SuccessValue);
        }

        [Fact(DisplayName = "BindAsync - Should allow chaining without await")]
        public async Task BindAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .BindAsync(x => Task.FromResult(SResult<int>.Success(x * 2)))
                .BindAsync(x => Task.FromResult(SResult<string>.Success($"Final: {x}")));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Final: 84", final.SuccessValue);
        }

        [Fact(DisplayName = "MapAsync - Should allow chaining without await")]
        public async Task MapAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .MapAsync(x => Task.FromResult(x * 2))
                .MapAsync(x => Task.FromResult($"Final: {x}"));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Final: 84", final.SuccessValue);
        }

        [Fact(DisplayName = "ApplyAsync - Should allow chaining without await")]
        public async Task ApplyAsync_ShouldAllowChainingWithoutAwait()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .ApplyAsync(
                    fail => Task.FromResult(SResult<int>.Error(fail)),
                    success => Task.FromResult(SResult<int>.Success(success * 2)))
                .ApplyAsync(
                    fail => Task.FromResult(SResult<string>.Error(fail)),
                    success => Task.FromResult(SResult<string>.Success($"Final: {success}")));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Final: 84", final.SuccessValue);
        }
    }
}
