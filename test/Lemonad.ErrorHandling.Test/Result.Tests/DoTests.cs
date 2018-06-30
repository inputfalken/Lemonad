using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class DoTests {
        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False__Expects_Action_To_Be_Executed() {
            var isExecuted = false;
            var result = "ERROR"
                .ToResultError<int, string>()
                .Do(() => isExecuted = true);
            Assert.True(isExecuted, "Should get exectued.");
            Assert.False(result.HasValue, "Result should not be error.");
            Assert.True(result.HasError, "Result should be error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("ERROR", result.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_False_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var result = "ERROR"
                    .ToResultError<int, string>()
                    .Do(action);
                Assert.False(result.HasValue, "Result should not be error.");
                Assert.True(result.HasError, "Result should be error.");
                Assert.Equal(default, result.Value);
                Assert.Equal("ERROR", result.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True__Expects_Action_To_Not_Be_Executed() {
            var isExecuted = false;
            var result = 10
                .ToResult<int, string>()
                .Do(() => isExecuted = true);

            Assert.True(isExecuted, "Should get exectued.");
            Assert.True(result.HasValue, "Result should be right.");
            Assert.False(result.HasError, "Result should not be error.");
            Assert.Equal(10, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public void
            Result_String_Int_Whose_Property_HasValue_Is_True_Null_Action__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Action action = null;
                var result = 10
                    .ToResult<int, string>()
                    .Do(action);
                Assert.True(result.HasValue, "Result should be right.");
                Assert.False(result.HasError, "Result should not be error.");
                Assert.Equal(10, result.Value);
                Assert.Equal(default, result.Error);
            });
        }
    }
}