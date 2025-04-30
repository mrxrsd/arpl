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
        [Fact(DisplayName = "Complex chaining - Mix of Map, Bind, and Match with sync and async")]
        public async Task ComplexChaining_MixOfOperations_ShouldWorkCorrectly()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .Map(x => x + 10)  // sync map: 42 -> 52
                .BindAsync(async x =>
                {
                    await Task.Delay(1);
                    return x % 2 == 0
                        ? SResult<double>.Success(x * 1.5)
                        : SResult<double>.Error(Errors.New("Odd number not allowed"));
                })  // async bind: 52 -> 78.0
                .MapAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.ToString("F1");
                })  // async map: 78.0 -> "78.0"
                .Match(
                    fail => SResult<string>.Error(fail),
                    success => SResult<string>.Success($"Success: {success}"));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Success: 78,0", final.SuccessValue);
        }

        [Fact(DisplayName = "Complex error handling - Mix of sync and async operations")]
        public async Task ComplexErrorHandling_MixOfOperations_ShouldPropagate()
        {
            // Arrange
            var result = SResult<int>.Success(42);
            
            // Act
            var final = await result
                .MapAsync(async x =>
                {
                    await Task.Delay(1);
                    return x * 2;
                })  // async map: 42 -> 84
                .Bind(x => x > 50
                    ? SResult<int>.Error(Errors.New("Value too large"))
                    : SResult<int>.Success(x))  // sync bind: fails with Error
                .Map(x => x.ToString())  // not executed due to Error
                .ApplyAsync(
                    async fail =>
                    {
                        await Task.Delay(1);
                        return SResult<string>.Error(Errors.New($"Validation failed: {fail.Message}"));
                    },
                    async success =>
                    {
                        await Task.Delay(1);
                        return SResult<string>.Success($"Valid: {success}");
                    });
            
            // Assert
            Assert.True(final.IsFail);
            Assert.Equal("Validation failed: Value too large", final.ErrorValue.Message);
        }

        [Fact(DisplayName = "Nested operations - Mix of sync and async with multiple binds")]
        public async Task NestedOperations_MixOfOperations_ShouldWorkCorrectly()
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
                .Bind(x => SResult<(int original, int doubled)>.Success((42, x)))  // sync bind: create tuple
                .MapAsync(async tuple =>
                {
                    await Task.Delay(1);
                    return tuple.doubled / tuple.original;
                })  // async map: calculate ratio
                .Apply(
                    fail => SResult<string>.Error(fail),
                    success => SResult<string>.Success($"Ratio: {success:F1}"));
            
            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("Ratio: 2,0", final.SuccessValue);
        }

        [Fact(DisplayName = "Try - When function succeeds - Should return Success")]
        public void Try_WhenFunctionSucceeds_ShouldReturnSuccess()
        {
            // Arrange
            SResult<int> SafeDivide(int x, int y)
            {
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = SResult<int>.Try(() => SafeDivide(10, 2));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.SuccessValue);
        }

        [Fact(DisplayName = "Try - When function throws - Should return Error")]
        public void Try_WhenFunctionThrows_ShouldReturnError()
        {
            // Arrange
            SResult<int> SafeDivide(int x, int y)
            {
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = SResult<int>.Try(() => SafeDivide(10, 0));

            // Assert
            Assert.True(result.IsFail);
            Assert.Contains("DivideByZeroException", result.ErrorValue.Message);
        }

        [Fact(DisplayName = "TryAsync - When async function succeeds - Should return Success")]
        public async Task TryAsync_WhenFunctionSucceeds_ShouldReturnSuccess()
        {
            // Arrange
            async Task<SResult<int>> SafeDelayedDivide(int x, int y)
            {
                await Task.Delay(1);
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = await SResult<int>.TryAsync(() => SafeDelayedDivide(10, 2));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.SuccessValue);
        }

        [Fact(DisplayName = "TryAsync - When async function throws - Should return Error")]
        public async Task TryAsync_WhenFunctionThrows_ShouldReturnError()
        {
            // Arrange
            async Task<SResult<int>> SafeDelayedDivide(int x, int y)
            {
                await Task.Delay(1);
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = await SResult<int>.TryAsync(() => SafeDelayedDivide(10, 0));

            // Assert
            Assert.True(result.IsFail);
            Assert.Contains("DivideByZeroException", result.ErrorValue.Message);
        }

        [Fact(DisplayName = "Try - Complex chaining with error handling")]
        public void Try_ComplexChaining_WithErrorHandling()
        {
            // Arrange
            SResult<int> SafeDivide(int x, int y)
            {
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            SResult<string> SafeFormat(int x)
            {
                try
                {
                    return SResult<string>.Success($"Result: {x}");
                }
                catch (Exception ex)
                {
                    return SResult<string>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = SResult<int>.Try(() => SafeDivide(10, 2))
                .Map(x => x * 2)
                .Bind(x => SResult<string>.Try(() => SafeFormat(x)));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Result: 10", result.SuccessValue);
        }

        [Fact(DisplayName = "TryAsync - Complex async chaining with error handling")]
        public async Task TryAsync_ComplexChaining_WithErrorHandling()
        {
            // Arrange
            async Task<SResult<int>> SafeDelayedDivide(int x, int y)
            {
                await Task.Delay(1);
                try
                {
                    return SResult<int>.Success(x / y);
                }
                catch (Exception ex)
                {
                    return SResult<int>.Error(Errors.New(ex));
                }
            }

            async Task<SResult<string>> SafeDelayedFormat(int x)
            {
                await Task.Delay(1);
                try
                {
                    return SResult<string>.Success($"Result: {x}");
                }
                catch (Exception ex)
                {
                    return SResult<string>.Error(Errors.New(ex));
                }
            }

            // Act
            var result = await SResult<int>.TryAsync(() => SafeDelayedDivide(10, 2))
                .MapAsync(async x => 
                {
                    await Task.Delay(1);
                    return x * 2;
                })
                .BindAsync(x => SResult<string>.TryAsync(() => SafeDelayedFormat(x)));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Result: 10", result.SuccessValue);
        }
    }
}
