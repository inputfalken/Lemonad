using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class FlatMapNullableAsyncTests {
        [Fact]
        public async Task None_Null_Int__Expects_Result_With_Value() {
            var selectorInvoked = false;
            await ErrorHandling.AsyncMaybe
                .Value(2)
                .FlatMapAsync(async i => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) 2;
                })
                .AssertValue(2);

            Assert.True(selectorInvoked);
        }

        [Fact]
        public async Task None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            await ErrorHandling.AsyncMaybe
                .Value(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) 2;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                }).AssertValue(4);

            Assert.True(selectorInvoked);
            Assert.True(resultSelectorInvoked);
        }

        [Fact]
        public async Task Null_Int__Expects_Result_With_Value() {
            var selectorInvoked = false;
            await ErrorHandling.AsyncMaybe.Value(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) null;
                })
                .AssertNone();

            Assert.True(selectorInvoked);
        }

        [Fact]
        public async Task Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            await ErrorHandling.AsyncMaybe
                .Value(2)
                .FlatMapAsync(async _ => {
                    await AssertionUtilities.Delay;
                    selectorInvoked = true;
                    return (int?) null;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return x + y;
                })
                .AssertNone();

            Assert.True(selectorInvoked);
            Assert.False(resultSelectorInvoked);
        }

        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => ErrorHandling.Maybe.Value(2).FlatMapAsync((Func<int, Task<int?>>) null)
            );
    }
}