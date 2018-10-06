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
        public void Result_With_Error__With_Invalid_Casting() {
            var programResult = Program(1);
            Assert.False(programResult.Either.HasValue, "Result should not have value");
            Assert.True(programResult.Either.HasError,
                "Resultction ReSharperGotoNextErrorInSolution should have error");
            Assert.Equal(default, programResult.Either.Value);
            Assert.Equal(ExitCodes.Fail, programResult.Either.Error);
            Assert.Throws<InvalidCastException>(() => programResult.CastError<string>());
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var programResult = Program(1);
            Assert.False(programResult.Either.HasValue, "Result should not have value");
            Assert.True(programResult.Either.HasError, "Result should have error");
            Assert.Equal(default, programResult.Either.Value);
            Assert.Equal(ExitCodes.Fail, programResult.Either.Error);

            var castResult = programResult.CastError<int>();

            Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.Either.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal(1, castResult.Either.Error);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(0);
            Assert.True(programResult.Either.HasValue, "Result should have value");
            Assert.False(programResult.Either.HasError, "Result should not have error");
            Assert.Equal("Success", programResult.Either.Value);
            Assert.Equal(default, programResult.Either.Error);

            var exception = Record.Exception(() => {
                var castResult = programResult.CastError<string>();
                Assert.True(castResult.Either.HasValue, "Result should have value");
                Assert.False(castResult.Either.HasError, "Result should not have error");
                Assert.Equal("Success", castResult.Either.Value);
                Assert.Equal(default, castResult.Either.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var programResult = Program(0);
            Assert.True(programResult.Either.HasValue, "Result should have value");
            Assert.False(programResult.Either.HasError, "Result should not have error");
            Assert.Equal("Success", programResult.Either.Value);
            Assert.Equal(default, programResult.Either.Error);

            var castResult = programResult.CastError<int>();

            Assert.True(castResult.Either.HasValue, "Casted Result should have value.");
            Assert.False(castResult.Either.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Either.Error);
            Assert.Equal("Success", castResult.Either.Value);
        }
    }
}