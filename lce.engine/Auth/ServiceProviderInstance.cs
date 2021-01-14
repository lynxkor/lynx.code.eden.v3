/* file name：lce.engine.Auth.ServiceProviderInstance.cs
* author：lynx lynx.kor@163.com @ 2021/1/12 8:24:37
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ServiceProviderInstance
* revision：
*
*/

using System;

namespace lce.engine.Auth
{
    /// <summary>
    /// action：ServiceProviderInstance
    /// </summary>
    public static class ServiceProviderInstance
    {
        /// <summary>
        /// </summary>
        public static IServiceProvider Instance { get; set; }
    }
}