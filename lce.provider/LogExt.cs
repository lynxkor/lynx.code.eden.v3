/* file name：lce.ext.providers.Log.cs
* author：lynx lynx.kor@163.com @ 2019/11/21 13:01:47
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for Log
* revision：
*
*/

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace lce.provider
{
    /// <summary>
    /// 把错误写入日志(委托)
    /// </summary>
    /// <param name="message">日志描述 ex.Message会自动加上</param>
    /// <param name="ex">     异常信息</param>
    public delegate void WriteErr(string message, Exception ex);

    /// <summary>
    /// 把信息写入日志(委托)
    /// </summary>
    /// <param name="message">日志描述</param>
    public delegate void WriteInfo(string message);

    /// <summary>
    /// 写入日志(委托)
    /// </summary>
    /// <param name="logType">日志级别</param>
    /// <param name="message">日志描述</param>
    public delegate void WriteLog(LogType logType, string message);

    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
    public enum LogType
    {
        /// <summary>
        /// 信息
        /// </summary>
        INFO,

        /// <summary>
        /// 调试
        /// </summary>
        DEBUG,

        /// <summary>
        /// 异常
        /// </summary>
        ERROR
    }

    /// <summary>
    /// 概述：日志文件处理器
    ///<para>文件：Toolkit.Log.Log</para>
    ///<para>作者：Lynx.kor</para>
    ///<para>创建时间：2016/5/25 10:10:58</para>
    ///<para>描述：appSettings add namevalue</para>
    ///<code><add key = "LogPath" value="C:\Log\" /></code>
    ///<code><add key = "AppName" value="Toolkit"/></code>
    ///<para>> add description for Log 修改历史：</para>
    /// </summary>
    public static class LogExt
    {
        /// <summary>
        /// 把错误写入日志(委托)
        /// </summary>
        public static WriteErr e;

        /// <summary>
        /// 把信息写入日志(委托)
        /// </summary>
        public static WriteInfo i;

        /// <summary>
        /// 写入日志(委托)
        /// </summary>
        public static WriteLog write;

        /// <summary>
        /// 日志器构造
        /// </summary>
        static LogExt()
        {
            var dirpath = ConfigExt.Get("LogPath");
            if (string.IsNullOrEmpty(dirpath))
                dirpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            SoftName = ConfigExt.Get("AppName");
            if (string.IsNullOrEmpty(SoftName))
                SoftName = Process.GetCurrentProcess().MainModule.ModuleName;
            LogDir = Path.Combine(dirpath, SoftName);

            LogSize = ConfigExt.Get("LogSize");
            if (string.IsNullOrEmpty(LogSize)) LogSize = "5";

            try
            {
                if (!System.IO.Directory.Exists(LogDir))
                {
                    System.IO.Directory.CreateDirectory(LogDir);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            write = (LogType logType, string message) =>
            {
                writeLine(logType, message);
            };
            i = (string message) =>
            {
                writeLine(LogType.INFO, message);
            };
            e = (string message, Exception ex) =>
            {
                writeLine(LogType.ERROR, message, ex);
            };
            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs args) => { Dispose(); };
        }

        /// <summary>
        /// 每个日志文件大小(M)
        /// </summary>
        public static string LogSize { get; set; } = "2";

        /// <summary>
        /// 日志目录
        /// </summary>
        private static string LogDir { get; set; }

        /// <summary>
        /// 应用程序名
        /// </summary>
        private static string SoftName { get; set; }

        #region Log 成员

        private static void Dispose()
        {
        }

        /// <summary>
        /// 时间戳/年轮
        /// </summary>
        /// <returns>e.g.13C10T15:55:55:444</returns>
        private static string GetRing()
        {
            var date = DateTime.Now;
            return $"{date.ToString("yy").ToInt32().ToHex()}{date.Month.ToHex()}{date.ToString("ddTHH:mm:ss:fff")}";
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logType">日志级别</param>
        /// <param name="message">日志描述</param>
        /// <param name="ex">     异常信息</param>
        /// <param name="logPath"></param>
        private static void writeLine(LogType logType, string message, Exception ex = null, string logPath = null)
        {
            var logStr = string.Empty;
            var sb = new StringBuilder();
            // 补充ex.Message
            if (ex != null) message += " =>" + ex.Message;
            sb.Append($"on:{GetRing()}:[{logType}]desc:{message}\r\n");
            if (LogType.ERROR == logType)
            {
                var format = "line:{0};cols:{1};in:{2}\r\n";
                var st = ex != null ? new StackTrace(ex, true) : new StackTrace(true);
                foreach (var frame in st.GetFrames())
                {
                    var method = frame.GetMethod();
                    if (method == null) continue;
                    if (method.ReflectedType == typeof(LogExt)) continue;
                    string name = method.DeclaringType != null ? method.DeclaringType.FullName : string.Empty;
                    if (name.StartsWith("System.")) continue;

                    string fileName = null;
                    try
                    {
                        fileName = frame.GetFileName();
                    }
                    catch (NotSupportedException)
                    {
                        continue;
                    }
                    catch (SecurityException)
                    {
                        continue;
                    }

                    sb.Append($"@ {(method.DeclaringType != null ? method.ReflectedType.FullName.Replace('+', '.') : string.Empty)}:{ method.Name}\r\n");
                    var line = frame.GetFileLineNumber();
                    if (line > 0 && !string.IsNullOrEmpty(fileName))
                    {
                        var cols = frame.GetFileColumnNumber();
                        sb.AppendFormat(CultureInfo.InvariantCulture, format, new object[] { line, cols, fileName });
                    }
                }
                if (null != ex.InnerException)
                {
                    sb.Append($"[InnerException]desc:{ex.InnerException.Message}\r\n");
                    var ist = new StackTrace(ex.InnerException, true);
                    foreach (var frame in ist.GetFrames())
                    {
                        var method = frame.GetMethod();
                        if (method == null) continue;
                        if (method.ReflectedType == typeof(LogExt)) continue;
                        string name = method.DeclaringType != null ? method.DeclaringType.FullName : string.Empty;
                        if (name.StartsWith("System.")) continue;

                        string fileName = null;
                        try
                        {
                            fileName = frame.GetFileName();
                        }
                        catch (NotSupportedException)
                        {
                            continue;
                        }
                        catch (SecurityException)
                        {
                            continue;
                        }

                        sb.Append($"@ {(method.DeclaringType != null ? method.ReflectedType.FullName.Replace('+', '.') : string.Empty)}:{ method.Name}\r\n");
                        var line = frame.GetFileLineNumber();
                        if (line > 0 && !string.IsNullOrEmpty(fileName))
                        {
                            var cols = frame.GetFileColumnNumber();
                            sb.AppendFormat(CultureInfo.InvariantCulture, format, new object[] { line, cols, fileName });
                        }
                    }
                }
            }
            sb.Append("\r\n");
            logStr = sb.ToString();

            Task.Run(() =>
            {
                try
                {
                    var logFile = System.IO.Path.Combine(LogDir, $"{DateTime.Now.ToString("yyyyMMdd")}.log");
                    using (StreamWriter sw = File.AppendText(logFile))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine(logStr);
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    try
                    {
                        if (File.Exists(logFile))
                        {
                            var size = new FileInfo(logFile).Length;
                            size = (long)(size / 1024.00 / 1024.00);
                            if (size >= LogSize.ToInt32())
                            {
                                var destFile = System.IO.Path.Combine(LogDir, $"{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.log");
                                File.Move(logFile, destFile);
                            }
                        }
                    }
                    catch { }
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.ToString());
                }

                if (logType == LogType.ERROR)
                {
                    try
                    {
                        //确定smtp服务器地址。实例化一个Smtp客户端
                        var client = new SmtpClient("smtp.163.com");
                        //构造一个发件人地址对象
                        var from = new MailAddress("lynxce@163.com", "lynxce.com", Encoding.UTF8);
                        //构造一个收件人地址对象
                        var to = new MailAddress("lynx.kor@163.com", "lynx.kor", Encoding.UTF8);
                        var leap = new MailAddress("ke_linjun@leapmotor.com", "山猫", Encoding.UTF8);
                        //构造一个Email的Message对象
                        var mailMessage = new MailMessage(from, to)
                        {
                            //添加邮件主题和内容
                            Subject = string.Format("[Err]{0}:{1}", SoftName, Process.GetCurrentProcess().MainModule.FileName),
                            SubjectEncoding = Encoding.UTF8,
                            BodyEncoding = Encoding.UTF8,
                            IsBodyHtml = false,
                            Body = logStr,
                        };
                        mailMessage.To.Add(leap);
                        //设置用户名和密码，用户登陆信息
                        client.Credentials = new NetworkCredential("lynxce", "134567890");
                        //设置邮件的信息
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //发送邮件
                        client.Send(mailMessage);
                    }
                    catch { }
                }
            });
#if DEBUG
            if (logType == LogType.ERROR)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(logStr);
            if (logType == LogType.ERROR)
                Console.ResetColor();
#endif
        }

        #endregion Log 成员
    }
}