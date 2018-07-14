namespace Lemonad.ErrorHandling.Test.Result.Tests {
    internal static class AssertionUtilities {
        internal static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";

            return left / right;
        }
    }
}