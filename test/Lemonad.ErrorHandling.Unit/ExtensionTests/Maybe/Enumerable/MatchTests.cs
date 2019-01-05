using System;
using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Maybe.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe.Enumerable {
    public class MatchTests {
        [Fact]
        public void Passing_Null_As_Enumerable_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((List<IMaybe<int>>) null).Match(i => i, () => -1)
            );
        }

        [Fact]
        public void Passing_Null_As_None_Selector_Throws() {
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

            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.MaybeNoneSelector,
                () => maybes.Match(i => i, null)
            );
        }

        [Fact]
        public void Passing_Null_As_Value_Selector_Throws() {
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

            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.MaybeValueSelector,
                () => maybes.Match(null, () => -1)
            );
        }

        [Fact]
        public void Works_Like_Extension_Methods_Values_And_Nones() {
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
            var matches = maybes
                .Match(i => i, () => -1)
                .ToArray();

            Assert.Equal(maybes.Values(), matches.Where(x => x != -1));
            Assert.Equal(maybes.Nones(() => -1), matches.Where(x => x == -1));
        }
    }
}