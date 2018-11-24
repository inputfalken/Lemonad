using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.EnumerableExtensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.EnumerableTests {
    public class SingleOrErrorTests {
        [Fact]
        public void No_Predicate_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError(() => "ERROR");
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_Element__Expects_Error() {
            var result = Enumerable.Range(0, 2).SingleOrError(() => "ERROR");

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_No_Element___Expects_Error() {
            var result = Enumerable.Empty<int>().SingleOrError(() => "ERROR");

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void No_Predicate_With_No_Element_Using_Queue___Expects_Error() {
            var result = new Queue<int>(Enumerable.Empty<int>()).SingleOrError(() => "ERROR");

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Empty_Collection__Expects_Error() {
            var result = Enumerable.Empty<int>().SingleOrError(i => i == 2, () => "ERROR");

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Falsy__Expects_Error() {
            var result = Enumerable.Range(10, 2).SingleOrError(i => i == 2, () => "The integer 2 does not exist.");

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("The integer 2 does not exist.", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Falsy_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError(i => i == 10, () => "ERROR");
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Predicate_Truthy__Expects_Value() {
            var result = Enumerable.Range(10, 2).SingleOrError(i => i == 11, () => "The integer 11 does not exist.");

            Assert.False(result.Either.HasError);
            Assert.True(result.Either.HasValue);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal(11, result.Either.Value);
        }

        [Fact]
        public void Predicate_Truthy_Collection_With_Same_Elements__Expects_Error() {
            var result = Enumerable.Repeat(11, 10).SingleOrError(i => i == 11, () => "ERROR");
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }
    }
}