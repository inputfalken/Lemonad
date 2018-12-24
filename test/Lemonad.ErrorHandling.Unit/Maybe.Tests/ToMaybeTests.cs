using System;
using Assertion;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class ToMaybeTests {
        [Fact]
        public void Passing_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            var exception = Record.Exception(() => {
                string x = null;
                var foo = x.ToMaybe(s => s is null == false);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Predicate_Overload__Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            string.Empty.ToMaybe(s => s.Length > 5).AssertNone();
        }

        [Fact]
        public void Predicate_Overload__Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_True() {
            Assert.Throws<InvalidMaybeStateException>(() => {
                bool? foo = null;
                foo.ToMaybe(_ => true).AssertValue(default);
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
            "Foobar".ToMaybe(s => s.Length > 5).AssertValue("Foobar");
        }
    }
}