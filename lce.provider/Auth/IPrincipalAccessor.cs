/* file name：${namespace}.IPrincipalAccessor.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/15 10:42
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for IPrincipalAccessor
* revision：
*
*/
using System.Security.Claims;

namespace lce.provider.Auth
{
    /// <summary>
    /// 身份注入器
    /// </summary>
    public interface IPrincipalAccessor
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        ClaimsPrincipal Principal { get; }
    }

}
