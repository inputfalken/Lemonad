﻿using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class MapTests {
        [Fact]
        public void Mapping_Integer_With_Multiplication() {
            var maybe = 20.ToMaybe().Map(s => s * 2);

            Assert.True(maybe.HasValue,
                "Should have value since value is not null and no failed predicates has been used.");
            Assert.Equal(40, maybe.Value);
        }

        [Fact]
        public void Mapping_String_Length() {
            var maybe = "hello".ToMaybe().Map(s => s.Length);

            Assert.True(maybe.HasValue,
                "Should have value since value is not null and no failed predicates has been used.");
            Assert.Equal(5, maybe.Value);
        }


        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> function = null;
                "foo".ToMaybe().Map(function);
            });
        }
    }
}