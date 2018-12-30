using System;
using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Maybe.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe.Enumerable {
    public class ValuesTests {
        [Fact]
        public void Getting_Values_From_Maybe_Enumerable() {
            var maybes = new List<IMaybe<int>> {
                AssertionUtilities.DivisionMaybe(4, 2),
                AssertionUtilities.DivisionMaybe(3, 0),
                AssertionUtilities.DivisionMaybe(3, 3),
                AssertionUtilities.DivisionMaybe(4, 0),
                AssertionUtilities.DivisionMaybe(5, 0),
                AssertionUtilities.DivisionMaybe(6, 0),
                AssertionUtilities.DivisionMaybe(7, 0),
                AssertionUtilities.DivisionMaybe(8, 0),
                AssertionUtilities.DivisionMaybe(10, 2)
            };

            var list = maybes.Values().ToArray();
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }

        [Fact]
        public void Passing_Null_Maybe_When_Getting_Values_From_Maybe_Enumerable() {
            IEnumerable<IMaybe<int>> maybes = null;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => maybes.Values()
            );
        }
    }
}