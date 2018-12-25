using System;
using System.Collections.Generic;
using System.Text;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests
{
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
    }
}
