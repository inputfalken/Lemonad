using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError" />.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="additional">
        ///     A <see cref="IReadOnlyList{T}" /> containing <typeparamref name="TError" />.
        /// </param>
        /// <param name="first"></param>
        public static IResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IResult<T, TError> source,
            Func<IResult<T, TError>, IResult<T, TError>> first,
            Func<IResult<T, TError>, IResult<T, TError>> second,
            params Func<IResult<T, TError>, IResult<T, TError>>[] additional
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (additional is null) throw new ArgumentNullException(nameof(additional));

            var either = EitherMethods.Multiple(
                initial: source.Either,
                first: first.Compose(x => x.Either)(source),
                second: second.Compose(x => x.Either)(source),
                additional: additional.Select(x => x.Compose(y => y.Either)(source)
                )
            );
            return either.HasValue
                ? ErrorHandling.Result.Value<T, IReadOnlyList<TError>>(either.Value)
                : ErrorHandling.Result.Error<T, IReadOnlyList<TError>>(either.Error);
        }
    }
}