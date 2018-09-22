using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ToMaybeTests {
        [Fact]
        public void Lift_Overload__Some_Should_Always_Have_Value__Except_When_Null_Is_Passed() {
            Assert.True("".ToMaybe().HasValue, "Empty string should have value if called with some");
            Assert.True(0.ToMaybe().HasValue, "Integer '0'  should have value if called with some");
            Assert.True(2.5d.ToMaybe().HasValue, "Double '2.5'  should have value if called with some");
            string isNullString = null;
            Assert.True(isNullString.ToMaybe().HasValue,
                "Strings with value null should have a value since they have no explicitly checked for not null.");

            int? isNullInteger = null;
            Assert.False(isNullInteger.ToMaybe().HasValue,
                "Nullable<int> with value null should not have a value since they have no explicitly checked for not null.");

            double? isNullDouble = null;
            Assert.False(isNullDouble.ToMaybe().HasValue,
                "Nullable<double> with value null should not have a value since they have no explicitly checked for not null.");
        }

        [Fact]
        public void Predicate_Overload__Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            var maybe = string.Empty.ToMaybe(s => s.Length > 5);

            Assert.False(maybe.HasValue, "This predicate should not have a value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Predicate_Overload__Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_True() {
            bool? foo = null;
            var exception = Record.Exception(() => {
                var maybe = foo.ToMaybe(_ => true);

                Assert.True(maybe.HasValue, "This predicate should get evaluated");
                Assert.Equal(default, maybe.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Predicate_Overload__Passing_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".ToMaybe(predicate);
            });
        }

        [Fact]
        public void Predicate_Overload__String_With_Content__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_True() {
            var maybe = "Foobar".ToMaybe(s => s.Length > 5);

            Assert.True(maybe.HasValue, "This predicate should have a value.");
            Assert.Equal("Foobar", maybe.Value);
        }
    }
}