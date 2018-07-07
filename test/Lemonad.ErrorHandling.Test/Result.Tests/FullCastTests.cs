using System;
using Lemonad.ErrorHandling.DataTypes.Result;
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
        public void Result_With_Error__With_Valid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            var castResult = programResult.FullCast<int, int>();
            Assert.False(castResult.HasValue, "CastResult should not have value");
            Assert.True(castResult.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Error);
            Assert.Equal(default, castResult.Value);
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, string>());
        }

        [Fact]
        public void Result_With_Error__With_Valid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<int, string>());
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            var castResult = programResult.FullCast<string, int>();
            Assert.False(castResult.HasValue, "CastResult should not have value");
            Assert.True(castResult.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Error);
            Assert.Equal(default, castResult.Value);
        }

        [Fact]
        public void Result_With_Value__With_Valid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            var castResult = programResult.FullCast<int, int>();
            Assert.True(castResult.HasValue, "CastResult should have value");
            Assert.False(castResult.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Error);
            Assert.Equal(1, castResult.Value);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, string>());
        }

        [Fact]
        public void Result_With_Value__With_Valid_Value_Casting_And_Invalid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            var castResult = programResult.FullCast<int, string>();
            Assert.True(castResult.HasValue, "CastResult should have value");
            Assert.False(castResult.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Error);
            Assert.Equal(1, castResult.Value);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Value_Casting_And_Valid_Error_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string, int>());
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            var castResult = programResult.FullCast<int>();
            Assert.True(castResult.HasValue, "CastResult should have value");
            Assert.False(castResult.HasError, "CastResult should not have error");
            Assert.Equal(default, castResult.Error);
            Assert.Equal(1, castResult.Value);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(1);
            Assert.True(programResult.HasValue, "ProgramResult should have value");
            Assert.False(programResult.HasError, "ProgramResult should not have error");
            Assert.Equal(default, programResult.Error);
            Assert.Equal(Gender.Male, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string>());
        }
        
        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            var castResult = programResult.FullCast<int>();
            Assert.False(castResult.HasValue, "CastResult should not have value");
            Assert.True(castResult.HasError, "CastResult should have error");
            Assert.Equal(1, castResult.Error);
            Assert.Equal(default, castResult.Value);
        }

        [Fact]
        public void Result_With_Error__With_Invalid_Casting() {
            var programResult = Program(5);
            Assert.False(programResult.HasValue, "ProgramResult should not have value");
            Assert.True(programResult.HasError, "ProgramResult should have error");
            Assert.Equal(Error.Fail, programResult.Error);
            Assert.Equal(default, programResult.Value);
            Assert.Throws<InvalidCastException>(() => programResult.FullCast<string>());
        }
    }
}