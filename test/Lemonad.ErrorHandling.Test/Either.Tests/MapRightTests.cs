using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class MapRightTests {
        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_False() {
            var isExectued = false;
            var mapRight = ErrorHandling.Either.Parse.Int("foo").MapRight(i => {
                isExectued = true;
                return i * 2;
            });

            Assert.False(isExectued, "Should not get exectued.");
            Assert.True(mapRight.IsLeft, "Either should have a left value.");
            Assert.False(mapRight.IsRight, "Either should not have a right value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", mapRight.Left);
            Assert.Equal(default(int), mapRight.Right);
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True() {
            var isExectued = false;
            var mapRight = ErrorHandling.Either.Parse.Int("2").MapRight(i => {
                isExectued = true;
                return i * 2;
            });

            Assert.True(isExectued, "Should get exectued.");
            Assert.True(mapRight.IsRight, "Either should have a right value.");
            Assert.False(mapRight.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), mapRight.Left);
            Assert.Equal(4, mapRight.Right);
        }

        [Fact]
        public void Either_String_Int_Whose_Property_IsRight_Is_True__Null_Selector__Expects_ArgumentNullExcetpion() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, int> selector = null;
                var mapRight = ErrorHandling.Either.Parse.Int("2").MapRight(selector);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Null_Selector__Expects_No_ArgumentNullExcetpion() {
            var exception = Record.Exception(() => {
                var mapRight = ErrorHandling.Either.Parse.Int("foo").MapRight(i => i * 2);
                Assert.True(mapRight.IsLeft, "Either should have a left value.");
                Assert.False(mapRight.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", mapRight.Left);
                Assert.Equal(default(int), mapRight.Right);
            });
            Assert.Null(exception);
        }
    }
}