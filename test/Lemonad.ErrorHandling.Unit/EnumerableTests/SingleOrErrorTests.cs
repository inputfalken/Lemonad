using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Extensions.Result.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.EnumerableTests {
    public class SingleOrErrorTests {
        [Fact]
        public void No_Predicate_Collection_With_Same_Elements__Expects_Error() => Enumerable.Repeat(11, 10)
            .SingleOrError().AssertError(SingleOrErrorCase.ManyElements);

        [Fact]
        public void No_Predicate_With_Element__Expects_Error() =>
            Enumerable.Range(0, 2).SingleOrError().AssertError(SingleOrErrorCase.ManyElements);

        [Fact]
        public void No_Predicate_With_No_Element___Expects_Error() =>
            Enumerable.Empty<int>().SingleOrError().AssertError(SingleOrErrorCase.NoElement);

        [Fact]
        public void No_Predicate_With_No_Element_Using_Queue___Expects_Error() =>
            new Queue<int>(Enumerable.Empty<int>()).SingleOrError().AssertError(SingleOrErrorCase.NoElement);

        [Fact]
        public void Predicate_Empty_Collection__Expects_Error() => Enumerable.Empty<int>().SingleOrError(i => i == 2)
            .AssertError(SingleOrErrorCase.NoElement);

        [Fact]
        public void Predicate_Falsy__Expects_Error() => Enumerable.Range(10, 2).SingleOrError(i => i == 2)
            .AssertError(SingleOrErrorCase.NoElement);

        [Fact]
        public void Predicate_Falsy_Collection_With_Same_Elements__Expects_Error() => Enumerable.Repeat(11, 10)
            .SingleOrError(i => i == 10).AssertError(SingleOrErrorCase.NoElement);

        [Fact]
        public void Predicate_Truthy__Expects_Value() =>
            Enumerable.Range(10, 2).SingleOrError(i => i == 11).AssertValue(11);

        [Fact]
        public void Predicate_Truthy_Collection_With_Same_Elements__Expects_Error() => Enumerable.Repeat(11, 10)
            .SingleOrError(i => i == 11).AssertError(SingleOrErrorCase.ManyElements);
    }
}