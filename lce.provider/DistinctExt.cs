// action：
// file name：${namespace}.DistinctExt.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 23:02
// copyright (c) 2019 lynxce.com
// desc：
// > add description for DistinctExt
// revision：
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace lce.provider
{
    public static class DistinctExt
    {
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
    }

    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        readonly Func<T, V> keySelector;

        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}
