// action：
// file name：${namespace}.EnumExt.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 23:03
// copyright (c) 2019 lynxce.com
// desc：
// > add description for EnumExt
// revision：
//
using System;
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
            MemberInfo memberInfo = typeInfo.GetMember(eValue.ToString()).First();
            var descriptionAttribute =
              memberInfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute.Description;
        }
    }
}
