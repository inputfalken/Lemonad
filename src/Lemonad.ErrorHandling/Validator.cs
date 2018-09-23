using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling {
    /// <summary>
    /// An <typeparamref name="TError"/> collection of <typeparamref name="TError"/> based on validations of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the validation candidate.
    /// </typeparam>
    /// <typeparam name="TError">
    /// The type of the the error type.
    /// </typeparam>
    public readonly struct Validator<T, TError> : IReadOnlyCollection<TError> {
        private readonly T _candidate;
        private readonly Queue<TError> _errors;

        /// <summary>
        /// Convert to a <see cref="Result{T,TError}"/>.
        /// </summary>
        public Result<T, IReadOnlyCollection<TError>> Result {
            get {
                var candidate = _candidate;
                return _errors.ToResultError<T, IReadOnlyCollection<TError>>(x => x.Count > 0, () => candidate);
            }
        }

        /// <summary>
        ///  Creates an instance of <see cref="Validator{T,TError}"/>.
        /// </summary>
        /// <param name="candidate">
        /// The validation <paramref name="candidate"/>.
        /// </param>
        public Validator(T candidate) {
            _candidate = candidate;
            _errors = new Queue<TError>();
        }

        private Validator(in Queue<TError> errors, in T candidate) {
            _errors = errors;
            _candidate = candidate;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<TError> GetEnumerator() => _errors.GetEnumerator();

        /// <summary>
        /// Validate <typeparamref name="T"/> with an <paramref name="predicate"/> function and set the failure type in <paramref name="errorSelector"/>.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/> for a condition.
        /// </param>
        /// <param name="errorSelector">
        /// A function to set the error if the predicate function would return false.
        /// </param>
        /// <returns>
        /// An <see cref="Validator{T,TError}"/>.
        /// </returns>
        public Validator<T, TError> Validate(Func<T, bool> predicate, Func<TError> errorSelector) {
            var result = _candidate.ToResult(predicate, errorSelector);
            if (result.HasError) _errors.Enqueue(result.Error);
            return new Validator<T, TError>(_errors, _candidate);
        }

        /// <summary>
        /// Validate <typeparamref name="T"/> with an <paramref name="predicate"/> function and set the failure type with <paramref name="error"/>.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/> for a condition.
        /// </param>
        /// <param name="error">
        /// The error value.
        /// </param>
        /// <returns>
        /// An <see cref="Validator{T,TError}"/>.
        /// </returns>
        public Validator<T, TError> Validate(Func<T, bool> predicate, TError error) => Validate(predicate, () => error);

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
        public int Count => _errors.Count;
    }
}