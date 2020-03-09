/* file name：lce.provider.EnumExt.cs
 * author：lynx lynx.kor@163.com @ 2019/6/5 23:03
 * copyright (c) 2019 lynxce.com
 * desc：
 * > add description for EnumExt
 * revision：
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace lce.provider
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExt
    {
        /// <summary>
        /// 获取枚举项描述信息
        /// </summary>
        /// <param name="eValue">枚举项</param>
        /// <returns>枚举项描述信息</returns>
        public static string Description(this Enum eValue)
        {
            var typeInfo = eValue.GetType().GetTypeInfo();
            var memberInfo = typeInfo.GetMember(eValue.ToString()).First();
            var descriptionAttribute =
              memberInfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute.Description;
        }

        /// <summary>
        /// 获取枚举类型的数据字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> List<T>()
        {
            var type = typeof(T);
            var list = new Dictionary<string, int>();

            foreach (var e in Enum.GetValues(type))
            {
                var desc = type.GetMember(e.ToString()).First().GetCustomAttribute<DescriptionAttribute>().Description;
                list.Add(desc, Convert.ToInt32(e));
            }
            return list;
        }
    }
}