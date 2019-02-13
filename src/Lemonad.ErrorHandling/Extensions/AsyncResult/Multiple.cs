using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        /// <summary>
        ///     An asynchronous version of <see cref="Extensions.Result.Index.Multiple{T,TError}" />.
        /// </summary>
        public static IAsyncResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IAsyncResult<T, TError> source,
            Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>> first,
            Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>> second,
            params Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>>[] validations
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (validations is null) throw new ArgumentNullException(nameof(validations));
            return AsyncResult<T, IReadOnlyList<TError>>.Factory(
                EitherMethods.MultipleAsync(
                    source.Either.ToTaskEither(),
                    first(source).Either.ToTaskEither(),
                    second(source).Either.ToTaskEither(),
                    validations.Select(x => x.Compose(y => y.Either.ToTaskEither())(source))
                )
            );
        }
    }
}