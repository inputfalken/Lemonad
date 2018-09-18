using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test {
    public class OutcomeAndResultTypeTests {
        [Fact]
        public void Share_Same_Generic_Public_Instance_Methods() {
            IReadOnlyList<string> Filter(Type type) {
                var exclusions = type
                    .GetInterfaces()
                    .SelectMany(x => x.GetMethods())
                    .Concat(typeof(object).GetMethods())
                    .Select(x => x.Name)
                    .ToList();

                return type
                    .GetMethods()
                    .Where(x => exclusions.Contains(x.Name) == false)
                    .Where(x => x.IsStatic == false)
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToArray();
            }

            var result = typeof(Result<string, string>);
            var resultMethods = Filter(result);

            var outcome = typeof(AsyncResult<string, string>);
            var outcomeMethods = Filter(outcome);

            var differences = (resultMethods.Count > outcomeMethods.Count
                    ? resultMethods.Where(x => outcomeMethods.Contains(x) == false)
                    : outcomeMethods.Where(x => resultMethods.Contains(x) == false))
                .ToArray();
            var difference = differences.Aggregate("Method differences:",
                (x, y) => $"{x}{Environment.NewLine}\tName of method: '{y}'");

            Assert.True(differences.Length == 0, difference);
        }
    }
}