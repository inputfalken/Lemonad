using System;
using System.Linq;
using System.Linq.Expressions;

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
        ) => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.Cast<object>()
                .FirstOrDefault()
                .ToResult(x => x != null, _ => errorSelector())
                .Cast<TSource>();

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
        ) => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.Where(predicate)
                .Cast<object>()
                .FirstOrDefault()
                .ToResult(x => x != null, _ => errorSelector())
                .Cast<TSource>();
    }
}