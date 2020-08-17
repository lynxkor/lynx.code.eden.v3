/* file name：lce.mscrm.engine.Attributes.EntityNameAttribute.cs
* author：lynx lynx.kor@163.com @ 2020/03/16 11:22:41
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for EntityNameAttribute
* revision：
*
*/

using System;

namespace lce.mscrm.engine.Attributes
{
    /// <summary>
    /// 实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class EntityNameAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">  entity name</param>
        /// <param name="prefix">prefix</param>
        public EntityNameAttribute(string name, string prefix = "")
        {
            Name = name;
            Prefix = prefix;
        }
    }
}