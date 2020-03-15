/* file name：${namespace}.IUser.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 13:14
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for IUser
* revision：
*
*/
using System.Collections.Generic;

namespace lce.provider.Auth
{
    /// <summary>
    /// 系统用户基类
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 组织Id
        /// </summary>
        int OrganId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        string OrganName { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        int CompId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        string CompName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        IList<string> RoleIds { get; set; }

    }
}
