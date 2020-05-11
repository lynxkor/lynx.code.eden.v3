/* file name：lce.provider.StringExt.cs
* author：lynx lynx.kor@163.com @ 2020/05/11 10:29:22
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for StringExt
* revision：
*
*/

using System;
using System.Text;

namespace lce.provider
{
    /// <summary>
    /// action：StringExt
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// string to int32.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this string input)
        {
            try { return Convert.ToInt32(input); }
            catch { return -1; }
        }

        /// <summary>
        /// left {length} char of string.
        /// </summary>
        /// <param name="input"> </param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string input, int length)
        {
            return input.Left(length, ' ').Trim();
        }

        /// <summary>
        /// left {length} char of string supplement with {supplemen}.
        /// </summary>
        /// <param name="input">     </param>
        /// <param name="length">    </param>
        /// <param name="supplement"></param>
        /// <returns></returns>
        public static string Left(this string input, int length, char supplement)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length >= length) return input.Substring(0, length);
            return input.PadRight(length, supplement);
        }

        /// <summary>
        /// right {length} char of string.
        /// </summary>
        /// <param name="input"> </param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this int input, int length)
        {
            return input.ToString().Right(length, ' ').Trim();
        }

        /// <summary>
        /// right {length} char of string supplement with {supplemen}.
        /// </summary>
        /// <param name="input">     </param>
        /// <param name="length">    </param>
        /// <param name="supplement"></param>
        /// <returns></returns>
        public static string Right(this int input, int length, char supplement)
        {
            return input.ToString().Right(length, supplement);
        }

        /// <summary>
        /// right {length} char of string.
        /// </summary>
        /// <param name="input"> </param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string input, int length)
        {
            return input.Right(length, ' ').Trim();
        }

        /// <summary>
        /// right {length} char of string supplement with {supplemen}.
        /// </summary>
        /// <param name="input">     </param>
        /// <param name="length">    </param>
        /// <param name="supplement">长度不度时(前)补位字符</param>
        /// <returns></returns>
        public static string Right(this string input, int length, char supplement)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length >= length) return input.Substring(input.Length - length);
            return input.PadLeft(length, supplement);
        }

        /// <summary>
        /// 字符串转换成十六进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToHex(this string input)
        {
            byte[] b = Encoding.UTF8.GetBytes(input);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 十进制2十六进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToHex(this int input)
        {
            return Convert.ToString(input, 16).ToUpper();
        }
    }
}