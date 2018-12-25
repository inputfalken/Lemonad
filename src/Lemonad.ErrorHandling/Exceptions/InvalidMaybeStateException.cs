using System;

namespace Lemonad.ErrorHandling.Exceptions {
    public class InvalidMaybeStateException : Exception {
        public InvalidMaybeStateException(string message) : base(message) { }
    }
}