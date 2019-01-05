using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FlatMapNullableAsyncTests {
        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName, () =>
                ErrorHandling.Result
                    .Value<int, string>(2)
                    .FlatMapAsync<int>(null, () => "ERROR")
            );

        [Fact]
        public void Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorSelectorName, () =>
                ErrorHandling.Result
                    .Value<int, string>(2)
                    .FlatMapAsync(async i => {
                        await AssertionUtilities.Delay;
                        return (int?) 2;
                    }, null)
            );

        [Fact]
        public void Passing_Null_Selector_ResultSelector_Overload_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName, () =>
                ErrorHandling.Result
                    .Value<int, string>(2)
                    .FlatMapAsync<int, string>(null, (i, i1) => "", () => "")
            );

        [Fact]
        public void Passing_Null_ResultSelector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ResultSelector, () =>
                ErrorHandling.Result
                    .Value<int, string>(2)
                    .FlatMapAsync<int, string>(async i => {
                        await AssertionUtilities.Delay;
                        return (int?) 2;
                    }, null, () => "")
            );

        [Fact]
        public void Passing_Null_ErrorSelector_ResultSelector_Overload_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorSelectorName, () =>
                ErrorHandling.Result
                    .Value<int, string>(2)
                    .FlatMapAsync(async i => {
                        await AssertionUtilities.Delay;
                        return (int?) 2;
                    }, (i, i1) => "", null)
            );

        [Fact]
        public async Task None_Null_Int__Expects_Result_With_Value() {
            var selectorInvoked = false;
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) 2;
                }, () => "ERROR")
                .AssertValue(2);

            Assert.True(selectorInvoked);
        }

        [Fact]
        public async Task None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) 2;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }, () => "ERROR")
                .AssertValue(4);

            Assert.True(selectorInvoked);
            Assert.True(resultSelectorInvoked);
        }

        [Fact]
        public async Task Null_Int__Expects_Result_With_Error() {
            var selectorInvoked = false;
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) null;
                }, () => "ERROR")
                .AssertError("ERROR");

            Assert.True(selectorInvoked);
        }

        [Fact]
        public async Task Null_Int_Using_ResultSelector__Expects_Result_With_Null() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            await ErrorHandling.Result
                .Value<int, string>(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) null;
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