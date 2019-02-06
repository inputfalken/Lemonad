using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Internal.Either;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Internal.Tests.EitherAsyncTests {
    public class EitherAsyncTests {
        [Fact]
        public async Task Accessing_Error_After_Awaiting_HasError_Does_Not_Throw() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );

            await either.HasError;
            var exception = Record.Exception(() => either.Error);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Accessing_Error_After_Awaiting_HasValue_Does_Not_Throw() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );

            await either.HasValue;
            var exception = Record.Exception(() => either.Error);
            Assert.Null(exception);
        }

        [Fact]
        public void Accessing_Error_Before_Await_Throws() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );
            Assert.Throws<InvalidEitherStateException>(() => either.Error);
        }

        [Fact]
        public async Task Accessing_Value_Before_After_Awaiting_HasError_Does_Not_Throw() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );
            await either.HasError;
            var exception = Record.Exception(() => either.Value);
            Assert.Null(exception);
        }

        [Fact]
        public async Task Accessing_Value_Before_After_Awaiting_HasValue_Does_Not_Throw() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );
            await either.HasValue;
            var exception = Record.Exception(() => either.Value);
            Assert.Null(exception);
        }

        [Fact]
        public void Accessing_Value_Before_Await_Throws() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );
            Assert.Throws<InvalidEitherStateException>(() => either.Value);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Await_Both() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateError<int, string>("ERROR");
                })
            );
            Assert.True(await either.HasError);
            Assert.False(await either.HasValue);

            Assert.Equal(default, either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Awaiting_HasError() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateError<int, string>("ERROR");
                })
            );

            Assert.False(await either.HasValue);
            Assert.Equal(default, either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Error_After_Delay_Awaiting_HasValue() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateError<int, string>("ERROR");
                })
            );

            Assert.True(await either.HasError);

            Assert.Equal(default, either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Await_Both() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );

            Assert.True(await either.HasValue);
            Assert.False(await either.HasError);

            Assert.Equal(20, either.Value);
            Assert.Equal(default, either.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Awaiting_HasError() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );
            Assert.False(await either.HasError);
            Assert.Equal(20, either.Value);
            Assert.Equal(default, either.Error);
        }

        [Fact]
        public async Task Creating_Either_With_Value_After_Delay_Awaiting_HasValue() {
            var either = new AsyncEither<int, string>(
                Task.Run(async () => {
                    await Task.Delay(100);
                    return EitherMethods.CreateValue<int, string>(20);
                })
            );

            Assert.True(await either.HasValue);
            Assert.Equal(20, either.Value);
            Assert.Equal(default, either.Error);
        }
    }
}