using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class DoWhenLeftTests {
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Expects_Action_To_Be_Executed() {
            string value = null;
            var either = "ERROR"
                .ToEitherLeft<string, int>()
                .DoWhenLeft(s => value = s);
            Assert.Equal("ERROR", value);
            Assert.False(either.IsRight, "Either should not be right.");
            Assert.True(either.IsLeft, "Either should be left.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> action = null;
                var either = "ERROR"
                    .ToEitherLeft<string, int>()
                    .DoWhenLeft(action);
                Assert.False(either.IsRight, "Either should not be right.");
                Assert.True(either.IsLeft, "Either should be left.");
                Assert.Equal(default(int), either.Right);
                Assert.Equal("ERROR", either.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Expects_Action_To_Not_Be_Executed() {
            var either = 10
                .ToEitherRight<string, int>()
                .DoWhenLeft(_ => throw new Exception("This action should not get exectued."));
            Assert.True(either.IsRight, "Either should be right.");
            Assert.False(either.IsLeft, "Either should not be left.");
            Assert.Equal(10, either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True_Null_Action__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Action<string> action = null;
                var either = 10
                    .ToEitherRight<string, int>()
                    .DoWhenLeft(action);
                Assert.True(either.IsRight, "Either should be right.");
                Assert.False(either.IsLeft, "Either should not be left.");
                Assert.Equal(10, either.Right);
                Assert.Equal(default(string), either.Left);
            });
            Assert.Null(exception);
        }
    }
}