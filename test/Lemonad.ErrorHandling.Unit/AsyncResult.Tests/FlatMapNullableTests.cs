using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FlatMapNullableTests {
        [Fact]
        public async Task None_Null_Int__Expects_Result_With_Value() {
            var number = Task.Run(async () => {
                await Task.Delay(200);
                int? nullable = 2;
                return nullable;
            });

            var selectorInvoked = false;
            var result = await ErrorHandling.Result.Value<int, string>(2).FlatMapAsync(_ => {
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
        public async Task None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var number = Task.Run(async () => {
                await Task.Delay(200);
                int? nullable = 2;
                return nullable;
            });
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            var result = await ErrorHandling.Result.Value<int, string>(2)
                .FlatMapAsync(_ => {
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
        public async Task Null_Int__Expects_Result_With_Value() {
            var number = Task.Run(async () => {
                await Task.Delay(200);
                int? nullable = null;
                return nullable;
            });
            var selectorInvoked = false;
            var result = await ErrorHandling.Result.Value<int, string>(2).FlatMapAsync(_ => {
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
        public async Task Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var number = Task.Run(async () => {
                await Task.Delay(200);
                int? nullable = null;
                return nullable;
            });
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            var result = await ErrorHandling.Result.Value<int, string>(2)
                .FlatMapAsync(_ => {
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