using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class EitherExtensionsTests
    {
        [Fact(DisplayName = "Sequence - When all are Right - Returns Right with all values")]
        public void Sequence_WhenAllRight_ReturnsRightWithAllValues()
        {
            // Arrange
            var items = new[]
            {
                Either<string, int>.Right(1),
                Either<string, int>.Right(2),
                Either<string, int>.Right(3)
            };

            // Act
            var result = items.Sequence();

            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { 1, 2, 3 }, result.RightValue);
        }

        [Fact(DisplayName = "Sequence - When any is Left - Returns first Left")]
        public void Sequence_WhenAnyLeft_ReturnsFirstLeft()
        {
            // Arrange
            var items = new[]
            {
                Either<string, int>.Right(1),
                Either<string, int>.Left("error"),
                Either<string, int>.Right(3)
            };

            // Act
            var result = items.Sequence();

            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("error", result.LeftValue);
        }

        [Fact(DisplayName = "SequenceAsync - When all are Right - Returns Right with all values")]
        public async Task SequenceAsync_WhenAllRight_ReturnsRightWithAllValues()
        {
            // Arrange
            var items = new[]
            {
                Task.FromResult(Either<string, int>.Right(1)),
                Task.FromResult(Either<string, int>.Right(2)),
                Task.FromResult(Either<string, int>.Right(3))
            };

            // Act
            var result = await items.SequenceAsync();

            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { 1, 2, 3 }, result.RightValue);
        }

        [Fact(DisplayName = "SequenceAsync - When any is Left - Returns first Left")]
        public async Task SequenceAsync_WhenAnyLeft_ReturnsFirstLeft()
        {
            // Arrange
            var items = new[]
            {
                Task.FromResult(Either<string, int>.Right(1)),
                Task.FromResult(Either<string, int>.Left("error")),
                Task.FromResult(Either<string, int>.Right(3))
            };

            // Act
            var result = await items.SequenceAsync();

            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("error", result.LeftValue);
        }

        [Fact(DisplayName = "Traverse - When all mappings are Right - Returns Right with all values")]
        public void Traverse_WhenAllRight_ReturnsRightWithAllValues()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = items.Traverse(x => Either<string, string>.Right($"Value: {x}"));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { "Value: 1", "Value: 2", "Value: 3" }, result.RightValue);
        }

        [Fact(DisplayName = "Traverse - When any mapping is Left - Returns first Left")]
        public void Traverse_WhenAnyLeft_ReturnsFirstLeft()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = items.Traverse(x => x == 2 
                ? Either<string, string>.Left("Error at 2")
                : Either<string, string>.Right($"Value: {x}"));
            
            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("Error at 2", result.LeftValue);
        }

        [Fact(DisplayName = "TraverseAsync - When all mappings are Right - Returns Right with all values")]
        public async Task TraverseAsync_WhenAllRight_ReturnsRightWithAllValues()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = await items.TraverseAsync(x => 
                Task.FromResult(Either<string, string>.Right($"Value: {x}")));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { "Value: 1", "Value: 2", "Value: 3" }, result.RightValue);
        }

        [Fact(DisplayName = "TraverseAsync - When any mapping is Left - Returns first Left")]
        public async Task TraverseAsync_WhenAnyLeft_ReturnsFirstLeft()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            
            // Act
            var result = await items.TraverseAsync(x => x == 2
                ? Task.FromResult(Either<string, string>.Left("Error at 2"))
                : Task.FromResult(Either<string, string>.Right($"Value: {x}")));
            
            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("Error at 2", result.LeftValue);
        }

        [Fact(DisplayName = "SequenceAsync - Should allow chaining without intermediate awaits")]
        public async Task SequenceAsync_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.Select(x => Either<string, int>.Right(x)));
            
            // Act
            var result = await task
                .SequenceAsync()
                .MapAsync(xs => Task.FromResult(xs.Select(x => x * 2)))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.RightValue);
        }

        [Fact(DisplayName = "TraverseAsync - Should allow chaining without intermediate awaits")]
        public async Task TraverseAsync_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.AsEnumerable());
            
            // Act
            var result = await task
                .TraverseAsync(x => Either<string, int>.Right(x * 2))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.RightValue);
        }

        [Fact(DisplayName = "TraverseAsync - Should allow chaining with async mapping without intermediate awaits")]
        public async Task TraverseAsync_WithAsyncMapping_ShouldAllowChainingWithoutAwaits()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            var task = Task.FromResult(items.AsEnumerable());
            
            // Act
            var result = await task
                .TraverseAsync(x => Task.FromResult(Either<string, int>.Right(x * 2)))
                .MapAsync(xs => Task.FromResult(xs.Select(x => $"Value: {x}")));
            
            // Assert
            Assert.True(result.IsRight);
            Assert.Equal(new[] { "Value: 2", "Value: 4", "Value: 6" }, result.RightValue);
        }

        [Fact(DisplayName = "MatchAsync - With sync functions - Should handle Right value")]
        public async Task MatchAsync_WithSyncFunctions_ShouldHandleRight()
        {
            // Arrange
            var task = Task.FromResult(Either<string, int>.Right(42));

            // Act
            var result = await task.MatchAsync(
                left => Either<string, string>.Left(left),
                right => Either<string, string>.Right($"Value: {right}"));

            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Value: 42", result.RightValue);
        }

        [Fact(DisplayName = "MatchAsync - With sync functions - Should handle Left value")]
        public async Task MatchAsync_WithSyncFunctions_ShouldHandleLeft()
        {
            // Arrange
            var task = Task.FromResult(Either<string, int>.Left("error"));

            // Act
            var result = await task.MatchAsync(
                left => Either<string, string>.Left($"Error: {left}"),
                right => Either<string, string>.Right($"Value: {right}"));

            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("Error: error", result.LeftValue);
        }

        [Fact(DisplayName = "MatchAsync - With async functions - Should handle Right value")]
        public async Task MatchAsync_WithAsyncFunctions_ShouldHandleRight()
        {
            // Arrange
            var task = Task.FromResult(Either<string, int>.Right(42));

            // Act
            var result = await task.MatchAsync(
                left => Task.FromResult(Either<string, string>.Left(left)),
                right => Task.FromResult(Either<string, string>.Right($"Value: {right}")));

            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("Value: 42", result.RightValue);
        }

        [Fact(DisplayName = "MatchAsync - With async functions - Should handle Left value")]
        public async Task MatchAsync_WithAsyncFunctions_ShouldHandleLeft()
        {
            // Arrange
            var task = Task.FromResult(Either<string, int>.Left("error"));

            // Act
            var result = await task.MatchAsync(
                left => Task.FromResult(Either<string, string>.Left($"Error: {left}")),
                right => Task.FromResult(Either<string, string>.Right($"Value: {right}")));

            // Assert
            Assert.True(result.IsLeft);
            Assert.Equal("Error: error", result.LeftValue);
        }

        [Fact(DisplayName = "MatchAsync - Should allow chaining with other async operations")]
        public async Task MatchAsync_ShouldAllowChaining()
        {
            // Arrange
            var task = Task.FromResult(Either<string, int>.Right(42));

            // Act
            var result = await task
                .MatchAsync(
                    left => Task.FromResult(Either<string, int>.Left(left)),
                    right => Task.FromResult(Either<string, int>.Right(right * 2)))
                .BindAsync(value => Task.FromResult(Either<string, string>.Right($"Value: {value}")))
                .MapAsync(str => Task.FromResult(str.ToUpper()));

            // Assert
            Assert.True(result.IsRight);
            Assert.Equal("VALUE: 84", result.RightValue);
        }
    }
}
