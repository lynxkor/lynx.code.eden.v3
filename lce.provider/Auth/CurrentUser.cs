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
        /// 
        /// </summary>
        public CurrentUser()
        {
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public int OrganId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrganName { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public IList<string> RoleIds { get; set; }
    }
}
