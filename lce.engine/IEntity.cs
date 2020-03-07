// action：
// file name： IEntity.cs
// author：lynx <lynx.kor@163.com> @ 2019/6/5 11:09
// copyright (c) 2019 lynxce.com
// desc：
// > add description for IEntity
// revision：
// //
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lce.engine
{
    public class IEntity
    {
        /// <summary>
        /// 主键/ID
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 代码/编码/编号
        /// </summary>
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 标题/名称/姓名
        /// </summary>
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 备注/说明
        /// </summary>
        [Column("memo")]
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 状态：0:正常；1:禁用
        /// </summary>
        [Column("state")]
        public int State { get; set; } = 0;

        /// <summary>
        /// 序号；用于排序'
        /// </summary>
        [Column("series")]
        public int Series { get; set; } = 0;

        /// <summary>
        /// 所属人
        /// </summary>
        [Column("ownerid")]
        public int OwnerId { get; set; } = 0;

        /// <summary>
        /// 所属部门
        /// </summary>
        [Column("ownerorganid")]
        public int OwnerOrganId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("createdby")]
        public int CreatedBy { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("modifiedby")]
        public int ModifiedBy { get; set; } = 0;

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("modifiedon")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
