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

        public static IResult<TSource, TError>
            FirstOrError<TSource, TError>(this IEnumerable<TSource> source, Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            switch (source) {
                case IList<TSource> list when list.Count > 0:
                    return Result.Value<TSource, TError>(list[0]);
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

        public static IResult<T, TError>
            FirstOrError<T, TError>(this IEnumerable<T> source, Func<T, bool> predicate, Func<TError> errorSelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            foreach (var val in source)
                if (val.IsNotNull() && predicate(val))
                    return Result.Value<T, TError>(val);

            return Result.Error<T, TError>(errorSelector());
        }
    }
}