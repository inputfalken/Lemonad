using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class CastErrorTests {
        private enum ExitCodes {
            Fail = 1,
            Unhandled
        }

        private static Result<string, ExitCodes> Program(int code) {
            switch (code) {
                case 0:
                    return "Success";
                case 1:
                    return ExitCodes.Fail;
                default:
                    return ExitCodes.Unhandled;
            }
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var programResult = Program(0);
            Assert.True(programResult.HasValue, "Result should have value");
            Assert.False(programResult.HasError, "Result should not have error");
            Assert.Equal("Success", programResult.Value);
            Assert.Equal(default, programResult.Error);

            var castResult = programResult.CastError<int>();

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Error);
            Assert.Equal("Success", castResult.Value);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(0);
            Assert.True(programResult.HasValue, "Result should have value");
            Assert.False(programResult.HasError, "Result should not have error");
            Assert.Equal("Success", programResult.Value);
            Assert.Equal(default, programResult.Error);

            var exception = Record.Exception(() => {
                var castResult = programResult.CastError<string>();
                Assert.True(castResult.HasValue, "Result should have value");
                Assert.False(castResult.HasError, "Result should not have error");
                Assert.Equal("Success", castResult.Value);
                Assert.Equal(default, castResult.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Casting() {
            var programResult = Program(1);
            Assert.False(programResult.HasValue, "Result should not have value");
            Assert.True(programResult.HasError, "Result should have error");
            Assert.Equal(default, programResult.Value);
            Assert.Equal(ExitCodes.Fail, programResult.Error);
            Assert.Throws<InvalidCastException>(() => programResult.CastError<string>());
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var programResult = Program(1);
            Assert.False(programResult.HasValue, "Result should not have value");
            Assert.True(programResult.HasError, "Result should have error");
            Assert.Equal(default, programResult.Value);
            Assert.Equal(ExitCodes.Fail, programResult.Error);

            var castResult = programResult.CastError<int>();

            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal(1, castResult.Error);
        }
    }
}