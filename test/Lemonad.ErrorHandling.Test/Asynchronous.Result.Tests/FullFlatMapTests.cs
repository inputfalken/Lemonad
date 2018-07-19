using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class FullFlatMapTests {
        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source..");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(0.5d, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return DivisionAsync(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;
            Assert.True(flatSelectorExecuted, "Flatmapselecotr should get executed.");
            Assert.True(resultSelectorExectued,
                "ResultSelector should get executed since both source and the result from flatmapselector contains values.");
            Assert.False(errorSelectorExecuted,
                "Erroselector should not get executed since both source and the result from flatmapselector contains values.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(1.5d, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source..");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Value__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 0).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value_Sync() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(0.5d, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error_Sync() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            var result = await fullFlatMap;

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Value__Expects_Result_With_Value_Sync() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            var fullFlatMap = DivisionAsync(2, 2).FullFlatMap(x => {
                flatSelectorExecuted = true;
                return Division(x, 2);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.False(flatSelectorExecuted, "flatselector should get exectued before the value is awaited.");
            Assert.False(errorSelectorExecuted,
                "ErrorSelectorExecuted Should not get exectued before the value is awaited.");
            var result = await fullFlatMap;
            Assert.True(flatSelectorExecuted, "Flatmapselecotr should get executed.");
            Assert.True(resultSelectorExectued,
                "ResultSelector should get executed since both source and the result from flatmapselector contains values.");
            Assert.False(errorSelectorExecuted,
                "Erroselector should not get executed since both source and the result from flatmapselector contains values.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(1.5d, result.Value);
            Assert.Equal(default, result.Error);
        }
    }
}