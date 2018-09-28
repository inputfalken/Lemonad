using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///  Source: https://github.com/louthy/language-ext/blob/master/LanguageExt.Core/Extensions/ObjectExt.cs
    /// </summary>
    internal static class EqualityFunctions {
        /// <summary>
        /// Returns true if the value is equal to this type's
        /// default value.
        /// </summary>
        /// <example>
        ///     0.IsDefault()  // true
        ///     1.IsDefault()  // false
        /// </example>
        /// <returns>True if the value is equal to this type's
        /// default value</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDefault<T>(this T value) =>
            Check<T>.IsDefault(value);

        /// <summary>
        /// Returns true if the value is null, and does so without
        /// boxing of any value-types.  Value-types will always
        /// return false.
        /// </summary>
        /// <example>
        ///     int x = 0;
        ///     string y = null;
        ///     
        ///     x.IsNull()  // false
        ///     y.IsNull()  // true
        /// </example>
        /// <returns>True if the value is null, and does so without
        /// boxing of any value-types.  Value-types will always
        /// return false.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(this T value) =>
            Check<T>.IsNull(value);

        private static class Check<T> {
            private static readonly bool IsReferenceType;
            private static readonly bool IsNullable;
            private static readonly EqualityComparer<T> DefaultEqualityComparer;

            static Check() {
                IsNullable = Nullable.GetUnderlyingType(typeof(T)) != null;
                IsReferenceType = !typeof(T).GetTypeInfo().IsValueType;
                DefaultEqualityComparer = EqualityComparer<T>.Default;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool IsDefault(T value) =>
                DefaultEqualityComparer.Equals(value, default);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool IsNull(T value) =>
                IsNullable
                    ? value.Equals(default(T))
                    : IsReferenceType && DefaultEqualityComparer.Equals(value, default);
        }
    }
}