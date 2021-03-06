﻿using System;
using System.Linq;

namespace Lemonad.ErrorHandling.Internal {
    internal static class StringFunctions {
        public static string PrettyTypeString(object item) {
            switch (item) {
                case null:
                    return "(null)";
                case string str:
                    return $"(\"{str.Replace("\\", "\\")}\")";
                case char letter:
                    return $"('{letter}')";
                default:
                    return $"({item})";
            }
        }

        internal static string ToHumanString(this Type t) => t.IsGenericType
            ? $"{t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.InvariantCulture))}<{string.Join(", ", t.GetGenericArguments().Select(ToHumanString))}>"
            : t.Name;
    }
}