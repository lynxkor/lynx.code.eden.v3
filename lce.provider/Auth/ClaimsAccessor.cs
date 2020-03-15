/* file name：${namespace}.ClaimsAccessor.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/15 11:19
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for ClaimsAccessor
* revision：
*
*/
using System;
using System.Linq;
using System.Security.Claims;

namespace lce.provider.Auth
{
    /// <summary>
    /// </summary>
    public class ClaimsAccessor : IClaimsAccessor
    {
        /// <summary>
        /// </summary>
        protected IPrincipalAccessor _principalAccessor { get; }

        /// <summary>
        /// </summary>
        public ClaimsAccessor(IPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
            var userJson = principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;

            if (!string.IsNullOrEmpty(userJson))
            {
                CurrentUser = userJson.ToModel<CurrentUser>();
            }
        }

        /// <summary>
        /// </summary>
        public IUser CurrentUser { get; }
    }
}
