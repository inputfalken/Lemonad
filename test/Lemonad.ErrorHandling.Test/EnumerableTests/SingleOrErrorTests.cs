using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Extensions.Result.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Test.EnumerableTests {
    public class SingleOrErrorTests {
        [Fact]
        public void No_Predicate_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError();
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.ManyElements, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_Element__Expects_Error() {
            var result = Enumerable.Range(0, 2).SingleOrError();

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.ManyElements, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_No_Element___Expects_Error() {
            var result = Enumerable.Empty<int>().SingleOrError();

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_No_Element_Using_Queue___Expects_Error() {
            var result = new Queue<int>(Enumerable.Empty<int>()).SingleOrError();

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Empty_Collection__Expects_Error() {
            var result = Enumerable.Empty<int>().SingleOrError(i => i == 2);

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Falsy__Expects_Error() {
            var result = Enumerable.Range(10, 2).SingleOrError(i => i == 2);

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Falsy_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError(i => i == 10);
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Truthy__Expects_Value() {
            var result = Enumerable.Range(10, 2).SingleOrError(i => i == 11);

            Assert.False(result.Either.HasError);
            Assert.True(result.Either.HasValue);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal(11, result.Either.Value);
        }

        [Fact]
        public void Predicate_Truthy_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError(i => i == 11);
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(SingleOrErrorCase.ManyElements, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }
    }
}