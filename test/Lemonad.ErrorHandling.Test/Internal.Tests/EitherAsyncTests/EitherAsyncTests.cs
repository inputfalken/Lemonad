using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Internal.Tests.EitherAsyncTests {
    // TODO remove dependency to IResult from tests
    public class EitherAsyncTests {
        [Fact]
        public async Task Creating_Either_With_Value_After_Delay() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });

            var eitherAsync = new EitherAsync<int, string>(either);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);

            Assert.True(await eitherAsync.HasValue);

            Assert.Equal(20, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Error<int, string>("ERROR").Either;
            });

            var eitherAsync = new EitherAsync<int, string>(either);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);

            Assert.True(await eitherAsync.HasError);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal("ERROR", eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Verify_HasValue_And_HasError_Are_Not_The_Equal() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });

            var eitherAsync = new EitherAsync<int, string>(either);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);

            Assert.True(await eitherAsync.HasValue);
            Assert.False(await eitherAsync.HasError);

            Assert.Equal(20, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Verify_HasValue_And_HasError_Are_Not_The_Equal() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Error<int, string>("ERROR").Either;
            });

            var eitherAsync = new EitherAsync<int, string>(either);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);

            Assert.True(await eitherAsync.HasError);
            Assert.False(await eitherAsync.HasValue);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal("ERROR", eitherAsync.Error);
        }
    }
}