using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class FlatMapSameTErrorAsyncTests {
        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 0), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            });

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 0), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            });

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Error__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 0), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            });

            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 0), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            });
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 2), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            });

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 2), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            });
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(0.5d, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 2), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            });

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var result = await TaskResultFunctions.FlatMap(Division(2, 2), x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            });
            Assert.True(flatSelectorExecuted, "Flatmapselecotr should get executed.");
            Assert.True(resultSelectorExectued,
                "ResultSelector should get executed since both source and the result from flatmapselector contains values.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(1.5d, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }
    }
}