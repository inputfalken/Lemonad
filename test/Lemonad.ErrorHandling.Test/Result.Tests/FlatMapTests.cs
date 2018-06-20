using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class FlatMapTests {
        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Selector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, Result<int, string>> selector = null;
            var exception = Record.Exception(() => {
                var result = ResultParsers.Int("foo").FlatMap(selector, x => x);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True_With_Null_Selector__Expects_ArgumentNullException_Thrown() {
            Func<int, Result<int, string>> selector = null;
            Assert.Throws<ArgumentNullException>(() => ResultParsers.Int("2").FlatMap(selector, x => x));
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_Selector_To_Never_Be_Invoked() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("foo");

            var isInvoked = false;
            var result = intParse.FlatMap(_ => {
                isInvoked = true;
                return doubleParse;
            }, x => x);

            Assert.False(isInvoked, "Selector should never have been invoked.");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_String_Double_Whose_Property_HasValue_Is_False() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, s => s);
            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_Selector_To_Never_Be_Invoked() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("2");

            var isInvoked = false;
            var result = intParse.FlatMap(_ => {
                isInvoked = true;
                return doubleParse;
            }, s => s);

            Assert.False(isInvoked, "Selector should never have been invoked.");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_String_Double_Whose_Property_HasValue_Is_False() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("2");

            var result = intParse.FlatMap(x => doubleParse, s => s);
            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_Selector_To_Be_Invoked() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("2");

            var isInvoked = false;
            var result = intParse.FlatMap(_ => {
                isInvoked = true;
                return doubleParse;
            }, s => s);

            Assert.True(isInvoked, "Selector should be invoked.");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_String_Double_Whose_Property_HasValue_Is_False() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, s => s);
            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Double.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_Selector_To_Be_Invoked() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("2");

            var isInvoked = false;
            var result = intParse.FlatMap(_ => {
                isInvoked = true;
                return doubleParse;
            }, s => s);

            Assert.True(isInvoked, "Selector should be invoked.");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_String_Double_Whose_Property_HasValue_Is_True() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, x => x);
            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.Equal(default(string), result.Error);
            Assert.Equal(2.2, result.Value);
        }
    }
}