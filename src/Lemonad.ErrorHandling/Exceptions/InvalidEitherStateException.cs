using System;

namespace Lemonad.ErrorHandling.Exceptions {
    public class InvalidEitherStateException : Exception {
        public InvalidEitherStateException(string message) : base(message) { }
    }
}