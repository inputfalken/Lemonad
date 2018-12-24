using System;
using Assertion;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class ToMaybeNoneTests {
        [Fact]
        public void Empty_String() {
            string.Empty.ToMaybeNone(string.IsNullOrWhiteSpace).AssertNone();
        }

        [Fact]
        public void Null_String_With_Value_Should_Throw() {
            Assert.Throws<InvalidMaybeStateException>(() => {
                string f = null;
                var noneWhenStringIsNullOrEmpty = f.ToMaybeNone(_ => false);

                Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
                Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
            });
        }

        [Fact]
        public void String_With_Content() {
            "hello".ToMaybeNone(string.IsNullOrWhiteSpace).AssertValue("hello");
        }
    }
}