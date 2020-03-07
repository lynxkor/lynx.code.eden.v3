// action：
// file name：${namespace}.JsonExt.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 23:06
// copyright (c) 2019 lynxce.com
// desc：
// > add description for JsonExt
// revision：
//
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace lce.provider
{
    /// <summary>
    /// JsonExt
    /// </summary>
    public static class JsonExt
    {
        /// <summary>
        /// object to json string.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="indented">If set to <c>true</c> indented.</param>
        /// <param name="includeNull">If set to <c>true</c> include null.</param>
        public static string ToJson(this object obj, bool indented = false, bool includeNull = false)
        {
            if (null == obj) return null;
            return JsonConvert.SerializeObject(obj,
                indented ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
                });
        }

        /// <summary>
        /// Json string to model.
        /// </summary>
        /// <returns>The model.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ToModel<T>(this string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch
            {
                return default;
            }
        }
    }
}
