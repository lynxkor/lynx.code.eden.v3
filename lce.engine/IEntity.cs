// action：
// file name： IEntity.cs
// author：lynx <lynx.kor@163.com> @ 2019/6/5 11:09
// copyright (c) 2019 lynxce.com
// desc：
// > add description for IEntity
// revision：
// //
using System;
namespace lce.engine
{
    public class IEntity
    {
        /// <summary>
        /// 主键/ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 标题/名称/姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 状态：0:正常；1:禁用
        /// </summary>
        public int State { get; set; } = 0;

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改人
        /// </summary>
        public int ModifiedBy { get; set; } = 0;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
