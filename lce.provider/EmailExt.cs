/* file name：lce.provider.EmailExt.cs
* author：lynx lynx.kor@163.com @ 2019/6/5 23:15
* copyright (c) 2019 lynxce.com
* desc：
* > add description for EmailExt
* revision：
*
*/

using System;

namespace lce.provider
{
    /// <summary>
    /// EmailExt
    /// </summary>
    public static class EmailExt
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="content"> </param>
        /// <param name="subject"> </param>
        /// <param name="mailto">  </param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mailfrom"></param>
        /// <param name="smtp">    </param>
        /// <param name="port">    </param>
        public static void Send(
            string content,
            string subject,
            string mailto = "lynx.kor@163.com",
            string username = "lynxce",
            string password = "134567890",
            string mailfrom = "lynxce@163.com",
            string smtp = "smtp.163.com",
            int port = 25)
        {
            try
            {
                //设置用户名和密码。
                //用户登陆信息
                var myCredentials = new System.Net.NetworkCredential(username, password);
                //确定smtp服务器地址。实例化一个Smtp客户端
                var client = new System.Net.Mail.SmtpClient(smtp, port)
                {
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = myCredentials
                };
                //构造一个发件人地址对象
                var mfrom = new System.Net.Mail.MailAddress(mailfrom, mailfrom, System.Text.Encoding.UTF8);
                //构造一个收件人地址对象
                var mto = new System.Net.Mail.MailAddress(mailto, mailto, System.Text.Encoding.UTF8);
                //构造一个Email的Message对象
                var mailMessage = new System.Net.Mail.MailMessage(mfrom, mto)
                {
                    //添加邮件主题和内容
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Subject = subject,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    Body = content,
                    IsBodyHtml = false
                };
                //发送邮件
                client.Send(mailMessage);
            }
            catch
            {
                Console.WriteLine("send mail err");
            }
        }
    }
}
