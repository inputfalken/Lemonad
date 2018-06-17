using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MapTests {
        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_False() {
            var result = ResultParsers.Int("foo").FullMap(i => i * 2, s => $"ERROR: {s}");

            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should not have a right value.");
            Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null__OkSelector__Expects_No__ArgumentNullExceptiion() {
            var exception = Record.Exception(() => {
                Func<string, string> errorselector = null;
                Func<int, int> leftSelector = null;
                var result = ResultParsers.Int("foo").FullMap(leftSelector, s => $"ERROR: {s}");

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should not have a right value.");
                Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(int), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorselector = null;
                var result = ResultParsers.Int("foo").FullMap(i => i * 2, errorselector);

                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have a right value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(4, result.Value);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector_And_Null_OkSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorselector = null;
                Func<int, int> leftselector = null;
                var result = ResultParsers.Int("foo").FullMap(leftselector, errorselector);

                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have a right value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(4, result.Value);
            });
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_False__Should_Not_Execute_OkSelector() {
            var isExectued = false;
            var result = ResultParsers.Int("foo").FullMap(i => {
                isExectued = true;
                return i * 2;
            }, s => $"ERROR: {s}");

            Assert.False(isExectued, "Okselector should not be exectued.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should not have a right value.");
            Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_True() {
            var result = ResultParsers.Int("2").FullMap(i => i * 2, s => $"ERROR: {s}");

            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.True(result.HasValue, "Result should have a right value.");
            Assert.Equal(default(string), result.Error);
            Assert.Equal(4, result.Value);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null__OkSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorselector = null;
                Func<int, int> leftSelector = null;
                var result = ResultParsers.Int("2").FullMap(leftSelector, s => $"ERROR: {s}");

                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have a right value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(4, result.Value);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector__Expects_No_ArgumentNullExceptiion() {
            var exception = Record.Exception(() => {
                Func<string, string> errorselector = null;
                var result = ResultParsers.Int("2").FullMap(i => i * 2, errorselector);

                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have a right value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(4, result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector_And_Null_OkSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorselector = null;
                Func<int, int> leftSelector = null;
                var result = ResultParsers.Int("2").FullMap(leftSelector, errorselector);

                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have a right value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(4, result.Value);
            });
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_True__Should_Not_Execute_errorSelector() {
            var isExectued = false;
            var result = ResultParsers.Int("2").FullMap( i => i * 2, s => {
                isExectued = true;
                return $"ERROR: {s}";
            });

            Assert.False(isExectued, "errorselector should not be exectued.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.True(result.HasValue, "Result should have a right value.");
            Assert.Equal(default(string), result.Error);
            Assert.Equal(4, result.Value);
        }
    }
}