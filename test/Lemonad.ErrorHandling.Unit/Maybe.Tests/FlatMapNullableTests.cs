using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlatMapNullableTests {
        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => ErrorHandling.Maybe.Value(2).FlatMap((Func<int, int?>) null)
            );

        [Fact]
        public void None_Null_Int__Expects_Result_With_Value() {
            var selectorInvoked = false;
            ErrorHandling.Maybe
                .Value(2)
                .FlatMap(i => {
                    selectorInvoked = true;
                    return (int?) 2;
                })
                .AssertValue(2);

            Assert.True(selectorInvoked);
        }

        [Fact]
        public void None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            ErrorHandling.Maybe
                .Value(2)
                .FlatMap(_ => {
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
        public void Null_Int__Expects_Result_With_Value() {
            var selectorInvoked = false;
            ErrorHandling.Maybe.Value(2)
                .FlatMap(_ => {
                    selectorInvoked = true;
                    return (int?) null;
                })
                .AssertNone();

            Assert.True(selectorInvoked);
        }

        [Fact]
        public void Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            ErrorHandling.Maybe
                .Value(2)
                .FlatMap(_ => {
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
    }
}