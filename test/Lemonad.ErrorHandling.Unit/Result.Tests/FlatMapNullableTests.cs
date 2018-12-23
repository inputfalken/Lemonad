using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FlatMapNullableTests {
        [Fact]
        public void None_Null_Int__Expects_Result_With_Value() {
            int? number = 2;
            var selectorInvoked = false;
            var result = ErrorHandling.Result.Value<int, string>(2).FlatMap(_ => {
                selectorInvoked = true;
                return number;
            }, () => "ERROR");

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(2, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.True(selectorInvoked);
        }

        [Fact]
        public void None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            int? number = 2;
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            var result = ErrorHandling.Result.Value<int, string>(2)
                .FlatMap(_ => {
                    selectorInvoked = true;
                    return number;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }, () => "ERROR");

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(4, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.True(selectorInvoked);
            Assert.True(resultSelectorInvoked);
        }

        [Fact]
        public void Null_Int__Expects_Result_With_Value() {
            int? number = null;
            var selectorInvoked = false;
            var result = ErrorHandling.Result.Value<int, string>(2).FlatMap(_ => {
                selectorInvoked = true;
                return number;
            }, () => "ERROR");

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.True(selectorInvoked);
        }

        [Fact]
        public void Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            int? number = null;
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            var result = ErrorHandling.Result.Value<int, string>(2)
                .FlatMap(_ => {
                    selectorInvoked = true;
                    return number;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }, () => "ERROR");

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.True(selectorInvoked);
            Assert.False(resultSelectorInvoked);
        }
    }
}