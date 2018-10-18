using System;
using System.Linq;
using System.Linq.Expressions;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    // TODO find a way to not have to specify constrains. (The constrains are needed since structs are hard to handle.)
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
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource> source,
            Func<TError> errorSelector) where TSource : class => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.FirstOrDefault().ToResult(x => x != null, _ => errorSelector());

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
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : class =>
            errorSelector == null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : source.FirstOrDefault(predicate).ToResult(x => x != null, _ => errorSelector());

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
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource?> source,
            Func<TError> errorSelector) where TSource : struct => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.FirstOrDefault().ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

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
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource?> source,
            Expression<Func<TSource?, bool>> predicate, Func<TError> errorSelector) where TSource : struct =>
            errorSelector == null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : source.FirstOrDefault(predicate).ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

        /// <summary>
        ///     Returns a single, specific element of a sequence, or a <typeparamref name="TError" /> if that element is not found.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IQueryable{T}" /> to return the single element of.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked when no element is found or the predicate could not be matched with any element or more
        ///     than one element is found.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource> source,
            Func<TError> errorSelector) where TSource : class => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.SingleOrDefault().ToResult(x => x != null, _ => errorSelector());

        /// <summary>
        ///     Returns the only element of a sequence that satisfies a specified condition <typeparamref name="TError" /> if no
        ///     such element exists.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IQueryable{T}" /> to return the single element of.
        /// </param>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="TSource" /> for a condition.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked when no element is found or the predicate could not be matched with any element or more
        ///     than one element is found that matches the predicate.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : class =>
            errorSelector == null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : source.SingleOrDefault(predicate).ToResult(x => x != null, _ => errorSelector());

        /// <summary>
        ///     Returns a single, specific element of a sequence, or a <typeparamref name="TError" /> if that element is not found.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IQueryable{T}" /> to return the single element of.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked when no element is found or the predicate could not be matched with any element or more
        ///     than one element is found.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource?> source,
            Func<TError> errorSelector) where TSource : struct => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.SingleOrDefault().ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

        /// <summary>
        ///     Returns the only element of a sequence that satisfies a specified condition <typeparamref name="TError" /> if no
        ///     such element exists.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IQueryable{T}" /> to return the single element of.
        /// </param>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="TSource" /> for a condition.
        /// </param>
        /// <param name="errorSelector">
        ///     A function that is invoked when no element is found or the predicate could not be matched with any element or more
        ///     than one element is found that matches the predicate.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by function <paramref name="errorSelector" />.
        /// </typeparam>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource?> source,
            Expression<Func<TSource?, bool>> predicate, Func<TError> errorSelector) where TSource : struct =>
            errorSelector == null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : source.SingleOrDefault(predicate).ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);
    }
}