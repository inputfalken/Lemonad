using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SelectManyTests {
        [Fact]
        public void Enumerable_ResultSelector_SelectMany_FlatMap_Maybe_With_Value() {
            const string input = "hello";
            var list = Enumerable
                .Range(0, 3)
                .SelectMany(_ => input.ToMaybeNone(string.IsNullOrWhiteSpace).ToEnumerable(), (x, y) => x + y.Length)
                .ToList();

            Assert.Equal(5, list[0]);
            Assert.Equal(6, list[1]);
            Assert.Equal(7, list[2]);
        }

        [Fact]
        public void Enumerable_ResultSelector_SelectMany_FlatMap_Maybe_Without_Value() {
            const string input = "hello";
            var list = Enumerable
                .Range(0, 3)
                .SelectMany(_ => input.ToMaybe(y => y.Length > 10).ToEnumerable(), (x, y) => x + y.Length)
                .ToList();

            Assert.False(list.Any());
        }

        [Fact]
        public void Predicate_Overload__Passing_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IEnumerable<string>> func = null;
                ErrorHandling.Maybe.Value("foo").ToEnumerable().SelectMany(func);
            });
        }
    }
}