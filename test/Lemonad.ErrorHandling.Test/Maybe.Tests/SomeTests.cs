using System;
using Lemonad.ErrorHandling.DataTypes.Maybe.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SomeTests {
        [Fact]
        public void Lift_Overload__Some_Should_Always_Have_Value__Except_When_Null_Is_Passed() {
            Assert.True("".Some().HasValue, "Empty string should have value if called with some");
            Assert.True(0.Some().HasValue, "Integer '0'  should have value if called with some");
            Assert.True(2.5d.Some().HasValue, "Double '2.5'  should have value if called with some");
            string isNullString = null;
            Assert.True(isNullString.Some().HasValue,
                "Strings with value null should have a value since they have no explicitly checked for not null.");

            int? isNullInteger = null;
            Assert.True(isNullInteger.Some().HasValue,
                "Nullable<int> with value null should have a value since they have no explicitly checked for not null.");

            double? isNullDouble = null;
            Assert.True(isNullDouble.Some().HasValue,
                "Nullable<double> with value null should have a value since they have no explicitly checked for not null.");
        }

        [Fact]
        public void Predicate_Overload__Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            var maybe = string.Empty.Some(s => s.Length > 5);

            Assert.False(maybe.HasValue, "This predicate should not have a value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Predicate_Overload__Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_True() {
            bool? foo = null;
            var exception = Record.Exception(() => {
                var maybe = foo.Some(_ => true);

                Assert.True(maybe.HasValue, "This predicate should get evaluated");
                Assert.Equal(default, maybe.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Predicate_Overload__Passing_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".Some(predicate);
            });
        }

        [Fact]
        public void Predicate_Overload__String_With_Content__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_True() {
            var maybe = "Foobar".Some(s => s.Length > 5);

            Assert.True(maybe.HasValue, "This predicate should have a value.");
            Assert.Equal("Foobar", maybe.Value);
        }
    }
}