using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Lemonad.ErrorHandling.Test {
    public class OutcomeAndResultTypeTests {
        [Fact]
        public void Share_Same_Generic_Public_Instance_Methods() {
            IReadOnlyList<string> Filter(Type type) {
                var methodInfos = type.GetInterfaces().SelectMany(x => x.GetMethods()).Select(x => x.Name).ToList();
                return type
                    .GetMethods()
                    .Where(x => methodInfos.Contains(x.Name) == false)
                    .Where(x => x.IsStatic == false)
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToArray();
            }

            var result = typeof(Result<string, string>);
            var resultMethods = Filter(result);

            var outcome = typeof(Outcome<string, string>);
            var outcomeMethods = Filter(outcome);

            Assert.Equal(resultMethods, outcomeMethods);
        }
    }
}