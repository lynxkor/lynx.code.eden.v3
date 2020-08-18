/* file name：${namespace}.PrincipalAccessor.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/15 10:44
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for PrincipalAccessor
* revision：
*
*/

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace lce.provider.Auth
{
    /// <summary>
    /// </summary>
    public class PrincipalAccessor : IPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public PrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// </summary>
        public ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User;
    }
}