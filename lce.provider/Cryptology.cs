/* file name：lce.provider.Cryptology.cs
* author：lynx lynx.kor@163.com @ 2019/6/6 16:32
* copyright (c) 2019 lynxce.com
* desc：
* > add description for Cryptology
* revision：
*
*/

using System;
using System.Security.Cryptography;
using System.Text;

namespace lce.provider
{
    /// <summary>
    /// 密码/编码/验证码 生成器
    /// </summary>
    public static class Cryptology
    {
        /// <summary>
        /// To the md5.
        /// </summary>
        /// <returns>The md5.</returns>
        /// <param name="input">Input.</param>
        public static string ToMd5(this string input)
        {
            using var provider = MD5.Create();
            return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns>The random.</returns>
        /// <param name="lenght">Lenght.</param>
        /// <param name="useNum">If set to <c>true</c> use number.</param>
        /// <param name="useLow">If set to <c>true</c> use low.</param>
        /// <param name="useUpp">If set to <c>true</c> use upp.</param>
        /// <param name="useSpe">If set to <c>true</c> use special char.</param>
        public static string Captcha(int lenght, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false)
        {
            var b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            var r = new Random(BitConverter.ToInt32(b, 0));
            var code = string.Empty;
            var dic = string.Empty;
            if (useNum) dic += "0123456789";
            if (useLow) dic += "abcdefghijklmnopqrstuvwxyz";
            if (useUpp) dic += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (useSpe) dic += "!#$%&'()*+,-./:;<=>?@[]^_`{|}~";
            var dicLength = dic.Length - 1;
            for (int i = 0; i < lenght; i++)
            {
                code += dic.Substring(r.Next(0, dicLength), 1);
            }
            return code;
        }

        /// <summary>
        /// GUID 2 UUID CODE
        /// </summary>
        /// <returns></returns>
        public static string Code()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}