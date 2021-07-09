/* file name：${namespace}.EntityExt.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 16:12
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for EntityExt
* revision：
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace lce.provider
{
    /// <summary>
    /// Entity Ext
    /// </summary>
    public static class EntityExt
    {
        /// <summary>
        /// Compare modified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"> </param>
        /// <param name="target"> </param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static Dictionary<string, object> Compare<T>(this T source, T target, IEnumerable<string> exclude = null) where T : class
        {
            var type = target.GetType();
            var keyValues1 = new Dictionary<string, object>();
            var keyValues2 = new Dictionary<string, object>();
            var properties = source.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (null != exclude && exclude.Contains(p.Name)) continue;
                var value1 = p.GetValue(source, null);
                var value2 = type.GetProperty(p.Name)?.GetValue(target, null);
                if (value1 != value2)
                {
                    var attr = p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
                    attr = string.IsNullOrEmpty(attr) ? p.Name : attr;
                    keyValues1.Add(attr, value1);
                    keyValues2.Add(attr, value2);
                }
            }
            return new Dictionary<string, object> {
                { "source", keyValues1 },
                { "target", keyValues2 }
            };
        }

        /// <summary>
        /// Mapping T to R.
        /// </summary>
        /// <typeparam name="T">source type</typeparam>
        /// <typeparam name="R">target type</typeparam>
        /// <param name="source">source</param>
        /// <returns>target</returns>
        public static R Mapping<T, R>(this T source) where R : class where T : class
        {
            if (null == source) return default;
            R result = Activator.CreateInstance<R>();
            return source.Mapping(result);
        }

        /// <summary>
        /// Mappging T to R.
        /// </summary>
        /// <typeparam name="T">source type</typeparam>
        /// <typeparam name="R">target type</typeparam>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static R Mapping<T, R>(this T source, R result) where R : class where T : class
        {
            if (null == source) return result;
            if (null == result) result = Activator.CreateInstance<R>();
            var type = source.GetType();
            var properties = typeof(R).GetProperties();
            foreach (var p in properties)
            {
                var pro = type.GetProperty(p.Name);
                if (null != pro) p.SetValue(result, pro.GetValue(source));
            }
            return result;
        }

        /// <summary>
        /// Mapping List t to List R.
        /// </summary>
        /// <typeparam name="T">source type</typeparam>
        /// <typeparam name="R">target type</typeparam>
        /// <param name="source">source list</param>
        /// <returns>target list</returns>
        public static IEnumerable<R> Mapping<T, R>(this IEnumerable<T> source) where T : class where R : class
        {
            return source.Select(x => x.Mapping<T, R>());
        }
    }
}