using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Test {
    internal static class AssertionUtilities {
        private static Task Delay => Task.Delay(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 200 : 50);

        internal static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";
            return left / right;
        }

        internal static async Task<Result<double, string>> DivisionAsync(double left, double right) {
            await Delay;

            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";
            return left / right;
        }

        internal static async Task<Result<Gender, string>> GetGender(int identity) {
            await Delay;
            switch (identity) {
                case 0:
                    return Gender.Male;
                case 1:
                    return Gender.Female;
                default:
                    return "Could not determine gender";
            }
        }

        internal static async Task<Result<string, ExitCodes>> Program(int code) {
            await Delay;

            switch (code) {
                case 0:
                    return "Success";
                case 1:
                    return ExitCodes.Fail;
                default:
                    return ExitCodes.Unhandled;
            }
        }

        internal enum ExitCodes {
            Fail = 1,
            Unhandled
        }

        internal enum Gender {
            Male = 0,
            Female = 1
        }
    }
}