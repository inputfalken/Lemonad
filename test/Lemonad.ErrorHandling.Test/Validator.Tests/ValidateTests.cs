using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Validator.Tests {
    public class ValidateTests {
        [Fact]
        public void Single_False_Validation() {
            const string error = "Not dividable by 2";
            var validator = new Validator<int, string>(1).Validate(i => i % 2 == 0, error).ToList();
            Assert.Single(validator, error);
        }

        [Fact]
        public void Single_True_Validation() {
            const string error = "Not dividable by 2";
            var validator = new Validator<int, string>(2).Validate(i => i % 2 == 0, error).ToList();
            Assert.Empty(validator);
        }

        [Fact]
        public void Double_True_Validation() {
            const string error1 = "Not dividable by 2";
            const string error2 = "Is equal to 2.";
            var validator = new Validator<int, string>(2)
                .Validate(i => i % 2 == 0, error1)
                .Validate(i => i == 2, () => error2)
                .ToList();
            Assert.Empty(validator);
        }

        [Fact]
        public void Double_False_Validation() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var validator = new Validator<int, string>(1)
                .Validate(i => i == 0, () => error1)
                .Validate(i => i % 2 == 0, () => error2)
                .ToList();
            Assert.Equal(2, validator.Count);
            Assert.Equal(error1, validator[0]);
            Assert.Equal(error2, validator[1]);
        }

        [Fact]
        public void Single_True_Validation_And_Single_False_Validation() {
            const string error1 = "Is not equal to 0";
            const string error2 = "Not dividable by 2";
            var validator = new Validator<int, string>(2)
                .Validate(i => i == 0, () => error1)
                .Validate(i => i % 2 == 0, () => error2)
                .ToList();
            Assert.Single(validator);
        }
    }
}