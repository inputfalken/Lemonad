using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class MatchTests {
        [Fact]
        public void Action_Overload__Match_Maybe_With_Value__Expects_Value_Of_SomeSelector() {
            "hello".SomeWhen(s => true).Match(s => { Assert.Equal("hello", s); }, () => { Assert.True(false); });
        }

        [Fact]
        public void Action_Overload__Match_Maybe_Without_Value__Expects_Value_Of_NoneSelector() {
            "hello".SomeWhen(s => false).Match(s => { Assert.True(false); }, () => { Assert.True(true); });
        }

        [Fact]
        public void Action_Overload__Null_NoneSelector__Throws() {
            Action noneSelector = null;
            Assert.Throws<ArgumentNullException>(() =>
                "hello".SomeWhen(s => false).Match(s => { Assert.True(false); }, noneSelector));
        }

        [Fact]
        public void Action_Overload__Null_NoneSelector_And_SomeSelector__Throws() {
            Action noneSelector = null;
            Action<string> someSelector = null;
            Assert.Throws<ArgumentNullException>(() => "hello".SomeWhen(s => false).Match(someSelector, noneSelector));
        }

        [Fact]
        public void Action_Overload__Null_SomeSelector__Throws() {
            Action<string> someSelector = null;
            Assert.Throws<ArgumentNullException>(() =>
                "hello".SomeWhen(s => false).Match(someSelector, () => { Assert.True(false); }));
        }

        [Fact]
        public void Match_Maybe_With_Value__Expects_Value_Of_SomeSelector() {
            var match = "hello".SomeWhen(s => true).Match(s => s.Length, () => 0);
            Assert.Equal(5, match);
        }

        [Fact]
        public void Match_Maybe_Without_Value__Expects_Value_Of_NoneSelector() {
            var match = "hello".SomeWhen(s => false).Match(s => s.Length, () => 0);
            Assert.Equal(0, match);
        }

        [Fact]
        public void Null_NoneSelector__Throws() {
            Func<int> noneSelector = null;
            Assert.Throws<ArgumentNullException>(() => "hello".SomeWhen(s => false).Match(s => s.Length, noneSelector));
        }

        [Fact]
        public void Null_NoneSelector_And_SomeSelector__Throws() {
            Func<int> noneSelector = null;
            Func<string, int> someSelector = null;
            Assert.Throws<ArgumentNullException>(() => "hello".SomeWhen(s => false).Match(someSelector, noneSelector));
        }

        [Fact]
        public void Null_SomeSelector__Throws() {
            Func<string, int> someSelector = null;
            Assert.Throws<ArgumentNullException>(() => "hello".SomeWhen(s => false).Match(someSelector, () => 2));
        }
    }
}