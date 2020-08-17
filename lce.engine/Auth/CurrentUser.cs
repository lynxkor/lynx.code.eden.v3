/* file name：${namespace}.CurrentUser.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 20:08
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for CurrentUser
* revision：
*
*/

using System.Collections.Generic;

namespace lce.provider.Auth
{
    /// <summary>
    /// CurrentUser
    /// </summary>
    public class CurrentUser : IUser
    {
        /// <summary>
        /// </summary>
        public CurrentUser()
        {
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = "SYS";

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "SYS";

        /// <summary>
        /// 组织Id
        /// </summary>
        public int OrganId { get; set; } = 0;

        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrganName { get; set; } = "SYS";

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompId { get; set; } = 0;

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompName { get; set; } = "SYS";

        /// <summary>
        /// 角色
        /// </summary>
        public IList<string> RoleIds { get; set; } = new string[] { "ALL" };
    }
}