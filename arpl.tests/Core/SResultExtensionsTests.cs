using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class SResultExtensionsTests
    {
        [Fact]
        public async Task Do_OnTaskOfSResult_ExecutesFunction()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));
            var executed = false;

            // Act
            var result = await task.Do(r => {
                executed = true;
                Assert.True(r.IsSuccess);
                Assert.Equal(42, r.SuccessValue);
                return r;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.SuccessValue);
        }

        [Fact]
        public async Task DoAsync_OnTaskOfSResult_ExecutesFunction()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));
            var executed = false;

            // Act
            var result = await task.DoAsync(async r => {
                executed = true;
                Assert.True(r.IsSuccess);
                Assert.Equal(42, r.SuccessValue);
                await Task.Delay(1); // Simulate async work
                return r;
            });

            // Assert
            Assert.True(executed);
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.SuccessValue);
        }

        [Fact]
        public async Task Transform_OnTaskOfSResult_TransformsToString()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));

            // Act
            var result = await task.Transform(r => r.IsSuccess ? $"Success({r.SuccessValue})" : $"Error({r.ErrorValue.Message})");

            // Assert
            Assert.Equal("Success(42)", result);
        }

        [Fact]
        public async Task TransformAsync_OnTaskOfSResult_TransformsToString()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));

            // Act
            var result = await task.TransformAsync(async r => {
                await Task.Delay(1); // Simulate async work
                return r.IsSuccess ? $"Success({r.SuccessValue})" : $"Error({r.ErrorValue.Message})";
            });

            // Assert
            Assert.Equal("Success(42)", result);
        }

        [Fact]
        public async Task Transform_ChainedWithMap_ExecutesInOrder()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));
            var order = new List<string>();

            // Act
            var result = await task
                .Do(r => {
                    order.Add("First");
                    return r;
                })
                .Map(x => {
                    order.Add("Second");
                    return x * 2;
                })
                .Transform(r => {
                    order.Add("Third");
                    return r.SuccessValue;
                });

            // Assert
            Assert.Equal(new[] { "First", "Second", "Third" }, order);
            Assert.Equal(84, result);
        }

        [Fact(DisplayName = "Sequence - When all are Success - Returns Success with all values")]
        public void Sequence_WhenAllSuccess_ReturnsSuccessWithAllValues()
        {
            // Arrange
            var items = new[]
            {
                SResult<int>.Success(1),
                SResult<int>.Success(2),
                SResult<int>.Success(3)
            };

            // Act
            var result = items.Sequence();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { 1, 2, 3 }, result.SuccessValue);
        }

        [Fact(DisplayName = "Sequence - When any is Error - Returns first Error")]
        public void Sequence_WhenAnyError_ReturnsFirstError()
        {
            // Arrange
            var error = Errors.New("test error");
            var items = new[]
            {
                SResult<int>.Success(1),
                SResult<int>.Error(error),
                SResult<int>.Success(3)
            };

            // Act
            var result = items.Sequence();

            // Assert
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }

        [Fact(DisplayName = "SequenceAsync - When all are Success - Returns Success with all values")]
        public async Task SequenceAsync_WhenAllSuccess_ReturnsSuccessWithAllValues()
        {
            // Arrange
            var items = new[]
            {
                Task.FromResult(SResult<int>.Success(1)),
                Task.FromResult(SResult<int>.Success(2)),
                Task.FromResult(SResult<int>.Success(3))
            };

            // Act
            var result = await items.SequenceAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { 1, 2, 3 }, result.SuccessValue);
        }

        [Fact(DisplayName = "SequenceAsync - When any is Error - Returns first Error")]
        public async Task SequenceAsync_WhenAnyError_ReturnsFirstError()
        {
            // Arrange
            var error = Errors.New("test error");
            var items = new[]
            {
                Task.FromResult(SResult<int>.Success(1)),
                Task.FromResult(SResult<int>.Error(error)),
                Task.FromResult(SResult<int>.Success(3))
            };

            // Act
            var result = await items.SequenceAsync();

            // Assert
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }

        [Fact(DisplayName = "Traverse - When all mappings are Success - Returns Success with all values")]
        public void Traverse_WhenAllSuccess_ReturnsSuccessWithAllValues()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = items.Traverse(x => SResult<string>.Success($"Value: {x}"));
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "Value: 1", "Value: 2", "Value: 3" }, result.SuccessValue);
        }

        [Fact(DisplayName = "Traverse - When any mapping is Error - Returns first Error")]
        public void Traverse_WhenAnyError_ReturnsFirstError()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var error = Errors.New("Error at 2");
            
            // Act
            var result = items.Traverse(x => x == 2 
                ? SResult<string>.Error(error)
                : SResult<string>.Success($"Value: {x}"));
            
            // Assert
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }

        [Fact(DisplayName = "TraverseAsync - When all mappings are Success - Returns Success with all values")]
        public async Task TraverseAsync_WhenAllSuccess_ReturnsSuccessWithAllValues()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = await items.TraverseAsync(x => 
                Task.FromResult(SResult<string>.Success($"Value: {x}")));
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "Value: 1", "Value: 2", "Value: 3" }, result.SuccessValue);
        }

        [Fact(DisplayName = "TraverseAsync - When any mapping is Error - Returns first Error")]
        public async Task TraverseAsync_WhenAnyError_ReturnsFirstError()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var error = Errors.New("Error at 2");
            
            // Act
            var result = await items.TraverseAsync(x => x == 2
                ? Task.FromResult(SResult<string>.Error(error))
                : Task.FromResult(SResult<string>.Success($"Value: {x}")));
            
            // Assert
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }

        [Fact(DisplayName = "SequenceAsync - Should allow chaining without intermediate awaits")]
        public async Task SequenceAsync_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.Select(x => SResult<int>.Success(x)));
            
            // Act
            var result = await task
                .SequenceAsync()
                .MapAsync(xs => Task.FromResult(xs.Select(x => x * 2)))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.SuccessValue);
        }

        [Fact(DisplayName = "TraverseAsync - Should allow chaining without intermediate awaits")]
        public async Task TraverseAsync_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.AsEnumerable());
            
            // Act
            var result = await task
                .TraverseAsync(x => SResult<int>.Success(x * 2))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.SuccessValue);
        }

        [Fact(DisplayName = "TraverseAsync - Should allow chaining with async mapping without intermediate awaits")]
        public async Task TraverseAsync_WithAsyncMapping_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.AsEnumerable());
            
            // Act
            var result = await task
                .TraverseAsync(x => Task.FromResult(SResult<int>.Success(x * 2)))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.SuccessValue);
        }

        [Fact(DisplayName = "MatchAsync - With sync functions - Should handle Success value")]
        public async Task MatchAsync_WithSyncFunctions_ShouldHandleSuccess()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));

            // Act
            var result = await task.Match(
                fail => SResult<string>.Error(fail),
                success => SResult<string>.Success($"Value: {success}"));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Value: 42", result.SuccessValue);
        }

        [Fact(DisplayName = "MatchAsync - With sync functions - Should handle Error value")]
        public async Task MatchAsync_WithSyncFunctions_ShouldHandleError()
        {
            // Arrange
            var error = Errors.New("test error");
            var task = Task.FromResult(SResult<int>.Error(error));

            // Act
            var result = await task.Match(
                fail => SResult<string>.Error(Errors.New($"Error: {fail.Message}")),
                success => SResult<string>.Success($"Value: {success}"));

            // Assert
            Assert.True(result.IsFail);
            Assert.Equal("Error: test error", result.ErrorValue.Message);
        }

        [Fact(DisplayName = "MatchAsync - With async functions - Should handle Success value")]
        public async Task MatchAsync_WithAsyncFunctions_ShouldHandleSuccess()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));

            // Act
            var result = await task.MatchAsync(
                fail => Task.FromResult(SResult<string>.Error(fail)),
                success => Task.FromResult(SResult<string>.Success($"Value: {success}")));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Value: 42", result.SuccessValue);
        }

        [Fact(DisplayName = "MatchAsync - With async functions - Should handle Error value")]
        public async Task MatchAsync_WithAsyncFunctions_ShouldHandleError()
        {
            // Arrange
            var error = Errors.New("test error");
            var task = Task.FromResult(SResult<int>.Error(error));

            // Act
            var result = await task.MatchAsync(
                fail => Task.FromResult(SResult<string>.Error(Errors.New($"Error: {fail.Message}"))),
                success => Task.FromResult(SResult<string>.Success($"Value: {success}")));

            // Assert
            Assert.True(result.IsFail);
            Assert.Equal("Error: test error", result.ErrorValue.Message);
        }

        [Fact(DisplayName = "MatchAsync - Should allow chaining with other async operations")]
        public async Task MatchAsync_ShouldAllowChaining()
        {
            // Arrange
            var task = Task.FromResult(SResult<int>.Success(42));

            // Act
            var result = await task
                .MatchAsync(
                    fail => Task.FromResult(SResult<int>.Error(fail)),
                    success => Task.FromResult(SResult<int>.Success(success * 2)))
                .BindAsync(value => Task.FromResult(SResult<string>.Success($"Value: {value}")))
                .MapAsync(str => Task.FromResult(str.ToUpper()));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("VALUE: 84", result.SuccessValue);
        }
    }
}
