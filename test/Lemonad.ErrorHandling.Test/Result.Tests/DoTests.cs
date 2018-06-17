using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class DoTests {
        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Expects_Action_To_Be_Executed() {
            var isExecuted = false;
            var either = "ERROR"
                .ToResultError<int, string>()
                .Do(() => isExecuted = true);
            Assert.True(isExecuted, "Should get exectued.");
            Assert.False(either.HasValue, "Result should not be right.");
            Assert.True(either.HasError, "Result should be error.");
            Assert.Equal(default(int), either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var either = "ERROR"
                    .ToResultError<int, string>()
                    .Do(action);
                Assert.False(either.HasValue, "Result should not be right.");
                Assert.True(either.HasError, "Result should be error.");
                Assert.Equal(default(int), either.Value);
                Assert.Equal("ERROR", either.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Expects_Action_To_Not_Be_Executed() {
            var isExecuted = false;
            var either = 10
                .ToResult<int, string>()
                .Do(() => isExecuted = true);

            Assert.True(isExecuted, "Should get exectued.");
            Assert.True(either.HasValue, "Result should be right.");
            Assert.False(either.HasError, "Result should not be error.");
            Assert.Equal(10, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var either = 10
                    .ToResult<int, string>()
                    .Do(action);
                Assert.True(either.HasValue, "Result should be right.");
                Assert.False(either.HasError, "Result should not be error.");
                Assert.Equal(10, either.Value);
                Assert.Equal(default(string), either.Error);
            });
        }
    }
}
