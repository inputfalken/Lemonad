using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SomWhenTests {
        [Fact]
        public void _Empty_String__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_False() {
            Assert.False("".Some().SomeWhen(s => s.Length > 5).HasValue,
                "This should not have a value since the predicate returns false.");
        }

        [Fact]
        public void String_With_Content__Length_Is_Greather_Than_5__Expects_HasValue_To_Be_True() {
            Assert.True("Foobar".Some().SomeWhen(s => s.Length > 5).HasValue,
                "This should have a value since the predicate returns true.");
        }
        
        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".Some().SomeWhen(predicate);
            });
        }
    }
}