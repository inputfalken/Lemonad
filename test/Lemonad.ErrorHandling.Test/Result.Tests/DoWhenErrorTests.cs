using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class DoWhenerrorTests {
        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Expects_Action_To_Be_Executed() {
            string value = null;
            var either = "ERROR"
                .ToResultError<int, string>()
                .DoWhenError(s => value = s);
            Assert.Equal("ERROR", value);
            Assert.False(either.HasValue, "Result should not be right.");
            Assert.True(either.HasError, "Result should be error.");
            Assert.Equal(default(int), either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<string> action = null;
                var either = "ERROR"
                    .ToResultError<int, string>()
                    .DoWhenError(action);
                Assert.False(either.HasValue, "Result should not be right.");
                Assert.True(either.HasError, "Result should be error.");
                Assert.Equal(default(int), either.Value);
                Assert.Equal("ERROR", either.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Expects_Action_To_Not_Be_Executed() {
            var either = 10
                .ToResult<int, string>()
                .DoWhenError(_ => throw new Exception("This action should not get exectued."));
            Assert.True(either.HasValue, "Result should be right.");
            Assert.False(either.HasError, "Result should not be error.");
            Assert.Equal(10, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True_Null_Action__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Action<string> action = null;
                var either = 10
                    .ToResult<int, string>()
                    .DoWhenError(action);
                Assert.True(either.HasValue, "Result should be right.");
                Assert.False(either.HasError, "Result should not be error.");
                Assert.Equal(10, either.Value);
                Assert.Equal(default(string), either.Error);
            });
            Assert.Null(exception);
        }
    }
}
