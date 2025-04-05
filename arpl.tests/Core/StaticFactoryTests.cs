using Xunit;
using Arpl.Core;

namespace Arpl.Tests.Core
{
    public class StaticFactoryTests
    {
        [Fact]
        public void Success_CreatesSResultSuccess()
        {
            var result = StaticFactory.Success(10);
            Assert.True(result.IsSuccess);
            Assert.Equal(10, result.SuccessValue);
        }

        [Fact]
        public void Fail_CreatesSResultFail()
        {
            var error = Errors.New("fail");
            var result = StaticFactory.Fail<int>(error);
            Assert.True(result.IsFail);
            Assert.Equal(error, result.ErrorValue);
        }

        [Fact]
        public void Left_CreatesEitherLeft()
        {
            var either = StaticFactory.Left<string, int>("left");
            Assert.True(either.IsLeft);
            Assert.Equal("left", either.LeftValue);
        }

        [Fact]
        public void Right_CreatesEitherRight()
        {
            var either = StaticFactory.Right<string, int>(42);
            Assert.True(either.IsRight);
            Assert.Equal(42, either.RightValue);
        }
    }
}
