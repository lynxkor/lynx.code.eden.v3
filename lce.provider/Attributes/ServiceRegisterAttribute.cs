// /* file name：${namespace}.ServiceProviderAttribute.cs
// * author：lynx <lynx.kor@163.com> @ 2020/3/14 20:23
// * copyright (c) 2020 Copyright@lynxce.com
// * desc：
// * > add description for ServiceProviderAttribute
// * revision：
// *
// */
using System;
using Microsoft.Extensions.DependencyInjection;

namespace lce.provider.Attributes
{
    /// <summary>
    /// 用于标记服务，便于注入注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceProviderAttribute : Attribute
    {
        /// <summary>
        /// 生命周期，默认为Transient
        /// </summary>
        public ServiceLifetime Lifetime { get; set; }

        /// <summary>
        /// 服务类型，用于映射
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="lifetime">生命周期</param>
        public ServiceProviderAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            Type = type;
            Lifetime = lifetime;
        }
    }
}
