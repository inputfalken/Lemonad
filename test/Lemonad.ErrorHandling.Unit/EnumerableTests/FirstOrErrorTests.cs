using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.EnumerableTests {
    public class FirstOrErrorTests {
        [Fact]
        public void No_Predicate_Collection_With_Same_Elements__Expects_Error() =>
            Enumerable.Repeat(11, 10).FirstOrError(() => "ERROR").AssertValue(11);

        [Fact]
        public void No_Predicate_With_Element__Expects_Value() =>
            Enumerable.Range(0, 2).FirstOrError(() => "ERROR").AssertValue(0);

        [Fact]
        public void No_Predicate_With_No_Element___Expects_Error() =>
            Enumerable.Empty<int>().FirstOrError(() => "ERROR").AssertError("ERROR");

        [Fact]
        public void No_Predicate_With_No_Element_Using_Queue___Expects_Error() =>
            new Queue<int>(Enumerable.Empty<int>()).FirstOrError(() => "ERROR").AssertError("ERROR");

        [Fact]
        public void Predicate_Empty_Collection__Expects_Error() =>
            Enumerable.Empty<int>().FirstOrError(i => i == 2, () => "ERROR").AssertError("ERROR");

        [Fact]
        public void Predicate_Falsy__Expects_Error() => Enumerable.Range(10, 2)
            .FirstOrError(i => i == 2, () => "The integer 2 does not exist.")
            .AssertError("The integer 2 does not exist.");

        [Fact]
        public void Predicate_Falsy_Collection_With_Same_Elements__Expects_Error() => Enumerable.Repeat(11, 10)
            .FirstOrError(i => i == 10, () => "ERROR").AssertError("ERROR");

        [Fact]
        public void Predicate_Truthy__Expects_Value() => Enumerable.Range(10, 2)
            .FirstOrError(i => i == 11, () => "The integer 11 does not exist.").AssertValue(11);

        [Fact]
        public void Predicate_Truthy_Collection_With_Same_Elements__Expects_Value() => Enumerable.Repeat(11, 10)
            .FirstOrError(i => i == 11, () => "ERROR").AssertValue(11);
    }
}