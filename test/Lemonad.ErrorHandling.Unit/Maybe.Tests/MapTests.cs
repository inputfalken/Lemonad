using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class MapTests {
        [Fact]
        public void Mapping_Integer_With_Multiplication() {
            ErrorHandling.Maybe.Value(20).Map(s => s * 2).AssertValue(40);
        }

        [Fact]
        public void Mapping_String_Length() {
            ErrorHandling.Maybe.Value("hello").Map(s => s.Length).AssertValue(5);
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> function = null;
                ErrorHandling.Maybe.Value("foo").Map(function);
            });
        }
    }
}