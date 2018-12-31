using System;
using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Maybe.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe.Enumerable {
    public class NonesTests {
        [Fact]
        public void Passing_Null_Maybe_When_Getting_Errors_From_Maybe_Enumerable_Throws() {
            IEnumerable<IMaybe<int>> maybes = null;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => maybes.Nones(() => 2)
            );
        }

        [Fact]
        public void Passing_Null_Selector_When_Getting_Errors_From_Maybe_Enumerable() {
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
            Func<int> selector = null;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => maybes.Nones(selector)
            );
        }

        [Fact]
        public void Getting_Errors_From_Maybe_Enumerable() {
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

            var list = maybes.Nones(() => "Failed divisions.").ToArray();
            Assert.Equal(6, list.Length);
        }
    }
}