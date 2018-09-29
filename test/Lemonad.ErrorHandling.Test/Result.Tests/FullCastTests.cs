using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class FullCastTests {
        private enum Error {
            Fail = 1
        }

        private enum Gender {
            Male = 1,
            Female = 2
        }

        private static Result<Gender, Error> Program(int code) {
            switch (code) {
                case 1:
                    return Gender.Male;
                case 2:
                    return Gender.Female;
                default:
                    return Error.Fail;
            }
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string>());
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, string>());
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            var castResult = programResult.FullCast<string, int>();
            Assert.False(castResult.Either.HasValue, "CastResult should not have value");
            Assert.True(castResult.Either.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Either.Error);
            Assert.Equal(default, castResult.Either.Value);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            var castResult = programResult.FullCast<int>();
            Assert.False(castResult.Either.HasValue, "CastResult should not have value");
            Assert.True(castResult.Either.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Either.Error);
            Assert.Equal(default, castResult.Either.Value);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<int, string>());
        }

        [Fact]
        public void Result_With_Error__With_Valid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.Either.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.Either.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Either.Error);
            Assert.Equal(default, programResult.Either.Value);
            var castResult = programResult.FullCast<int, int>();
            Assert.False(castResult.Either.HasValue, "CastResult should not have value");
            Assert.True(castResult.Either.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Either.Error);
            Assert.Equal(default, castResult.Either.Value);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string>());
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, string>());
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, int>());
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            var castResult = programResult.FullCast<int>();
            Assert.True(castResult.Either.HasValue, "CastResult should have value");
            Assert.False(castResult.Either.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Either.Error);
            Assert.Equal(1, castResult.Either.Value);
        }

        [Fact]
        public void Result_With_Value__With_Valid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            var castResult = programResult.FullCast<int, string>();
            Assert.True(castResult.Either.HasValue, "CastResult should have value");
            Assert.False(castResult.Either.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Either.Error);
            Assert.Equal(1, castResult.Either.Value);
        }

        [Fact]
        public void Result_With_Value__With_Valid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.Either.HasValue, "ProgramResult should have value");
            Assert.False(programResult.Either.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Either.Error);
            Assert.Equal(Gender.Male, programResult.Either.Value);
            var castResult = programResult.FullCast<int, int>();
            Assert.True(castResult.Either.HasValue, "CastResult should have value");
            Assert.False(castResult.Either.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Either.Error);
            Assert.Equal(1, castResult.Either.Value);
        }
    }
}