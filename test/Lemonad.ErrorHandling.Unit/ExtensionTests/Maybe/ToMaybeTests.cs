using System;
using Assertion;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe {
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
        public void Predicate_Overload__Falsy_Predicate__Expects_None() {
            string.Empty.ToMaybe(s => s.Length > 5).AssertNone();
        }

        [Fact]
        public void Passing_Null_Value_With_Invalid_Null_Check_Throws() {
            Assert.Throws<InvalidMaybeStateException>(() => {
                string foo = null;
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
        public void Predicate_Overload__Truthy_Predicate__Expects_Value() {
            "Foobar".ToMaybe(s => s.Length > 5).AssertValue("Foobar");
        }
    }
}