using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SomeTests {
        [Fact]
        public void Lift_Overload__Some_Should_Always_Have_Value__Except_When_Null_Is_Passed() {
            Assert.True("".Some().HasValue, "Empty string should have value if called with some");
            Assert.True(0.Some().HasValue, "Integer '0'  should have value if called with some");
            Assert.True(2.5d.Some().HasValue, "Double '2.5'  should have value if called with some");
            string isNullString = null;
            Assert.False(isNullString.Some().HasValue,
                "Strings with value null should always have no value if null is passed.");

            int? isNullInteger = null;
            Assert.False(isNullInteger.Some().HasValue,
                "Nullables with value null should always have no value if null is passed.");

            double? isNullDouble = null;
            Assert.False(isNullDouble.Some().HasValue,
                "Nullables with value null should always have no value if null is passed.");
        }

        [Fact]
        public void Predicate_Overload__Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            var maybe = string.Empty.SomeWhen(s => s.Length > 5);

            Assert.False(maybe.HasValue, "This predicate should not have a value.");
            Assert.Equal(default(string), maybe.Value);
        }

        [Fact]
        public void Predicate_Overload__Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_False() {
            bool? foo = null;
            var maybe = foo.SomeWhen(_ => throw new ArgumentNullException());

            Assert.False(maybe.HasValue, "This predicate should not be evaluated.");
            Assert.Equal(default(bool?), foo);
        }

        [Fact]
        public void Predicate_Overload__Passing_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".SomeWhen(predicate);
            });
        }

        [Fact]
        public void Predicate_Overload__String_With_Content__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_True() {
            var maybe = "Foobar".SomeWhen(s => s.Length > 5);

            Assert.True(maybe.HasValue, "This predicate should have a value.");
            Assert.Equal("Foobar", maybe.Value);
        }
    }
}