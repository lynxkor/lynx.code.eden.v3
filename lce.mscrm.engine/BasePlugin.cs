/* file name：lce.mscrm.engine.BasePlugin.cs
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
    #region === MessageName ===

    /// <summary>
    /// Plugin MessageName
    /// </summary>
    public enum MessageName
    {
        /// <summary>
        /// Assign
        /// <para>Changes ownership of a record. Valid for user-owned or team-owned entities.</para>
        /// </summary>
        Assign,

        /// <summary>
        /// Creates links between a record and a collection of records where there is a relationship
        /// between the entities.
        /// </summary>
        Associate,

        /// <summary>
        /// Create Target
        /// <para>Creates a record of a specific entity type, including custom entities.</para>
        /// </summary>
        Create,

        /// <summary>
        /// Delete Target
        /// <para>Deletes a record.</para>
        /// </summary>
        Delete,

        /// <summary>
        /// Removes links between a record and a collection of records where there is a relationship
        /// between the entities.
        /// </summary>
        Disassociate,

        /// <summary>
        /// </summary>
        GrantAccess,

        /// <summary>
        /// </summary>
        ModifyAccess,

        /// <summary>
        /// Retrieves a record.
        /// </summary>
        Retrieve,

        /// <summary>
        /// Retrieves a collection of records.
        /// </summary>
        RetrieveMultiple,

        /// <summary>
        /// </summary>
        RetrievePrincipalAccess,

        /// <summary>
        /// </summary>
        RetrieveSharedPrincipalsAndAccess,

        /// <summary>
        /// </summary>
        RevokeAccess,

        /// <summary>
        /// Set the state of a record.
        /// </summary>
        SetState,

        /// <summary>
        /// Grants, modifies or revokes access to a record to another user or team. Valid for
        /// user-owned or team-owned entities.
        /// </summary>
        Share,

        /// <summary>
        /// Update Target
        /// <para>Modifies the contents of a record.</para>
        /// </summary>
        Update,
    }

    #endregion === MessageName ===

    /// <summary>
    /// action：BasePlugin
    /// <para>UserId = _context.UserId</para>
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        #region === 私有变量/方法 ===

        private IPluginExecutionContext _context;

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private T GetService<T>(IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

        #endregion === 私有变量/方法 ===

        /// <summary>
        /// 修改后
        /// </summary>
        protected Entity PostImage { get; set; }

        /// <summary>
        /// 修改前
        /// </summary>
        protected Entity PreImage { get; set; }

        /// <summary>
        /// 当前实例
        /// </summary>
        protected Entity Target { get; set; }

        /// <summary>
        /// 当前实例
        /// </summary>
        protected EntityReference TargetReference { get; set; }

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
        public static void ShowErrorMessage(string msg)
        {
            throw new InvalidPluginExecutionException(msg);
        }

        /// <summary>
        /// 实体状态比较
        /// </summary>
        /// <param name="state"> pre:20;post:40</param>
        /// <param name="action">create,update,delete</param>
        /// <returns></returns>
        public bool EqualActionOrMessage(int state, MessageName action)
        {
            return _context.Stage == state && _context.MessageName.ToLower().Equals(action.ToString().ToLower());
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            _context = GetService<IPluginExecutionContext>(serviceProvider);            //上下文
            var _tracing = GetService<ITracingService>(serviceProvider);                //跟踪服务
            var _factory = GetService<IOrganizationServiceFactory>(serviceProvider);    //服务工厂
            var _caller = _factory.CreateOrganizationService(_context.UserId);          //用户权限服务
            var _service = _factory.CreateOrganizationService(null);                    //管理员权限服务

            try
            {
                if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is EntityReference reference)
                {
                    TargetReference = reference;
                }

                if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity entity)
                {
                    Target = entity;
                    TargetReference = Target.ToEntityReference();
                }

                if (_context.PreEntityImages.Contains("PreImage") && _context.PreEntityImages["PreImage"] is Entity)
                    PreImage = _context.PreEntityImages["PreImage"];

                if (_context.PostEntityImages.Contains("PostImage") && _context.PostEntityImages["PostImage"] is Entity)
                    PostImage = _context.PostEntityImages["PostImage"];

                // do same logic
                HandleExecute(_service, _caller, _tracing, _context);
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ex;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                _tracing.Trace($"插件业务错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                throw new InvalidPluginExecutionException($"插件业务错误：{ex.Message}");
            }
            catch (Exception ex)
            {
                _tracing.Trace($"系统内部错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                ShowErrorMessage($"系统内部错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 插件继承BasePlugin后实现些方法，进行业务处理
        /// </summary>
        public abstract void HandleExecute(IOrganizationService service, IOrganizationService caller, ITracingService tracing, IPluginExecutionContext context);
    }
}