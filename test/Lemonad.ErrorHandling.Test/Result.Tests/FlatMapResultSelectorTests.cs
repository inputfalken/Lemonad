using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class FlatMapResultSelectorTests {
        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, bool, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ResultParsers.Int("foo")
                    .FlatMap(i => ResultParsers.Bool("x"), resultSelector);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Selector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, Result<int, string>> selector = null;
            var exception = Record.Exception(() => {
                var result = ResultParsers.Int("foo").FlatMap(selector, (x, y) => x + y);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Selector_And_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, Result<int, string>> selector = null;
            Func<int, int, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ResultParsers.Int("foo").FlatMap(selector, resultSelector);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True_With_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, int, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ResultParsers.Int("2")
                    .FlatMap(i => ResultParsers.Int("foo"), resultSelector);
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
            Assert.Throws<ArgumentNullException>(() => {
                var result = ResultParsers.Int("2").FlatMap(selector, (x, y) => x + y);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True_With_Null_Selector_And_Null_ResultSelector__Expects_ArgumentNullException_Thrown() {
            Func<int, Result< int, string>> selector = null;
            Func<int, int, int> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => {
                var result = ResultParsers.Int("2").FlatMap(selector, resultSelector);
                Assert.False(result.HasValue, "Result should have error.");
                Assert.True(result.HasError, "Result should have a error value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_String_Double_Whose_Property_HasValue_Is_False() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.HasValue, "Result should have have error value.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_String_Double_Whose_Property_HasValue_Is_False() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.HasValue, "Result should have have error value.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_False__Expects_String_Double_Whose_Property_HasValue_Is_True() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.HasValue, "Result should have have error value.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Double.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__FLatmaps_Result_String_Double_Whose_Property_HasValue_Is_True__Expects_String_Double_Whose_Property_HasValue_Is_True() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.Equal(default(string), result.Error);
            Assert.Equal(4.2, result.Value);
        }
    }
}
