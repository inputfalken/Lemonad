using System.Collections.Generic;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class OfTypeTests {
        private enum Collection {
            Array = 0,
            List = 1,
            Unknown
        }

        private static Result<IReadOnlyCollection<int>, string> GetCollection(Collection collection) {
            switch (collection) {
                case Collection.Array:
                    return new[] {1};
                case Collection.List:
                    return new List<int> {1};
                default:
                    return "Could not obtain a collection.";
            }
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Casting() {
            var collectionResult = GetCollection(Collection.Unknown);
            Assert.False(collectionResult.Either.HasValue, "Result should not have value");
            Assert.True(collectionResult.Either.HasError, "Result should have error");
            Assert.Equal(default, collectionResult.Either.Value);
            Assert.Equal("Could not obtain a collection.", collectionResult.Either.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<double>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to double.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.False(castResult.Either.HasValue, "Converted result should not have value");
            Assert.True(castResult.Either.HasError, "Converted Result should have error");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal("Could not obtain a collection.", castResult.Either.Error);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var collectionResult = GetCollection(Collection.Unknown);
            Assert.False(collectionResult.Either.HasValue, "Result should not have value");
            Assert.True(collectionResult.Either.HasError, "Result should have error");
            Assert.Equal(default, collectionResult.Either.Value);
            Assert.Equal("Could not obtain a collection.", collectionResult.Either.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<int[]>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to int array.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.False(castResult.Either.HasValue, "Converted result should not have value");
            Assert.True(castResult.Either.HasError, "Converted Result should have error");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal("Could not obtain a collection.", castResult.Either.Error);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var collectionResult = GetCollection(Collection.Array);
            Assert.True(collectionResult.Either.HasValue, "Result should have value");
            Assert.False(collectionResult.Either.HasError, "Result should not have error");
            Assert.IsType<int[]>(collectionResult.Either.Value);
            Assert.Equal(default, collectionResult.Either.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<double>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to double.";
            });

            Assert.True(errorSelectorExectued, "Errorselector should get executed");
            Assert.False(castResult.Either.HasValue, "Casted Result should not have value.");
            Assert.True(castResult.Either.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal("Could not cast collection to double.", castResult.Either.Error);
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var collectionResult = GetCollection(Collection.Array);
            Assert.True(collectionResult.Either.HasValue, "Result should have value");
            Assert.False(collectionResult.Either.HasError, "Result should not have error");
            Assert.IsType<int[]>(collectionResult.Either.Value);
            Assert.Equal(default, collectionResult.Either.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<int[]>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to int array.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.True(castResult.Either.HasValue, "Casted Result should have value.");
            Assert.False(castResult.Either.HasError, "Casted Result should not have error.");
            Assert.Equal(new[] {1}, castResult.Either.Value);
            Assert.Equal(default, castResult.Either.Error);
        }
    }
}