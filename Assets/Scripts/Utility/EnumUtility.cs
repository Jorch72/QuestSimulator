
using System;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class EnumUtility {

        public static T GetRandomEnumValue<T>(int startIndex = 0) {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(UnityEngine.Random.Range(startIndex, v.Length));
        }

    }

}