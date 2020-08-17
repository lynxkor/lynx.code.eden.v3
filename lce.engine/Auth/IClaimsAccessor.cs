/* file name：${namespace}.IClaimsAccessor.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/15 11:18
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for IClaimsAccessor
* revision：
*
*/

namespace lce.provider.Auth
{
    /// <summary>
    /// </summary>
    public interface IClaimsAccessor
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        IUser CurrentUser { get; }
    }
}