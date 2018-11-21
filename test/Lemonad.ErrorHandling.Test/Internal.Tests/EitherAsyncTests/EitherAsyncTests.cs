using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Internal.Either;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Internal.Tests.EitherAsyncTests {
    // TODO remove dependency to IResult from tests
    public class EitherAsyncTests {
        [Fact]
        public void Accessing_Error_Before_Await_Throws() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);
            Assert.Throws<InvalidEitherStateException>(() => eitherAsync.Error);
        }

        [Fact]
        public void Accessing_Value_Before_Await_Throws() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);
            Assert.Throws<InvalidEitherStateException>(() => eitherAsync.Value);
        }

        [Fact]
        public async Task Accessing_Error_After_Awaiting_HasValue_Does_Not_Throw() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            await eitherAsync.HasValue;
            var exception = Record.Exception(() => eitherAsync.Error);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Accessing_Value_Before_After_Awaiting_HasValue_Does_Not_Throw() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);
            await eitherAsync.HasValue;
            var exception = Record.Exception(() => eitherAsync.Value);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Accessing_Error_After_Awaiting_HasError_Does_Not_Throw() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            await eitherAsync.HasError;
            var exception = Record.Exception(() => eitherAsync.Error);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Accessing_Value_Before_After_Awaiting_HasError_Does_Not_Throw() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);
            await eitherAsync.HasError;
            var exception = Record.Exception(() => eitherAsync.Value);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Awaiting_HasValue() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            Assert.True(await eitherAsync.HasValue);

            Assert.Equal(20, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Awaiting_HasValue() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Error<int, string>("ERROR").Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            Assert.True(await eitherAsync.HasError);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal("ERROR", eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Awaiting_HasError() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            Assert.False(await eitherAsync.HasError);

            Assert.Equal(20, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Awaiting_HasError() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Error<int, string>("ERROR").Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            Assert.False(await eitherAsync.HasValue);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal("ERROR", eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Await_Both() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Value<int, string>(20).Either;
            });
            var eitherAsync = new AsyncEither<int, string>(either);

            Assert.True(await eitherAsync.HasValue);
            Assert.False(await eitherAsync.HasError);

            Assert.Equal(20, eitherAsync.Value);
            Assert.Equal(default, eitherAsync.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Await_Both() {
            var either = Task.Run(async () => {
                await Task.Delay(100);
                return ErrorHandling.Result.Error<int, string>("ERROR").Either;
            });

            var eitherAsync = new AsyncEither<int, string>(either);
            Assert.True(await eitherAsync.HasError);
            Assert.False(await eitherAsync.HasValue);

            Assert.Equal(default, eitherAsync.Value);
            Assert.Equal("ERROR", eitherAsync.Error);
        }
    }
}