using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    public static class ResultEnumerable {
        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" /> to an <see cref="IEnumerable{T}" />
        ///     of
        ///     <typeparamref name="TError" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<TError> Errors<T, TError>(this IEnumerable<IResult<T, TError>> enumerable) =>
            enumerable.SelectMany(x => x.ToErrorEnumerable());

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" /> to an <see cref="IEnumerable{T}" />
        ///     of
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<T> Values<T, TError>(this IEnumerable<IResult<T, TError>> enumerable) =>
            enumerable.SelectMany(x => x.ToEnumerable());

        /// <summary>
        /// Returns the first element of the sequence or a <typeparamref name="TError"/> if no such element is found.
        /// </summary>
        /// <param name="source">
        /// The <see cref="IEnumerable{T}"/> to iterate in.
        /// </param>
        /// <param name="errorSelector">
        /// A function that is invoked if either no element is found.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type returned by function <paramref name="errorSelector"/>.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// When any of the parameters are null.
        /// </exception>
        public static IResult<TSource, TError>
            FirstOrError<TSource, TError>(this IEnumerable<TSource> source, Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            switch (source) {
                case ICollection<TSource> collection when collection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IList<TSource> list when list.Count > 0:
                    return Result.Value<TSource, TError>(list[0]);
                case IReadOnlyCollection<TSource> collection when collection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IReadOnlyList<TSource> readOnlyList when readOnlyList.Count > 0:
                    return Result.Value<TSource, TError>(readOnlyList[0]);
                default:
                    using (var e = source.GetEnumerator()) {
                        return e.MoveNext()
                            ? Result.Value<TSource, TError>(e.Current)
                            : Result.Error<TSource, TError>(errorSelector());
                    }
            }
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or a <typeparamref name="TError"/> if no such element is found.
        /// </summary>
        /// <param name="source">
        /// The <see cref="IEnumerable{T}"/> to iterate in.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element until the condition is fulfilled.
        /// </param>
        /// <param name="errorSelector">
        /// A function that is invoked if either no element is found or the predicate could not be matched with any element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type returned by function <paramref name="errorSelector"/>.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// When any of the parameters are null.
        /// </exception>
        public static IResult<TSource, TError>
            FirstOrError<TSource, TError>(this IEnumerable<TSource> source, Func<TSource, bool> predicate,
                Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            if (source is ICollection<TSource> collection && collection.Count == 0)
                return Result.Error<TSource, TError>(errorSelector());

            foreach (var val in source)
                if (val.IsNotNull() && predicate(val))
                    return Result.Value<TSource, TError>(val);

            return Result.Error<TSource, TError>(errorSelector());
        }

        public static IResult<T, TError> SingleOrError<T, TError>(this IEnumerable<T> source, Func<T, bool> predicate,
            Func<TError> errorSelector) => throw new NotImplementedException();

        public static IResult<T, TError> SingleOrError<T, TError>(this IEnumerable<T> source,
            Func<TError> errorSelector) => throw new NotImplementedException();
    }
}