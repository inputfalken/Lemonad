using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MatchTests {
        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_False() {
            var errorExecuted = false;
            var rightExectuted = false;
            var either = ResultParsers.Int("foo").Match(i => {
                rightExectuted = true;
                return "Success";
            }, s => {
                errorExecuted = true;
                return s;
            });
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", either);
            Assert.True(errorExecuted, "error should be exectued");
            Assert.False(rightExectuted, "Ok should not be exectued");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector__Expects_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorSelector = null;
                var either = ResultParsers.Int("foo").Match(i => "Success", errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector__Void__Expects_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> errorSelector = null;
                ResultParsers.Int("foo").Match(i => { }, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector_And_Null_OkSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorSelector = null;
                Func<int, string> leftSelector = null;
                var either = ResultParsers.Int("foo")
                    .Match(leftSelector, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_errorSelector_And_Null_OkSelector__Void__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> errorSelector = null;
                Action<int> leftSelector = null;
                ResultParsers.Int("foo")
                    .Match(leftSelector, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_OkSelector__Expects_Not_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Func<int, string> leftSelector = null;
                var errorExecuted = false;
                var either = ResultParsers.Int("foo").Match(leftSelector, s => {
                    errorExecuted = true;
                    return s;
                });
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", either);
                Assert.True(errorExecuted, "error should be exectued");
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Null_OkSelector__Void__Expects_Not_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Action<int> leftSelector = null;
                ResultParsers.Int("foo").Match(leftSelector, s => { });
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_False__Void_Match() {
            var errorExecuted = false;
            var valueExecuted = false;
            ResultParsers.Int("foo").Match(x => { valueExecuted = true; }, x => { errorExecuted = true; });
            Assert.True(errorExecuted, "error should be exectued");
            Assert.False(valueExecuted, "Ok should not be exectued");
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_True() {
            var errorExecuted = false;
            var rightExectuted = false;
            var either = ResultParsers.Int("2").Match(i => {
                rightExectuted = true;
                return "Success" + i;
            }, s => {
                errorExecuted = true;
                return s;
            });
            Assert.Equal("Success2", either);
            Assert.False(errorExecuted, "error should not be exectued");
            Assert.True(rightExectuted, "Ok should be exectued");
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector__Expects_No_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Func<string, string> errorSelector = null;
                var either = ResultParsers.Int("2").Match(i => "Success" + i, errorSelector);
                Assert.Equal("Success2", either);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector__Void__Expects_No_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Action<string> errorSelector = null;
                ResultParsers.Int("2").Match(i => { }, errorSelector);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector_And_Null_OkSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> errorSelector = null;
                Func<int, string> leftSelector = null;
                var either = ResultParsers.Int("2")
                    .Match(leftSelector, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_errorSelector_And_Null_OkSelector__Void__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> errorSelector = null;
                Action<int> leftSelector = null;
                ResultParsers.Int("2")
                    .Match(leftSelector, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_OkSelector__Expects_Not_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, string> leftSelector = null;
                ResultParsers.Int("2").Match(leftSelector, s => s);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Null_OkSelector__Void__Expects_Not_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<int> leftSelector = null;
                ResultParsers.Int("2").Match(leftSelector, s => { });
            });
        }

        [Fact]
        public void Result_String_Int_Whose_Property_HasValue_Is_True__Void_Match() {
            var errorExecuted = false;
            var valueExecuted = false;
            ResultParsers.Int("2").Match(i => { valueExecuted = true; }, s => { errorExecuted = true; });
            Assert.False(errorExecuted, "error should not be exectued");
            Assert.True(valueExecuted, "Ok should be exectued");
        }
    }
}