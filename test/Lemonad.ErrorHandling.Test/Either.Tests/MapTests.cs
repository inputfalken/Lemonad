using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class MapTests {
        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False() {
            var result = EitherParsers.Int("foo").Map(s => $"ERROR: {s}", i => i * 2);

            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(int), result.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null__RightSelector__Expects_No__ArgumentNullExceptiion() {
            var exception = Record.Exception(() => {
                Func<string, string> leftselector = null;
                Func<int, int> rightSelector = null;
                var result = EitherParsers.Int("foo").Map(s => $"ERROR: {s}", rightSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(int), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftselector = null;
                var result = EitherParsers.Int("foo").Map(leftselector, i => i * 2);

                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(4, result.Right);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_LeftSelector_And_Null_RightSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftselector = null;
                Func<int, int> rightSelector = null;
                var result = EitherParsers.Int("foo").Map(leftselector, rightSelector);

                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(4, result.Right);
            });
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False__Should_Not_Execute_RightSelector() {
            var isExectued = false;
            var result = EitherParsers.Int("foo").Map(s => $"ERROR: {s}", i => {
                isExectued = true;
                return i * 2;
            });

            Assert.False(isExectued, "Rightselector should not be exectued.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("ERROR: Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(int), result.Right);
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True() {
            var result = EitherParsers.Int("2").Map(s => $"ERROR: {s}", i => i * 2);

            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4, result.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null__RightSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftselector = null;
                Func<int, int> rightSelector = null;
                var result = EitherParsers.Int("2").Map(s => $"ERROR: {s}", rightSelector);

                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(4, result.Right);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector__Expects_No_ArgumentNullExceptiion() {
            var exception = Record.Exception(() => {
                Func<string, string> leftselector = null;
                var result = EitherParsers.Int("2").Map(leftselector, i => i * 2);

                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(4, result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_LeftSelector_And_Null_RightSelector__Expects_ArgumentNullExceptiion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> leftselector = null;
                Func<int, int> rightSelector = null;
                var result = EitherParsers.Int("2").Map(leftselector, rightSelector);

                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(4, result.Right);
            });
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True__Should_Not_Execute_LeftSelector() {
            var isExectued = false;
            var result = EitherParsers.Int("2").Map(s => {
                isExectued = true;
                return $"ERROR: {s}";
            }, i => i * 2);

            Assert.False(isExectued, "Leftselector should not be exectued.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4, result.Right);
        }
    }
}