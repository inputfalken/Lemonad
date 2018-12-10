using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
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
        ///     Returns the first element of the sequence or a <typeparamref name="TError" /> if no such element is found.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> to iterate in.
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
        public static IResult<TSource, TError>
            FirstOrError<TSource, TError>(this IEnumerable<TSource> source, Func<TError> errorSelector) {
            // Since anonymous types are reference types, It's possible to wrap the value type in an anonymous type and perform a null check.
            return default(TSource).IsValueType()
                ? source
                    .Select(x => new {LemonadValueTypeWrapper = x})
                    .FirstOrDefault()
                    .ToResult(x => x != null, _ => errorSelector())
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
        ///     The <see cref="IEnumerable{T}" /> to iterate in.
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
        public static IResult<TSource, TError>
            FirstOrError<TSource, TError>(this IEnumerable<TSource> source, Func<TSource, bool> predicate,
                Func<TError> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            // Since anonymous types are reference types, It's possible to wrap the value type in an anonymous type and perform a null check.
            if (default(TSource).IsValueType())
                return source
                    .Where(predicate)
                    .Select(x => new {LemonadValueTypeWrapper = x})
                    .FirstOrDefault()
                    .ToResult(x => x != null, _ => errorSelector())
                    .Map(x => x.LemonadValueTypeWrapper);

            return source
                .Where(predicate)
                .FirstOrDefault()
                .ToResult(x => x != null, _ => errorSelector());
        }

        /// <summary>
        ///     Returns a single, specific element of a sequence, or a <typeparamref name="TError" /> if that element is not found.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}" /> to return the single element of.
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
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IEnumerable<TSource> source,
            Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            switch (source) {
                case ICollection<TSource> collection when collection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IReadOnlyCollection<TSource> readOnlyCollection when readOnlyCollection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IList<TSource> list:
                    return list.Count == 0
                        ? Result.Error<TSource, TError>(errorSelector())
                        : list.Count == 1
                            ? Result.Value<TSource, TError>(list[0])
                            : Result.Error<TSource, TError>(errorSelector());
                case IReadOnlyList<TSource> readOnlyList:
                    return readOnlyList.Count == 0
                        ? Result.Error<TSource, TError>(errorSelector())
                        : readOnlyList.Count == 1
                            ? Result.Value<TSource, TError>(readOnlyList[0])
                            : Result.Error<TSource, TError>(errorSelector());
                default: {
                    using (var e = source.GetEnumerator()) {
                        if (!e.MoveNext())
                            return Result.Error<TSource, TError>(errorSelector());

                        var result = e.Current;
                        if (!e.MoveNext())
                            return Result.Value<TSource, TError>(result);
                    }

                    return Result.Error<TSource, TError>(errorSelector());
                }
            }
        }

        /// <summary>
        ///     Returns the only element of a sequence that satisfies a specified condition <typeparamref name="TError" /> if no
        ///     such element exists.
        /// </summary>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}" /> to return the single element of.
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
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            switch (source) {
                case ICollection<TSource> collection when collection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IReadOnlyCollection<TSource> readOnlyCollection when readOnlyCollection.Count == 0:
                    return Result.Error<TSource, TError>(errorSelector());
                case IList<TSource> list:
                    return list.Count == 0
                        ? Result.Error<TSource, TError>(errorSelector())
                        : list.Count == 1 && predicate(list[0])
                            ? Result.Value<TSource, TError>(list[0])
                            : Result.Error<TSource, TError>(errorSelector());
                case IReadOnlyList<TSource> readOnlyList:
                    return readOnlyList.Count == 0
                        ? Result.Error<TSource, TError>(errorSelector())
                        : readOnlyList.Count == 1 && predicate(readOnlyList[0])
                            ? Result.Value<TSource, TError>(readOnlyList[0])
                            : Result.Error<TSource, TError>(errorSelector());
                default: {
                    using (var e = source.GetEnumerator()) {
                        while (e.MoveNext()) {
                            var result = e.Current;
                            if (!predicate(result)) continue;
                            while (e.MoveNext())
                                if (predicate(e.Current))
                                    return Result.Error<TSource, TError>(errorSelector());

                            return Result.Value<TSource, TError>(result);
                        }
                    }

                    return Result.Error<TSource, TError>(errorSelector());
                }
            }
        }

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
    }
}