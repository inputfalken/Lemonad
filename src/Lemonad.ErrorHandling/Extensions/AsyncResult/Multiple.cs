using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        public static IAsyncResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IAsyncResult<T, TError> source,
            params Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>>[] validations
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (validations is null) throw new ArgumentNullException(nameof(validations));
            return AsyncResult<T, IReadOnlyList<TError>>.Factory(EitherMethods.MultipleAsync(
                source.Either.ToTaskEither(),
                validations.Select(x =>
                    x.Compose<IAsyncResult<T, TError>, IAsyncResult<T, TError>, Task<IEither<T, TError>>>(y =>
                        y.Either.ToTaskEither<T, TError>())(source)).ToArray()));
        }
    }
}