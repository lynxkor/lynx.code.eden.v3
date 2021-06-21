/* file name：lce.provider.ChineseSpellExt.cs
* author：lynx lynx.kor@163.com @ 2021/6/21 15:18:48
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ChineseSpellExt
* revision：
*
*/

using System.Text;

namespace lce.provider
{
    /// <summary>
    /// action：ChineseSpellExt
    /// </summary>
    public static class ChineseSpellExt
    {
        /// <summary>
        /// 获取汉字首字母（可包含多个汉字）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToChsSpell(this string input)
        {
            input = input.Replace("行", "H");
            input = input.Replace("圳", "Z");
            input = input.Replace("莞", "G");
            int len = input.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += ToSpell(input.Substring(i, 1));
            }
            return myStr;
        }

        private static string ToSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "N";
            }
            else
                return cnChar;
        }
    }
}