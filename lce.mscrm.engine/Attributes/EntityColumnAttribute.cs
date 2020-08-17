/* file name：lce.mscrm.engine.Attributes.EntityColumnAttribute.cs
* author：lynx lynx.kor@163.com @ 2020/03/16 11:25:33
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for EntityColumnAttribute
* revision：
*
*/

using System;

namespace lce.mscrm.engine.Attributes
{
    /// <summary>
    /// 实体字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class EntityColumnAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public EntityDataType DataType { get; }

        /// <summary>
        /// 关联对象；实体/选项集
        /// </summary>
        public string LookUp { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">    字段名</param>
        /// <param name="dataType">数据类型</param>
        public EntityColumnAttribute(string name, EntityDataType dataType = EntityDataType.String)
        {
            Name = name;
            DataType = dataType;
        }

        /// <summary>
        /// 构造函数 LookUp
        /// </summary>
        /// <param name="name">  字段名</param>
        /// <param name="lookUp">关联对象</param>
        public EntityColumnAttribute(string name, string lookUp)
        {
            DataType = EntityDataType.EntityReference;
            Name = name;
            LookUp = lookUp;
        }
    }

    /// <summary>
    /// Entity数据类型
    /// </summary>
    public enum EntityDataType
    {
        /// <summary>
        /// GUID
        /// </summary>
        Guid,

        /// <summary>
        /// 整数
        /// </summary>
        Int,

        /// <summary>
        /// 文本/多行文本
        /// </summary>
        String,

        /// <summary>
        /// 十进制数
        /// </summary>
        Decimal,

        /// <summary>
        /// 浮点数
        /// </summary>
        Double,

        /// <summary>
        /// 货币
        /// </summary>
        Money,

        /// <summary>
        /// 两个选项
        /// </summary>
        Bool,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 选项集
        /// </summary>
        OptionSetValue,

        /// <summary>
        /// LookUp
        /// </summary>
        EntityReference
    }
}