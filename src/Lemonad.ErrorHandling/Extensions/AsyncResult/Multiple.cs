using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="validations"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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