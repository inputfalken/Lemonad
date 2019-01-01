using System;
using System.Collections.Generic;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Maybe.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe.Enumerable {
    public class FirstMaybeTests {
        [Fact]
        public void No_Predicate_With_Enumerable_Who_Has_Elements() {
            const int first = 2;
            const int second = 2;
            new List<int> {
                first,
                second
            }.FirstMaybe().AssertValue(first);
        }

        [Fact]
        public void No_Predicate_Passing_Null_Enumerable_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IEnumerable<int>) null).FirstMaybe()
            );

        [Fact]
        public void Passing_Null_Predicate_Throws() {
            const int first = 2;
            const int second = 2;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => new List<int> {
                    first,
                    second
                }.FirstMaybe(null));
        }

        [Fact]
        public void With_Truthy_Predicate_Passing_Null_Enumerable_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IEnumerable<int>) null).FirstMaybe(_ => true)
            );

        [Fact]
        public void With_Falsy_Predicate_Passing_Null_Enumerable_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IEnumerable<int>) null).FirstMaybe(_ => false)
            );

        [Fact]
        public void No_Predicate_With_Enumerable_Who_Has_No_Elements() => new List<int>().FirstMaybe().AssertNone();

        [Fact]
        public void With_Truthy_Predicate_With_Enumerable_Who_Has_No_Elements() =>
            new List<int>().FirstMaybe(i => i == 4).AssertNone();

        [Fact]
        public void With_Falsy_Predicate_With_Enumerable_Who_Has_No_Elements() =>
            new List<int>().FirstMaybe(i => i == 1337).AssertNone();

        [Fact]
        public void With_Truthy_Predicate_With_Enumerable_Who_Has_Elements() {
            const int first = 2;
            const int second = 4;
            new List<int> {
                first,
                second
            }.FirstMaybe(i => i == 4).AssertValue(second);
        }

        [Fact]
        public void With_Falsy_Predicate_With_Enumerable_Who_Has_Elements() {
            const int first = 2;
            const int second = 4;
            new List<int> {
                first,
                second
            }.FirstMaybe(i => i == 1337).AssertNone();
        }
    }
}