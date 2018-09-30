using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test {
    public class EitherTests {
        [Fact]
        public void String_Int_Either_With_Value() {
            var value = "foo";
            var error = 2;
            var either =
                new Result<string, int>.NonNullEither(value: in value, error: in error, hasError: false,
                    hasValue: true);
            Assert.Equal("foo", either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasValue);
        }

        [Fact]
        public void String_Int_Either_With_Error() {
            var value = "foo";
            var error = 2;
            var either =
                new Result<string, int>.NonNullEither(value: in value, error: in error, hasError: true,
                    hasValue: false);
            Assert.Equal("foo", either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasError);
        }

        /// <summary>
        /// Would be nice if this was not allowed.
        /// </summary>
        [Fact]
        public void String_Int_Either_With_Null_Error() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherErrorName, () => {
                var value = "foo";
                int? error = null;
                var either =
                    new Result<string, int?>.NonNullEither(value: in value, error: in error, hasError: true,
                        hasValue: false);
                Assert.Equal("foo", either.Value);
                Assert.Null(either.Error);
                Assert.NotEqual(either.HasError, either.HasValue);
                Assert.True(either.HasError);
            });
        }

        /// <summary>
        /// Would be nice if this was not allowed.
        /// </summary>
        [Fact]
        public void String_Int_Either_With_Null_Value() {
            string value = null;
            var error = 2;
            var either =
                new Result<string, int>.NonNullEither(value: in value, error: in error, hasError: true,
                    hasValue: false);
            Assert.Null(either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasError);
        }

        [Fact]
        public void String_Int_Either_With_Error_And_Value_Throws() {
            var value = "foo";
            var error = 2;
            Assert.Throws<ArgumentException>(() =>
                new Result<string, int>.NonNullEither(value: in value, error: in error, hasError: true,
                    hasValue: true));
        }
    }
}