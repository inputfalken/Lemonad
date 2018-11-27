using System;
using System.Collections.Generic;
using Lemonad.ErrorHandling.Internal;

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
    public interface IValidator<out T, TError> : IReadOnlyCollection<TError> {
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
        IValidator<T, TError> Validate(Func<T, bool> predicate, Func<T, TError> errorSelector);

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
        IValidator<T, TError> Validate(Func<T, bool> predicate, TError error);

        /// <summary>
        /// Gets the <see cref="IResult{T,TError}"/>.
        /// </summary>
        IResult<T, IReadOnlyCollection<TError>> Result { get; }
    }
}