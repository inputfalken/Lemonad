using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class DoTests {
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__Expects_Action_To_Be_Executed() {
            var isExecuted = false;
            var either = "ERROR"
                .ToEitherLeft<string, int>()
                .Do(() => isExecuted = true);
            Assert.True(isExecuted, "Should get exectued.");
            Assert.False(either.IsRight, "Either should not be right.");
            Assert.True(either.IsLeft, "Either should be left.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var either = "ERROR"
                    .ToEitherLeft<string, int>()
                    .Do(action);
                Assert.False(either.IsRight, "Either should not be right.");
                Assert.True(either.IsLeft, "Either should be left.");
                Assert.Equal(default(int), either.Right);
                Assert.Equal("ERROR", either.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__Expects_Action_To_Not_Be_Executed() {
            var isExecuted = false;
            var either = 10
                .ToEitherRight<string, int>()
                .Do(() => isExecuted = true);

            Assert.True(isExecuted, "Should get exectued.");
            Assert.True(either.IsRight, "Either should be right.");
            Assert.False(either.IsLeft, "Either should not be left.");
            Assert.Equal(10, either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var either = 10
                    .ToEitherRight<string, int>()
                    .Do(action);
                Assert.True(either.IsRight, "Either should be right.");
                Assert.False(either.IsLeft, "Either should not be left.");
                Assert.Equal(10, either.Right);
                Assert.Equal(default(string), either.Left);
            });
        }
    }
}