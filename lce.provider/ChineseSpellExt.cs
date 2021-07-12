/* file name：lce.provider.ChineseSpellExt.cs
* author：lynx lynx.kor@163.com @ 2021/6/21 15:18:48
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ChineseSpellExt
* revision：
*
*/

using System.Text;
using NPinyin;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//注册编码对象
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            string strA = Pinyin.ConvertEncoding(input, Encoding.UTF8, gb2312);
            //首字母
            return Pinyin.GetInitials(strA, gb2312);
            //拼音
            //string strC = Pinyin.GetPinyin(str);
        }
    }
}