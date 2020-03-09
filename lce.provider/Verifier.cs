/* file name：lce.provider.Verifier.cs
 * author：lynx lynx.kor@163.com @ 2019/6/12 16:08
 * copyright (c) 2019 Copyright@lynxce.com
 * desc：
 * > add description for Verifier
 * revision：
 *
 */

using System;
using System.Text.RegularExpressions;

namespace lce.provider
{
    /// <summary>
    /// Verifier
    /// </summary>
    public static class Verifier
    {
        /// <summary>
        /// 判断对象是否为数值
        /// </summary>
        /// <returns><c>true</c>, if number was ised, <c>false</c> otherwise.</returns>
        /// <param name="Value">Value.</param>
        public static bool IsNumber(this string Value)
        {
            return Regex.IsMatch(Value, "^[0-9]*$");
        }

        /// <summary>
        /// 校验是否为日期格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法日期返回传入值,非法日期返回当前日期值</returns>
        public static DateTime VerifyDateTime(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return DateTime.Now;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return DateTime.Now;
                    return DateTime.Parse(Value.ToString());
                }
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// 反回指定格式的日期字符串
        /// </summary>
        /// <param name="Value"> </param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string VerifyDateTime(this object Value, string format)
        {
            return Value.VerifyDateTime().ToString(format);
        }

        /// <summary>
        /// 校验是否为长整型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法长整型返回传入值,非法长整型返回零</returns>
        public static long VerifyLong(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if ((Value != null) && (Value.ToString() != "null"))
                {
                    if (Value.ToString() == "")
                        return 0;
                    return long.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为短整型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法短整型返回传入值,非法短整型返回零</returns>
        public static short VerifyShort(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return 0;
                    return short.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为整型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法整型返回传入值,非法整型返回零</returns>
        public static int VerifyInt(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString().ToUpper() == "TRUE")
                        return 1;
                    if (Value.ToString().ToUpper() == "FALSE")
                        return 0;
                    if (Value.ToString() == "")
                        return 0;
                    try
                    {
                        return Convert.ToInt32(Value);
                    }
                    catch
                    {
                        return int.Parse(Value.ToString());
                    }
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为字符串
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string VerifyString(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return "";
            else
            {
                if (Value != null)
                    return Value.ToString();
                else
                    return "";
            }
        }

        /// <summary>
        /// 校验布尔值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool VerifyBool(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return false;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return false;

                    if (Value.ToString() == "0")
                        return false;
                    if (Value.ToString() == "1")
                        return true;

                    return bool.Parse(Value.ToString());
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 校验是否为double型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法double型返回传入值,非法double型返回零</returns>
        public static double VerifyDouble(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return 0;
                    return double.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为single型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法single型返回传入值,非法single型返回零</returns>
        public static float VerifySingle(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return 0;
                    return float.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为Decimal型格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法Decimal型返回传入值,非法Decimal型返回零</returns>
        public static Decimal VerifyDecimal(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return 0;
                    return decimal.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// 校验是否为字节格式
        /// </summary>
        /// <param name="Value">要校验的值</param>
        /// <returns>合法字节返回传入值,非法字节返回零</returns>
        public static byte VerifyByte(this object Value)
        {
            if (DBNull.Value.Equals(Value))
                return 0;
            else
            {
                if (Value != null)
                {
                    if (Value.ToString() == "")
                        return 0;
                    return byte.Parse(Value.ToString());
                }
                else
                    return 0;
            }
        }
    }
}