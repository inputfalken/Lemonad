using System;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultOkTests {
        [Fact]
        public void Convert_Int_To_ResultOk() {
            var result = 2.ToResult(x => true, x => "");

            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal(2, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public void Convert_Null_String_To_ResultOk() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.MaybeValueName, () => {
                string str = null;
                var result = str.ToResult(x => true, x => "");

                Assert.True(result.Either.HasValue, "Result should have value.");
                Assert.False(result.Either.HasError, "Result should not have a error value.");
                Assert.Null(result.Either.Value);
            });
        }

        [Fact]
        public void Convert_String_To_ResultOk() {
            var result = "hello".ToResult(x => true, x => "");

            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal("hello", result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }
    }
}