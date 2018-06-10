using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class MapLeftTests {
        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False() {
            var isExectued = false;
            var mapRight = EitherParsers.Int("foo").MapLeft(i => {
                isExectued = true;
                return i;
            });

            Assert.True(isExectued, "Should get exectued.");
            Assert.True(mapRight.IsLeft, "Either should have a left value.");
            Assert.False(mapRight.IsRight, "Either should not have a right value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", mapRight.Left);
            Assert.Equal(default(int), mapRight.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_Selector__Expects_ArgumentNullExcetpion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string> selector = null;
                var mapRight = EitherParsers.Int("foo").MapLeft(selector);
            });
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True() {
            var isExectued = false;
            var mapRight = EitherParsers.Int("2").MapLeft(i => {
                isExectued = true;
                return i;
            });

            Assert.False(isExectued, "Should not get exectued.");
            Assert.True(mapRight.IsRight, "Either should have a right value.");
            Assert.False(mapRight.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), mapRight.Left);
            Assert.Equal(2, mapRight.Right);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Null_Selector__Expects_No_ArgumentNullExcetpion() {
            var exception = Record.Exception(() => {
                Func<string, string> selector = null;
                var mapRight = EitherParsers.Int("2").MapLeft(selector);
                Assert.True(mapRight.IsRight, "Either should have a right value.");
                Assert.False(mapRight.IsLeft, "Either should not have a left value.");
                Assert.Equal(2, mapRight.Right);
                Assert.Equal(default(string), mapRight.Left);
            });
            Assert.Null(exception);
        }
    }
}