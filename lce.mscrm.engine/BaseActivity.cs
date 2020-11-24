/* file name：lce.mscrm.engine.BaseActivity.cs
* author：lynx lynx.kor@163.com @ 2018/10/28 13:13:00
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for BaseActivity
* revision：
*
*/

using System;
using System.Activities;
using System.ServiceModel;
using lce.provider;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace lce.mscrm.engine
{
    public abstract class BaseActivity : CodeActivity
    {
        #region === 工作流 属性访问器 ===

        /// <summary>
        /// 用户权限服务
        /// </summary>
        public IOrganizationService Caller { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CodeActivityContext Context { get; set; }

        /// <summary>
        /// 服务工厂
        /// </summary>
        public IOrganizationServiceFactory Factory { get; set; }

        /// <summary>
        /// 管理员权限服务
        /// </summary>
        public IOrganizationService Service { get; set; }

        /// <summary>
        /// 跟踪服务
        /// </summary>
        public ITracingService Tracing { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 工作流上下文
        /// </summary>
        public IWorkflowContext WorkflowContext { get; set; }

        #endregion === 工作流 属性访问器 ===

        /// <summary>
        /// 工作流回滚，弹出消息。
        /// </summary>
        /// <param name="msg">消息类容</param>
        public static void ShowMessage(string msg)
        {
            throw new InvalidPluginExecutionException(msg);
        }

        /// <summary>
        /// 工作流继承 BaseActivity 后实现些方法，进行业务处理
        /// </summary>
        public abstract void HandleExecute();

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                Context = context;
                Tracing = Context.GetExtension<ITracingService>();
                WorkflowContext = Context.GetExtension<IWorkflowContext>();
                UserId = WorkflowContext.UserId;
                Factory = Context.GetExtension<IOrganizationServiceFactory>();
                Service = Factory.CreateOrganizationService(null);
                Caller = Factory.CreateOrganizationService(this.UserId);

                HandleExecute();
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ex;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException($"业务错误：{ex.Message}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Tracing.Trace($"系统内部错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                ShowMessage($"系统内部错误：{ex.Message}");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}