using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test {
    public class OutcomeAndResultTypeTests {
        [Fact]
        public void Share_Same_Generic_Public_Instance_Methods() {
            IReadOnlyList<string> Filter(Type type) => type
                .GetMethods()
                .Where(x => x.IsGenericMethod)
                .Where(x => x.IsPublic)
                .Where(x => x.IsStatic == false)
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToArray();

            var result = typeof(Result<string, string>);
            var resultMethods = Filter(result);

            var outcome = typeof(Outcome<string, string>);
            var outcomeMethods = Filter(outcome);

            Assert.True(
                resultMethods.Count == outcomeMethods.Count,
                $"'{result.Name}' and '{outcome.Name}' do not share the same amount of public methods."
            );
            var array = resultMethods.Zip(outcomeMethods, (x, y) => new {Result = x, Outcome = y}).ToArray();
            Assert.True(array.All(x => x.Outcome == x.Result), "The method name differs.");
        }
    }
}