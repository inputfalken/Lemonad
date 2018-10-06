using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ToMaybeTests {
        [Fact]
        public void Passing_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            var exception = Record.Exception(() => {
                string x = null;
                var foo = x.ToMaybe(s => s != null);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Predicate_Overload__Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            var maybe = string.Empty.ToMaybe(s => s.Length > 5);

            Assert.False(maybe.HasValue, "This predicate should not have a value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Predicate_Overload__Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_True() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherValueName, () => {
                bool? foo = null;
                var maybe = foo.ToMaybe(_ => true);

                Assert.True(maybe.HasValue, "This predicate should get evaluated");
                Assert.Equal(default, maybe.Value);
            });
        }

        [Fact]
        public void Predicate_Overload__Passing_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".ToMaybe(predicate);
            });
        }

        [Fact]
        public void
            Predicate_Overload__String_With_Content__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_True() {
            var maybe = "Foobar".ToMaybe(s => s.Length > 5);

            Assert.True(maybe.HasValue, "This predicate should have a value.");
            Assert.Equal("Foobar", maybe.Value);
        }
    }
}