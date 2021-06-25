/* file name：lce.provider.Attributes.ServiceAutowiredAttribute.cs
* author：lynx lynx.kor@163.com @ 2021/1/11 10:20:00
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ServiceAutowiredAttribute
* revision：
*
*/

using System;
using System.Linq;
using System.Reflection;

namespace lce.provider.Attributes
{
    /// <summary>
    /// 服务自动装载
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ServiceAutowiredAttribute : Attribute { }

    /// <summary>
    /// 服务自动装载
    /// </summary>
    public class ServiceAutowiredProvider
    {
        /// <summary>
        /// 服务自动装载
        /// <para>变量需要有 [ServiceAutowired] 标签</para>
        /// <para>变量需要以“_”开头命名</para>
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="provider"></param>
        public void PropertyActivate(object service, IServiceProvider provider)
        {
            var serviceType = service.GetType();
            //属性赋值
            var properties = serviceType.GetProperties().AsEnumerable().Where(x => x.Name.StartsWith("_"));
            foreach (PropertyInfo property in properties)
            {
                var autowiredAttr = property.GetCustomAttribute<ServiceAutowiredAttribute>();
                if (autowiredAttr != null)
                {
                    var innerService = provider.GetService(property.PropertyType);
                    PropertyActivate(innerService, provider);
                    property.SetValue(service, innerService);
                }
            }
            return;
        }
    }
}