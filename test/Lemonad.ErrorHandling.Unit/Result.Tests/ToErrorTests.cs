using System;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultErrorTests {
        [Fact]
        public void Convert_Int_To_ResultError() {
            var result = 2.ToResultError(i => true, x => "");

            Assert.False(result.Either.HasValue, "Result should have error.");
            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.Equal(2, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Convert_Null_String_To_ResultError() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherErrorName, () => {
                string str = null;
                var result = str.ToResultError(x => true, x => "");

                Assert.False(result.Either.HasValue, "Result should have error.");
                Assert.True(result.Either.HasError, "Result should have a error value.");
                Assert.Null(result.Either.Error);
                Assert.Equal(default, result.Either.Value);
            });
        }

        [Fact]
        public void Convert_String_To_ResultError() {
            var result = "hello".ToResultError(s => true, x => "");

            Assert.False(result.Either.HasValue, "Result should have value.");
            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.Equal("hello", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }
    }
}