using System;
using Assertion;
using Lemonad.ErrorHandling.Internal.Either;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Internal.Tests.Either.Tests {
    public class NonNullableEitherTests {
        [Fact]
        public void String_Int_Either_With_Error() {
            var value = "foo";
            var error = 2;
            var either = new NonNullableEither<string, int>(in value, in error, true,
                false);
            Assert.Equal("foo", either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasError);
        }

        [Fact]
        public void String_Int_Either_With_Error_And_Value_Throws() {
            var value = "foo";
            var error = 2;
            Assert.Throws<ArgumentException>(() =>
                new NonNullableEither<string, int>(in value, in error, true,
                    true));
        }

        /// <summary>
        ///     Would be nice if this was not allowed.
        /// </summary>
        [Fact]
        public void String_Int_Either_With_Null_Error() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorParamName, () => {
                var value = "foo";
                int? error = null;
                var either =
                    new NonNullableEither<string, int?>(in value, in error, true,
                        false);
                Assert.Equal("foo", either.Value);
                Assert.Null(either.Error);
                Assert.NotEqual(either.HasError, either.HasValue);
                Assert.True(either.HasError);
            });
        }

        /// <summary>
        ///     Would be nice if this was not allowed.
        /// </summary>
        [Fact]
        public void String_Int_Either_With_Null_Value() {
            string value = null;
            var error = 2;
            var either =
                new NonNullableEither<string, int>(in value, in error, true,
                    false);
            Assert.Null(either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasError);
        }

        [Fact]
        public void String_Int_Either_With_Value() {
            var value = "foo";
            var error = 2;
            var either =
                new NonNullableEither<string, int>(in value, in error, false,
                    true);
            Assert.Equal("foo", either.Value);
            Assert.Equal(2, either.Error);
            Assert.NotEqual(either.HasError, either.HasValue);
            Assert.True(either.HasValue);
        }
    }
}