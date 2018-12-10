using System;
using System.Linq;
using System.Linq.Expressions;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    /// <summary>
    ///     Contains extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static class ResultQueryable {
        /// <summary>
        ///     Returns the first element of the sequence or a <typeparamref name="TError" /> if no such element is found.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IQueryable{T}" /> to search in.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked if either no element is found.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the parameters are null.
        /// </exception>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(
            this IQueryable<TSource> source,
            Func<TError> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            // Since anonymous types are reference types, It's possible to wrap the value type in an anonymous type and perform a null check.
            return default(TSource).IsValueType()
                ? source
                    .Select(x => new {LemonadValueTypeWrapper = x})
                    .FirstOrDefault()
                    .ToResult(x => !(x is null), _ => errorSelector())
                    .Map(x => x.LemonadValueTypeWrapper)
                : source
                    .FirstOrDefault()
                    .ToResult(x => x != null, _ => errorSelector());
        }

        /// <summary>
        ///     Returns the first element of the sequence that satisfies a condition or a <typeparamref name="TError" /> if no such
        ///     element is found.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IQueryable{T}" /> to search in.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element until the condition is fulfilled.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked if either no element is found or the predicate could not be matched with any element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the parameters are null.
        /// </exception>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Func<TError> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            // Since anonymous types are reference types, It's possible to wrap the value type in an anonymous type and perform a null check.
            return default(TSource).IsValueType()
                ? source
                    .Where(predicate)
                    .Select(x => new {LemonadValueTypeWrapper = x})
                    .FirstOrDefault()
                    .ToResult(x => !(x is null), _ => errorSelector())
                    .Map(x => x.LemonadValueTypeWrapper)
                : source
                    .Where(predicate)
                    .FirstOrDefault()
                    .ToResult(x => x != null, _ => errorSelector());
        }
    }
}