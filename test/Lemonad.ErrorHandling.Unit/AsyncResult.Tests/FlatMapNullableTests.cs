using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FlatMapNullableTests {
        [Fact]
        public void None_Null_Int__Expects_Result_With_Value() {
            int? number = 2;
            var selectorInvoked = false;
            ErrorHandling.AsyncResult
                .Value<int, string>(2)
                .FlatMap(_ => {
                    selectorInvoked = true;
                    return number;
                }, () => "ERROR")
                .AssertValue(2);

            Assert.True(selectorInvoked);
        }

        [Fact]
        public void None_Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            int? number = 2;
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            ErrorHandling.AsyncResult
                .Value<int, string>(2)
                .FlatMap(_ => {
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
        public void Null_Int__Expects_Result_With_Value() {
            int? number = null;
            var selectorInvoked = false;
            ErrorHandling.AsyncResult.Value<int, string>(2)
                .FlatMap(_ => {
                    selectorInvoked = true;
                    return number;
                }, () => "ERROR")
                .AssertError("ERROR");

            Assert.True(selectorInvoked);
        }

        [Fact]
        public void Null_Int_Using_ResultSelector__Expects_Result_With_Value() {
            int? number = null;
            var selectorInvoked = false;
            var resultSelectorInvoked = false;
            ErrorHandling.AsyncResult
                .Value<int, string>(2)
                .FlatMap(_ => {
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

        [Fact]
        public void Passing_Null_ErrorSelector_ResultSelector_Overload_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorSelectorName, () =>
                ErrorHandling.AsyncResult
                    .Value<int, string>(2)
                    .FlatMap(i => (int?) 2, (i, i1) => "", null)
            );

        [Fact]
        public void Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorSelectorName, () =>
                ErrorHandling.AsyncResult
                    .Value<int, string>(2)
                    .FlatMap(i => (int?) 2, null)
            );

        [Fact]
        public void Passing_Null_ResultSelector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ResultSelector, () =>
                ErrorHandling.AsyncResult
                    .Value<int, string>(2)
                    .FlatMap<int, string>(i => (int?) 2, null, () => "")
            );

        [Fact]
        public void Passing_Null_Selector_ResultSelector_Overload_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName, () =>
                ErrorHandling.AsyncResult
                    .Value<int, string>(2)
                    .FlatMap<int, string>(null, (i, i1) => "", () => "")
            );

        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName, () =>
                ErrorHandling.AsyncResult
                    .Value<int, string>(2)
                    .FlatMap<int>(null, () => "ERROR")
            );
    }
}