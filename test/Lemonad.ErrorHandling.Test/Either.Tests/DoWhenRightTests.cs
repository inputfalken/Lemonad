using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class DoWhenRightTests {
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<int> action = null;
                var either = 10
                    .ToEitherRight<string, int>()
                    .DoWhenRight(action);
                Assert.True(either.IsRight, "Either should be right.");
                Assert.False(either.IsLeft, "Either should not be left.");
                Assert.Equal(10, either.Right);
                Assert.Equal(default(string), either.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Expects_Action_To_Be_Executed() {
            var either = 10
                .ToEitherRight<string, int>()
                .DoWhenRight(i => Assert.Equal(10, i));
            Assert.True(either.IsRight, "Either should be right.");
            Assert.False(either.IsLeft, "Either should not be left.");
            Assert.Equal(10, either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Expects_Action_Not_To_Be_Executed() {
            var either = "ERROR"
                .ToEitherLeft<string, int>()
                .DoWhenRight(_ => Assert.True(false, "This actions should never be exectued."));
            Assert.False(either.IsRight, "Either should not be right.");
            Assert.True(either.IsLeft, "Either should be left.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False_Null_Action__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Action<int> action = null;
                var either = "ERROR"
                    .ToEitherLeft<string, int>()
                    .DoWhenRight(action);
                Assert.False(either.IsRight, "Either should not be right.");
                Assert.True(either.IsLeft, "Either should be left.");
                Assert.Equal(default(int), either.Right);
                Assert.Equal("ERROR", either.Left);
            });
            Assert.Null(exception);
        }
    }
}