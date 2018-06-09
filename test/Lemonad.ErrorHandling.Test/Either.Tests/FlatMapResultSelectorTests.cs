using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class FlatMapResultSelectorTests {
        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True_With_Null_Selector__Expects_ArgumentNullException_Thrown() {
            Func<int, Either<string, int>> selector = null;
            Assert.Throws<ArgumentNullException>(() => {
                var result = ErrorHandling.Either.Parse.Int("2").FlatMap(selector, (x, y) => x + y);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True_With_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, int, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ErrorHandling.Either.Parse.Int("2")
                    .FlatMap(i => ErrorHandling.Either.Parse.Int("foo"), resultSelector);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True_With_Null_Selector_And_Null_ResultSelector__Expects_ArgumentNullException_Thrown() {
            Func<int, Either<string, int>> selector = null;
            Func<int, int, int> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => {
                var result = ErrorHandling.Either.Parse.Int("2").FlatMap(selector, resultSelector);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Selector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, Either<string, int>> selector = null;
            var exception = Record.Exception(() => {
                var result = ErrorHandling.Either.Parse.Int("foo").FlatMap(selector, (x, y) => x + y);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, bool, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ErrorHandling.Either.Parse.Int("foo")
                    .FlatMap(i => ErrorHandling.Either.Parse.Bool("x"), resultSelector);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Selector_And_Null_ResultSelector__Expects_No_ArgumentNullException_Thrown() {
            Func<int, Either<string, int>> selector = null;
            Func<int, int, int> resultSelector = null;
            var exception = Record.Exception(() => {
                var result = ErrorHandling.Either.Parse.Int("foo").FlatMap(selector, resultSelector);
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_True__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("2");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4.2, result.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_True__Expects_String_Double_Whose_Property_IsRight_Is_False() {
            var intParse = ErrorHandling.Either.Parse.Int("foo");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.IsRight, "Either should have not a right value.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_False__Expects_String_Double_Whose_Property_IsRight_Is_False() {
            var intParse = ErrorHandling.Either.Parse.Int("foo");
            var doubleParse = ErrorHandling.Either.Parse.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.IsRight, "Either should have not a right value.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_False__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("2");
            var doubleParse = ErrorHandling.Either.Parse.Double("foo");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.False(result.IsRight, "Either should have not a right value.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Double.", result.Left);
            Assert.Equal(default(double), result.Right);
        }
    }
}