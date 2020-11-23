/* file name：MSCRM.CRM.Plugins.VS.Engine.BasePlugin.cs
* author：lynx lynx.kor@163.com @ 2018/10/28 13:13:00
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for BasePlugin
* revision：
*
*/

using System;
using System.ServiceModel;
using lce.provider;
using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：BasePlugin
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        private IOrganizationService _caller;
        private IPluginExecutionContext _context;
        private IOrganizationServiceFactory _factory;
        private IOrganizationService _service;
        private IServiceProvider _serviceProvider;
        private ITracingService _tracing;

        /// <summary>
        /// 用户权限服务
        /// </summary>
        public IOrganizationService Caller
        {
            get
            {
                if (null == _caller)
                {
                    _caller = Factory.CreateOrganizationService(this.UserId);
                }
                return _caller;
            }
        }

        /// <summary>
        /// 上下文
        /// </summary>
        public IPluginExecutionContext Context
        {
            get
            {
                if (null == _context)
                {
                    _context = GetService<IPluginExecutionContext>(ServiceProvider);
                }
                return _context;
            }
        }

        /// <summary>
        /// 服务工厂
        /// </summary>
        public IOrganizationServiceFactory Factory
        {
            get
            {
                if (null == _factory)
                {
                    _factory = GetService<IOrganizationServiceFactory>(ServiceProvider);
                }
                return _factory;
            }
        }

        /// <summary>
        /// 修改后
        /// </summary>
        public Entity PostImage { get; set; }

        /// <summary>
        /// 修改前
        /// </summary>
        public Entity PreImage { get; set; }

        /// <summary>
        /// 管理员权限服务
        /// </summary>
        public IOrganizationService Service
        {
            get
            {
                if (null == _service)
                {
                    _service = Factory.CreateOrganizationService(null);
                }
                return _service;
            }
        }

        /// <summary>
        /// </summary>
        public IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        /// <summary>
        /// 当前实例
        /// </summary>
        public Entity Target { get; set; }

        /// <summary>
        /// 跟踪服务
        /// </summary>
        public ITracingService Tracing
        {
            get
            {
                if (null == _tracing)
                {
                    _tracing = GetService<ITracingService>(ServiceProvider);
                }
                return _tracing;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid UserId { get { return this.Context.UserId; } }

        /// <summary>
        /// check the attribute in entity is null.
        /// </summary>
        /// <param name="entity">   </param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool IsNotNull(Entity entity, string attribute)
        {
            return null != entity && entity.Contains(attribute) && IsNotNull(entity[attribute]);
        }

        /// <summary>
        /// check the object is not null.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(object obj)
        {
            return null != obj && DBNull.Value != obj && !string.IsNullOrEmpty(obj.ToString());
        }

        /// <summary>
        /// 插件回滚，弹出消息。
        /// </summary>
        /// <param name="msg">消息类容</param>
        public static void Message(string msg)
        {
            throw new InvalidPluginExecutionException(msg);
        }

        /// <summary>
        /// 实体状态比较
        /// </summary>
        /// <param name="state"> pre:20;post:40</param>
        /// <param name="action">create,update,delete</param>
        /// <returns></returns>
        public bool EqualActionOrMessage(int state, string action)
        {
            Tracing.Trace("Equal Action Message");
            return Context.Stage == state && !string.IsNullOrEmpty(action) && Context.MessageName.ToLower().Equals(action.ToLower());
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            try
            {
                if (Context.InputParameters.Contains("Target") && Context.InputParameters["Target"] is Entity)
                    Target = (Entity)Context.InputParameters["Target"];
                if (Context.PreEntityImages.Contains("PreImage") && Context.PreEntityImages["PreImage"] is Entity)
                    PreImage = (Entity)Context.PreEntityImages["PreImage"];
                if (Context.PreEntityImages.Contains("PostImage") && Context.PreEntityImages["PostImage"] is Entity)
                    PreImage = (Entity)Context.PreEntityImages["PostImage"];
                // do same logic
                HandleExecute();
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ex;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException($"插件业务错误：{ex.Message}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Tracing.Trace($"系统内部错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                Message($"系统内部错误：{ex.Message}");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// 插件继承BasePlugin后实现些方法，进行业务处理
        /// </summary>
        public abstract void HandleExecute();

        private T GetService<T>(IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}