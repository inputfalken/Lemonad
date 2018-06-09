using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class MatchTests {
        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False() {
            var leftExecuted = false;
            var rightExectuted = false;
            var either = ErrorHandling.Either.Parse.Int("foo").Match(s => {
                leftExecuted = true;
                return s;
            }, i => {
                rightExectuted = true;
                return "Success";
            });
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", either);
            Assert.True(leftExecuted, "Left should be exectued");
            Assert.False(rightExectuted, "Right should not be exectued");
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector__Void__Expects_No_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Action<string> leftSelector = null;
                ErrorHandling.Either.Parse.Int("2").Match(leftSelector, i => { });
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_RightSelector__Void__Expects_Not_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<int> rightSelector = null;
                ErrorHandling.Either.Parse.Int("2").Match(s => {  }, rightSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector_And_Null_RightSelector__Void__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> leftSelector = null;
                Action<int> rightSelector = null;
                ErrorHandling.Either.Parse.Int("2")
                    .Match(leftSelector, rightSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector__Void__Expects_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> leftSelector = null;
                ErrorHandling.Either.Parse.Int("foo").Match(leftSelector, i => { });
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_RightSelector__Void__Expects_Not_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Action<int> rightSelector = null;
                ErrorHandling.Either.Parse.Int("foo").Match(s => { }, rightSelector);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector_And_Null_RightSelector__Void__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> leftSelector = null;
                Action<int> rightSelector = null;
                ErrorHandling.Either.Parse.Int("foo")
                    .Match(leftSelector, rightSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector__Expects_No_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Func<string, string> leftSelector = null;
                var either = ErrorHandling.Either.Parse.Int("2").Match(leftSelector, i => "Success" + i);
                Assert.Equal("Success2", either);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_RightSelector__Expects_Not_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, string> rightSelector = null;
                ErrorHandling.Either.Parse.Int("2").Match(s => { return s; }, rightSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector_And_Null_RightSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftSelector = null;
                Func<int, string> rightSelector = null;
                var either = ErrorHandling.Either.Parse.Int("2")
                    .Match(leftSelector, rightSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector__Expects_ArgumentNulLException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftSelector = null;
                var either = ErrorHandling.Either.Parse.Int("foo").Match(leftSelector, i => "Success");
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_RightSelector__Expects_Not_ArgumentNulLException() {
            var exception = Record.Exception(() => {
                Func<int, string> rightSelector = null;
                var leftExecuted = false;
                var either = ErrorHandling.Either.Parse.Int("foo").Match(s => {
                    leftExecuted = true;
                    return s;
                }, rightSelector);
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", either);
                Assert.True(leftExecuted, "Left should be exectued");
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector_And_Null_RightSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftSelector = null;
                Func<int, string> rightSelector = null;
                var either = ErrorHandling.Either.Parse.Int("foo")
                    .Match(leftSelector, rightSelector);
            });
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True() {
            var leftExecuted = false;
            var rightExectuted = false;
            var either = ErrorHandling.Either.Parse.Int("2").Match(s => {
                leftExecuted = true;
                return s;
            }, i => {
                rightExectuted = true;
                return "Success" + i;
            });
            Assert.Equal("Success2", either);
            Assert.False(leftExecuted, "Left should not be exectued");
            Assert.True(rightExectuted, "Right should be exectued");
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False__Void_Match() {
            var leftExecuted = false;
            var rightExectuted = false;
            ErrorHandling.Either.Parse.Int("foo").Match(s => { leftExecuted = true; }, i => { rightExectuted = true; });
            Assert.True(leftExecuted, "Left should be exectued");
            Assert.False(rightExectuted, "Right should not be exectued");
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True__Void_Match() {
            var leftExecuted = false;
            var rightExectuted = false;
            ErrorHandling.Either.Parse.Int("2").Match(s => { leftExecuted = true; }, i => { rightExectuted = true; });
            Assert.False(leftExecuted, "Left should not be exectued");
            Assert.True(rightExectuted, "Right should be exectued");
        }
    }
}