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

    /// <summary>
    /// 实体字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class EntityColumnAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">        字段名</param>
        /// <param name="dataType">    数据类型</param>
        /// <param name="isAlias">     是否别名</param>
        /// <param name="isOptionName"></param>
        public EntityColumnAttribute(string name, EntityDataType dataType = EntityDataType.String, bool isAlias = false, bool isOptionName = false)
        {
            Name = name;
            DataType = dataType;
            IsAlias = isAlias;
            IsOptionName = isOptionName;
        }

        /// <summary>
        /// 构造函数 LookUp
        /// </summary>
        /// <param name="name">        字段名</param>
        /// <param name="lookUp">      关联对象</param>
        /// <param name="isLookUpName">是否关联对象的名称</param>
        /// <param name="isAlias">     是否别名</param>
        public EntityColumnAttribute(string name, string lookUp, bool isLookUpName = false, bool isAlias = false)
        {
            DataType = EntityDataType.EntityReference;
            Name = name;
            LookUp = lookUp;
            IsLookUpName = isLookUpName;
            IsAlias = isAlias;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public EntityDataType DataType { get; }

        /// <summary>
        /// 是否关联实体字段
        /// </summary>
        public bool IsAlias { get; set; } = false;

        /// <summary>
        /// 是否关联对象的名称，仅LookUp不为空时用于逻辑判定，用到Entity序列化至Model使用
        /// </summary>
        public bool IsLookUpName { get; set; } = false;

        /// <summary>
        /// 是否选项集名称，仅DataType为OptionSetValue时用于逻辑判定
        /// </summary>
        public bool IsOptionName { get; set; } = false;

        /// <summary>
        /// 关联对象；实体
        /// </summary>
        public string LookUp { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
    }
}