using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lemonad.ErrorHandling.Internal {
    /// <summary>
    ///     Source: https://github.com/louthy/language-ext/blob/master/LanguageExt.Core/Extensions/ObjectExt.cs
    /// </summary>
    internal static class EqualityFunctions {
        /// <summary>
        ///     Returns true if the value is null, and does so without
        ///     boxing of any value-types.  Value-types will always
        ///     return false.
        /// </summary>
        /// <example>
        ///     int x = 0;
        ///     string y = null;
        ///     x.IsNull()  // false
        ///     y.IsNull()  // true
        /// </example>
        /// <returns>
        ///     True if the value is null, and does so without
        ///     boxing of any value-types.  Value-types will always
        ///     return false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(this T value) => Check<T>.IsNull(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueType<T>(this T value) => Check<T>.IsValueType(value);

        private static class Check<T> {
            private static readonly EqualityComparer<T> DefaultEqualityComparer;
            private static readonly bool IsNullable;
            private static readonly bool IsReferenceType;

            static Check() {
                IsNullable = Nullable.GetUnderlyingType(typeof(T)) is null == false;
                IsReferenceType = !typeof(T).GetTypeInfo().IsValueType;
                DefaultEqualityComparer = EqualityComparer<T>.Default;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool IsNull(T value) =>
                IsNullable
                    ? value.Equals(default(T))
                    : IsReferenceType && DefaultEqualityComparer.Equals(value, default);

            internal static bool IsValueType(T _) => IsReferenceType == false;
        }

    }
}