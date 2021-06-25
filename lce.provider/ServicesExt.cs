/* file name：lce.provider.ServicesExt.cs
* author：lynx lynx.kor@163.com @ 2021/6/25 8:30:49
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ServicesExt
* revision：
*
*/

using System.Reflection;
using lce.provider.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace lce.provider
{
    /// <summary>
    /// action：ServicesExt
    /// </summary>
    public static class ServicesExt
    {
        /// <summary>
        /// 服务注册器
        /// <para>服务需要有 [ServiceProvider] 标签</para>
        /// </summary>
        /// <param name="services">     </param>
        /// <param name="rootnamespace">服务所在命名空间</param>
        public static void RegisterServices(this IServiceCollection services, string rootnamespace)
        {
            var assembly = Assembly.Load(rootnamespace);
            if (null == assembly) return;
            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<ServiceProviderAttribute>();
                if (null != attribute)
                {
                    switch (attribute.Lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(attribute.Type, type);
                            break;

                        case ServiceLifetime.Scoped:
                            services.AddScoped(attribute.Type, type);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(attribute.Type, type);
                            break;
                    }
                }
            }
        }
    }
}