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
                    .ToResult(x => !((object) x is null), _ => errorSelector());
        }

        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="Result{T,TError}"/> from.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="Result{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(this IQueryable<TSource> source) {
            var sources = source
                .Take(2)
                .ToArray();
            if (sources.Length == 1) return Result.Value<TSource, SingleOrErrorCase>(sources[0]);
            return Result.Error<TSource, SingleOrErrorCase>(sources.Length == 0
                ? SingleOrErrorCase.NoElement
                : SingleOrErrorCase.ManyElements
            );
        }

        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="Result{T,TError}"/> from.
        /// </param>
        /// <param name="predicate">
        /// A <see cref="Expression{TDelegate}"/> to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="Result{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate
        ) => source.Where(predicate).SingleOrError();

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
            return source.Where(predicate).FirstOrError(errorSelector);
        }
    }
}