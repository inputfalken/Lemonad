using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Lemonad.ErrorHandling.Extensions.Task;
using Microsoft.EntityFrameworkCore;

namespace Lemonad.ErrorHandling.EntityFramework.Core {
    public static partial class Index {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValueType<T>(T value) => Check<T>.IsValueType(value);

        private static class Check<T> {
            private static readonly bool IsReferenceType;

            static Check() => IsReferenceType = !typeof(T).GetTypeInfo().IsValueType;

            internal static bool IsValueType(T _) => IsReferenceType == false;
        }

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
        public static IAsyncResult<TSource, TError> FirstOrErrorAsync<TSource, TError>(
            this IQueryable<TSource> source,
            Func<TError> errorSelector
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));

            // Since anonymous types are reference types, It's possible to wrap the value type in an anonymous type and perform a null check.
            return IsValueType(default(TSource))
                ? source
                    .Select(x => new {LemonadValueTypeWrapper = x})
                    .FirstOrDefaultAsync()
                    .ToAsyncResult(x => !(x is null), _ => errorSelector())
                    .Map(x => x.LemonadValueTypeWrapper)
                : source
                    .FirstOrDefaultAsync()
                    .ToAsyncResult(x => !((object) x is null), _ => errorSelector());
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
        public static IAsyncResult<TSource, TError> FirstOrErrorAsync<TSource, TError>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Func<TError> errorSelector
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return source.Where(predicate).FirstOrErrorAsync(errorSelector);
        }
    }
}