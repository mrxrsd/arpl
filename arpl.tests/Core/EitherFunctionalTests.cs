using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class EitherFunctionalTests
    {
        [Fact]
        public void Do_WithRight_ExecutesFunction()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            var executed = false;

            // Act
            var result = either.Do(e => {
                executed = true;
                Assert.True(e.IsRight);
                Assert.Equal(42, e.RightValue);
                return e;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsRight);
            Assert.Equal(42, result.RightValue);
        }

        [Fact]
        public void Do_WithLeft_ExecutesFunction()
        {
            // Arrange
            var either = Either<string, int>.Left("error");
            var executed = false;

            // Act
            var result = either.Do(e => {
                executed = true;
                Assert.True(e.IsLeft);
                Assert.Equal("error", e.LeftValue);
                return e;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsLeft);
            Assert.Equal("error", result.LeftValue);
        }

        [Fact]
        public async Task DoAsync_WithRight_ExecutesFunction()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            var executed = false;

            // Act
            var result = await either.DoAsync(async e => {
                executed = true;
                Assert.True(e.IsRight);
                Assert.Equal(42, e.RightValue);
                await Task.Delay(1); // Simulate async work
                return e;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsRight);
            Assert.Equal(42, result.RightValue);
        }

        [Fact]
        public async Task DoAsync_WithLeft_ExecutesFunction()
        {
            // Arrange
            var either = Either<string, int>.Left("error");
            var executed = false;

            // Act
            var result = await either.DoAsync(async e => {
                executed = true;
                Assert.True(e.IsLeft);
                Assert.Equal("error", e.LeftValue);
                await Task.Delay(1); // Simulate async work
                return e;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsLeft);
            Assert.Equal("error", result.LeftValue);
        }

        [Fact]
        public void Do_ChainedWithMap_ExecutesInOrder()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            var order = new List<string>();

            // Act
            var result = either
                .Do(e => {
                    order.Add("First");
                    return e;
                })
                .Map(x => {
                    order.Add("Second");
                    return x * 2;
                })
                .Do(e => {
                    order.Add("Third");
                    return e;
                });

            // Assert
            Assert.Equal(new[] { "First", "Second", "Third" }, order);
            Assert.True(result.IsRight);
            Assert.Equal(84, result.RightValue);
        }

        [Fact]
        public async Task DoAsync_ChainedWithMapAsync_ExecutesInOrder()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            var order = new List<string>();

            // Act
            var result = await either
                .DoAsync(async e => {
                    order.Add("First");
                    await Task.Delay(1);
                    return e;
                })
                .MapAsync(async x => {
                    order.Add("Second");
                    await Task.Delay(1);
                    return x * 2;
                })
                .DoAsync(async e => {
                    order.Add("Third");
                    await Task.Delay(1);
                    return e;
                });

            // Assert
            Assert.Equal(new[] { "First", "Second", "Third" }, order);
            Assert.True(result.IsRight);
            Assert.Equal(84, result.RightValue);
        }

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
        [Fact(DisplayName = "Complex chaining - Mix of Map, Bind, and Match with sync and async")]
        public async Task ComplexChaining_MixOfOperations_ShouldWorkCorrectly()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = await either
                .Map(x => x + 10)  // sync map: 42 -> 52
                .BindAsync(async x =>
                {
                    await Task.Delay(1);
                    return x % 2 == 0
                        ? Either<string, double>.Right(x * 1.5)
                        : Either<string, double>.Left("Odd number not allowed");
                })  // async bind: 52 -> 78.0
                .MapAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                })  // async map: 78.0 -> "78.0"
                .Match(
                    left => Either<string, string>.Left($"Error: {left}"),
                    right => Either<string, string>.Right($"Success: {right}"));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Success: 78.0", result.RightValue);
        }

        [Fact(DisplayName = "Complex error handling - Mix of sync and async operations")]
        public async Task ComplexErrorHandling_MixOfOperations_ShouldPropagate()
        {
            // Arrange
            var either = Either<string, int>.Right(42);
            
            // Act
            var result = await either
                .MapAsync(async x =>
                {
                    await Task.Delay(1);
                    return x * 2;
                })  // async map: 42 -> 84
                .Bind(x => x > 50
                    ? Either<string, int>.Left("Value too large")
                    : Either<string, int>.Right(x))  // sync bind: fails with Left
                .Map(x => x.ToString())  // not executed due to Left
                .ApplyAsync(
                    async left =>
                    {
                        await Task.Delay(1);
                        return Either<string, string>.Left($"Validation failed: {left}");
                    },
                    async right =>
                    {
                        await Task.Delay(1);
                        return Either<string, string>.Right($"Valid: {right}");
                    });
            
            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("Validation failed: Value too large", result.LeftValue);
        }

        [Fact(DisplayName = "Nested operations - Mix of sync and async with multiple binds")]
        public async Task NestedOperations_MixOfOperations_ShouldWorkCorrectly()
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
                .Bind(x => Either<string, (int original, int doubled)>.Right((42, x)))  // sync bind: create tuple
                .MapAsync(async tuple =>
                {
                    await Task.Delay(1);
                    return tuple.doubled / tuple.original;
                })  // async map: calculate ratio
                .Apply(
                    left => Either<string, string>.Left(left),
                    right => Either<string, string>.Right($"Ratio: {right.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}"));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Ratio: 2.0", result.RightValue);
        }
    }
}
