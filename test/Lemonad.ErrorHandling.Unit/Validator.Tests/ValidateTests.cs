using System.Collections;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Validator.Tests {
    public class ValidateTests {
        [Fact]
        public void Single_False_Validation() {
            const string error = "Not dividable by 2";
            var validator = ErrorHandling.Validator.Value<int, string>(1).Validate(i => i % 2 == 0, error).ToList();
            Assert.Single(validator, error);
        }

        [Fact]
        public void Single_True_Validation() {
            const string error = "Not dividable by 2";
            var validator = ErrorHandling.Validator.Value<int, string>(2).Validate(i => i % 2 == 0, error).ToList();
            Assert.Empty(validator);
        }

        [Fact]
        public void Double_True_Validation() {
            const string error1 = "Not dividable by 2";
            const string error2 = "Is equal to 2.";
            var validator = ErrorHandling.Validator.Value<int, string>(2)
                .Validate(i => i % 2 == 0, error1)
                .Validate(i => i == 2, x => error2)
                .ToList();
            Assert.Empty(validator);
        }

        [Fact]
        public void Double_True_Validation_Verify_States() {
            const string error1 = "Not dividable by 2";
            const string error2 = "Is equal to 2.";
            var state0 = ErrorHandling.Validator.Value<int, string>(2);
            Assert.Empty(state0);
            var state1 = state0.Validate(i => i % 2 == 0, error1);
            Assert.Empty(state1);
            var state2 = state1.Validate(i => i == 2, x => error2);
            Assert.Empty(state2);
            var validator = state2.ToList();
            Assert.Empty(validator);
        }

        [Fact]
        public void Double_False_Validation() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var validator = ErrorHandling.Validator.Value<int, string>(1)
                .Validate(i => i == 0, x => error1)
                .Validate(i => i % 2 == 0, x => error2)
                .ToList();
            Assert.Equal(2, validator.Count);
            Assert.Equal(error1, validator[0]);
            Assert.Equal(error2, validator[1]);
        }

        [Fact]
        public void Double_False_Validation_Verify_States() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var state0 = ErrorHandling.Validator.Value<int, string>(1);
            Assert.Empty(state0);
            var state1 = state0.Validate(i => i == 0, x => error1);
            Assert.Single(state1);
            var state2 = state1.Validate(i => i % 2 == 0, x => error2);
            Assert.Equal(2, state2.Count);
            var validator = state2.ToList();
            Assert.Equal(2, validator.Count);
            Assert.Equal(error1, validator[0]);
            Assert.Equal(error2, validator[1]);
        }

        [Fact]
        public void First_False_Validation_And_Second_True_Validation_Verify_States() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var state0 = ErrorHandling.Validator.Value<int, string>(2);
            var state1 = state0.Validate(i => i == 0, x => error1);
            var state2 = state1.Validate(i => i % 2 == 0, x => error2);
            // These errors occur since the collection in `state2` references the collection in `state0`.
            // So all errors in `state2` are found in `state0` therefor.
            Assert.Empty(state0);
            Assert.Single(state1);
            Assert.Single(state2);
        }

        [Fact]
        public void First_True_Validation_And_Second_False_Validation_Verify_States() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var state0 = ErrorHandling.Validator.Value<int, string>(2);
            var state1 = state0.Validate(i => i % 2 == 0, x => error2);
            var state2 = state1.Validate(i => i == 0, x => error1);
            Assert.Empty(state0);
            Assert.Empty(state1);
            Assert.Single(state2);
        }

        [Fact]
        public void First_False_Validation_And_Second_True_Validation() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var validator = ErrorHandling.Validator.Value<int, string>(2)
                .Validate(i => i == 0, x => error1)
                .Validate(i => i % 2 == 0, x => error2)
                .ToList();
            Assert.Single((IEnumerable) validator);
        }

        [Fact]
        public void First_True_Validation_And_Second_False_Validation() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var validator = ErrorHandling.Validator.Value<int, string>(2)
                .Validate(i => i % 2 == 0, x => error2)
                .Validate(i => i == 0, x => error1)
                .ToList();
            Assert.Single((IEnumerable) validator);
        }

        [Fact]
        public void Result_Does_Not_Create_New_Instance_On_Get() {
            var validator = ErrorHandling.Validator.Value<int, string>(2);
            Assert.Equal(validator.Result, validator.Result);
            Assert.StrictEqual(validator.Result, validator.Result);
        }
    }
}