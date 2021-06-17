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
        /// <summary>
        /// 工作流回滚，弹出消息。
        /// </summary>
        /// <param name="msg">消息类容</param>
        public static void ShowErrorMessage(string msg)
        {
            throw new InvalidPluginExecutionException(msg);
        }

        /// <summary>
        /// 工作流继承 BaseActivity 后实现些方法，进行业务处理
        /// </summary>
        public abstract void HandleExecute(IOrganizationService service, IOrganizationService caller, ITracingService tracing, CodeActivityContext context, IWorkflowContext wfContext);

        /// <summary>
        /// </summary>
        /// <param name="context">上下文</param>
        protected override void Execute(CodeActivityContext context)
        {
            var tracing = context.GetExtension<ITracingService>();                  //跟踪服务
            try
            {
                var wfContext = context.GetExtension<IWorkflowContext>();           //工作流上下文
                var factory = context.GetExtension<IOrganizationServiceFactory>();  //服务工厂
                var caller = factory.CreateOrganizationService(wfContext.UserId);   //用户权限服务
                var service = factory.CreateOrganizationService(null);              //管理员权限服务

                //UserId = wfContext.UserId;
                HandleExecute(service, caller, tracing, context, wfContext);
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ex;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                tracing.Trace($"业务错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                ShowErrorMessage($"业务错误：{ex.Message}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                tracing.Trace($"系统内部错误：{ex.Message}", ex.StackTrace);
                LogExt.e(ex.Message, ex);
                ShowErrorMessage($"系统内部错误：{ex.Message}");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}