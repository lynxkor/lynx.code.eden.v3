/* file name：MSCRM.CRM.Plugins.VS.Engine.BasePlugin.cs
* author：lynx lynx.kor@163.com @ 2018/10/28 13:13:00
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for BasePlugin
* revision：
*
*/

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：BasePlugin
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        /// 用户权限服务
        /// </summary>
        public IOrganizationService Caller { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public IPluginExecutionContext Context { get; set; }

        /// <summary>
        /// 服务工厂
        /// </summary>
        public IOrganizationServiceFactory Factory { get; set; }

        /// <summary>
        /// 管理员权限服务
        /// </summary>
        public IOrganizationService Service { get; set; }

        /// <summary>
        /// 当前实例
        /// </summary>
        public Entity Target { get; set; }

        /// <summary>
        /// 跟踪服务
        /// </summary>
        public ITracingService Tracing { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 插件回滚，弹出消息。
        /// </summary>
        /// <param name="msg">消息类容</param>
        public static void Message(string msg)
        {
            throw new InvalidPluginExecutionException(msg);
        }

        /// <summary>
        /// 写文件日志
        /// </summary>
        /// <param name="message"> </param>
        /// <param name="filepath"></param>
        public static void WriteLog(string message, string filepath = "")
        {
            Task.Run(() =>
            {
                if (string.IsNullOrEmpty(filepath)) filepath = @"c:\mscrm.log\plugin";
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                var logFile = Path.Combine(filepath, $"{DateTime.Now.ToString("yyyyMMdd")}.log");
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine($"on:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}:{message}\r\n");
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            });
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
            this.Tracing = GetService<ITracingService>(serviceProvider);
            this.Context = GetService<IPluginExecutionContext>(serviceProvider);
            this.UserId = this.Context.UserId;
            this.Factory = GetService<IOrganizationServiceFactory>(serviceProvider);
            this.Service = Factory.CreateOrganizationService(null);
            this.Caller = Factory.CreateOrganizationService(this.UserId);

            try
            {
                if (Context.InputParameters.Contains("Target") && Context.InputParameters["Target"] is Entity)
                {
                    Target = (Entity)Context.InputParameters["Target"];
                    HandleExecute();
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Tracing.Trace($"系统内部错误：{ex.Message}", ex.StackTrace);
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