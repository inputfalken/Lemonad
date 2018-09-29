using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class FlattenTests {
        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            var result = await flattenAsync;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            var result = await flattenAsync;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });
            var result = await flattenAsync;

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            });
            var result = await flattenAsync;

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flatten = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            var result = await flatten;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flatten = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            var result = await flatten;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            });

            var result = await flattenAsync;

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 0), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 2);
            });

            var result = await flattenAsync;

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flatMap = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should not exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flatMap = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should not exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });
            var result = await flattenAsync;

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            });
            var result = await flattenAsync;

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flattenAsync;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flattenAsync;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            });
            var result = await flattenAsync;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Value_Sync() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 2);
            });
            var result = await flattenAsync;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flattenAsync;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted);
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
        public async Task
            Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            var result = await flattenAsync;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted);
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
        public async Task
            Result_With_Value_FlatmapsRS_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });

            var result = await flattenAsync;

            Assert.True(flatSelectorExecuted);
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
        public async Task
            Result_With_Value_FlatmapsRS_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var flattenAsync = TaskResultFunctions.Flatten(AssertionUtilities.DivisionAsync(2, 2), x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.Division(x, 0);
            });

            var result = await flattenAsync;

            Assert.True(flatSelectorExecuted);
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Either.Error);
        }
    }
}