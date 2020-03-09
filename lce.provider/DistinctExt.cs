/* file name：lce.provider.DistinctExt.cs
 * author：lynx lynx.kor@163.com @ 2019/6/5 23:02
 * copyright (c) 2019 lynxce.com
 * desc：
 * > add description for DistinctExt
 * revision：
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace lce.provider
{
    /// <summary>
    /// DistinctExt
    /// </summary>
    public static class DistinctExt
    {
        /// <summary>
        /// Distinct
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source">     </param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
    }

    /// <summary>
    /// CommonEqualityComparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private readonly Func<T, V> keySelector;

        /// <summary>
        /// </summary>
        /// <param name="keySelector"></param>
        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}