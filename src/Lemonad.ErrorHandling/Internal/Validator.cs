using System;
using System.Collections;
using System.Collections.Generic;
using Lemonad.ErrorHandling.Extensions;

namespace Lemonad.ErrorHandling.Internal {
    internal readonly struct Validator<T, TError> : IValidator<T, TError> {
        private readonly T _candidate;
        private readonly Queue<TError> _errors;

        public IResult<T, IReadOnlyCollection<TError>> Result { get; }

        internal Validator(T candidate) {
            _candidate = candidate;
            _errors = new Queue<TError>();
            Result = ErrorHandling.Result.Value<T, IReadOnlyCollection<TError>>(_candidate);
        }

        private Validator(in Queue<TError> errors, in T candidate) {
            _errors = new Queue<TError>(errors);
            _candidate = candidate;
            Result = _errors.Count > 0
                ? ErrorHandling.Result.Error<T, IReadOnlyCollection<TError>>(_errors)
                : ErrorHandling.Result.Value<T, IReadOnlyCollection<TError>>(_candidate);
        }

        public IEnumerator<TError> GetEnumerator() => _errors.GetEnumerator();

        public IValidator<T, TError> Validate(Func<T, bool> predicate, Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentException(nameof(errorSelector));
            var result = _candidate.ToResult(predicate, errorSelector);
            // This refers to the initial queue.
            if (result.Either.HasError) _errors.Enqueue(result.Either.Error);
            return new Validator<T, TError>(_errors, _candidate);
        }

        public IValidator<T, TError> Validate(Func<T, bool> predicate, TError error) => Validate(predicate, _ => error);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _errors.Count;
    }
}