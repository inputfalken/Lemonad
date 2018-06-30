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
        public void Result_With_Value__With_Valid_Casting() {
            var collectionResult = GetCollection(Collection.Array);
            Assert.True(collectionResult.HasValue, "Result should have value");
            Assert.False(collectionResult.HasError, "Result should not have error");
            Assert.IsType<int[]>(collectionResult.Value);
            Assert.Equal(default(string), collectionResult.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<int[]>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to int array.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(new[] {1}, castResult.Value);
            Assert.Equal(default(string), castResult.Error);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var collectionResult = GetCollection(Collection.Array);
            Assert.True(collectionResult.HasValue, "Result should have value");
            Assert.False(collectionResult.HasError, "Result should not have error");
            Assert.IsType<int[]>(collectionResult.Value);
            Assert.Equal(default(string), collectionResult.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<double>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to double.";
            });

            Assert.True(errorSelectorExectued, "Errorselector should get executed");
            Assert.False(castResult.HasValue, "Casted Result should not have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default(double), castResult.Value);
            Assert.Equal("Could not cast collection to double.", castResult.Error);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var collectionResult = GetCollection(Collection.Unknown);
            Assert.False(collectionResult.HasValue, "Result should not have value");
            Assert.True(collectionResult.HasError, "Result should have error");
            Assert.Equal(default(IReadOnlyCollection<int>), collectionResult.Value);
            Assert.Equal("Could not obtain a collection.", collectionResult.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<int[]>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to int array.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.False(castResult.HasValue, "Converted result should not have value");
            Assert.True(castResult.HasError, "Converted Result should have error");
            Assert.Equal(default(int[]), castResult.Value);
            Assert.Equal("Could not obtain a collection.", castResult.Error);
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Casting() {
            var collectionResult = GetCollection(Collection.Unknown);
            Assert.False(collectionResult.HasValue, "Result should not have value");
            Assert.True(collectionResult.HasError, "Result should have error");
            Assert.Equal(default(IReadOnlyCollection<int>), collectionResult.Value);
            Assert.Equal("Could not obtain a collection.", collectionResult.Error);

            var errorSelectorExectued = false;
            var castResult = collectionResult.SafeCast<double>(() => {
                errorSelectorExectued = true;
                return "Could not cast collection to double.";
            });

            Assert.False(errorSelectorExectued, "Errorselector should not get executed");
            Assert.False(castResult.HasValue, "Converted result should not have value");
            Assert.True(castResult.HasError, "Converted Result should have error");
            Assert.Equal(default(double), castResult.Value);
            Assert.Equal("Could not obtain a collection.", castResult.Error);
        }
    }
}