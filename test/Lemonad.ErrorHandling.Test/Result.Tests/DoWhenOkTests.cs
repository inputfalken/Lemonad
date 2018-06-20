using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class DoWhenOkTests {
        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Expects_Action_Not_To_Be_Executed() {
            var result = "ERROR"
                .ToResultError<int, string>()
                .DoWhenOk(_ => throw new Exception("This actions should never be exectued."));
            Assert.False(result.HasValue, "Result should not be error.");
            Assert.True(result.HasError, "Result should be error.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("ERROR", result.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False_Null_Action__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Action<int> action = null;
                var result = "ERROR"
                    .ToResultError<int, string>()
                    .DoWhenOk(action);
                Assert.False(result.HasValue, "Result should not be error.");
                Assert.True(result.HasError, "Result should be error.");
                Assert.Equal(default(int), result.Value);
                Assert.Equal("ERROR", result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Expects_Action_To_Be_Executed() {
            var result = 10
                .ToResult<int, string>()
                .DoWhenOk(i => Assert.Equal(10, i));
            Assert.True(result.HasValue, "Result should be right.");
            Assert.False(result.HasError, "Result should not be error.");
            Assert.Equal(10, result.Value);
            Assert.Equal(default(string), result.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action<int> action = null;
                var result = 10
                    .ToResult<int, string>()
                    .DoWhenOk(action);
                Assert.True(result.HasValue, "Result should be right.");
                Assert.False(result.HasError, "Result should not be error.");
                Assert.Equal(10, result.Value);
                Assert.Equal(default(string), result.Error);
            });
        }
    }
}
