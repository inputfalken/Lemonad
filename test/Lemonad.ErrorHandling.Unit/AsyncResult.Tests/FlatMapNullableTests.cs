using System.Threading.Tasks;
using Assertion;
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
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(_ => {
                    selectorInvoked = true;
                    return number;
                }, () => "ERROR")
                .AssertValue(2);

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
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(_ => {
                    selectorInvoked = true;
                    return number;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }, () => "ERROR")
                .AssertValue(4);

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
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(_ => {
                    selectorInvoked = true;
                    return number;
                }, () => "ERROR")
                .AssertError("ERROR");

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
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(_ => {
                    selectorInvoked = true;
                    return number;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }, () => "ERROR")
                .AssertError("ERROR");

            Assert.True(selectorInvoked);
            Assert.False(resultSelectorInvoked);
        }
    }
}