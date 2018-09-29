﻿using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class MapErrorTests {
        [Fact]
        public async Task Result_With_Error__Expects_Error_To_Be_Mapped() {
            var errorSelectorInvoked = false;
            var task = TaskResultFunctions.MapError(AssertionUtilities.DivisionAsync(10, 0), s => {
                errorSelectorInvoked = true;
                return s.ToUpper();
            });

            var result = await task;
            Assert.True(errorSelectorInvoked,
                "Errorselector should get exeuted since there is an error in the result.");
            Assert.False(result.Either.HasValue, "Result should not have a value.");
            Assert.True(result.Either.HasError, "Result should have a error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("CAN NOT DIVIDE '10' WITH '0'.", result.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value__Expects_Error_To_Not_Be_Mapped() {
            var errorSelectorInvoked = false;
            var result = await TaskResultFunctions.MapError(AssertionUtilities.DivisionAsync(10, 2), s => {
                errorSelectorInvoked = true;
                return s.ToUpper();
            });

            Assert.False(errorSelectorInvoked,
                "Errorselector not should get exeuted since there is an value in the result.");
            Assert.True(result.Either.HasValue, "Result should have a value.");
            Assert.False(result.Either.HasError, "Result should not have a error.");
            Assert.Equal(5d, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }
    }
}