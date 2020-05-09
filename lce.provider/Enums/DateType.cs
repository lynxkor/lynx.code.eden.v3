/* file name：${namespace}.DateType.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/19 22:58
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for DateType
* revision：
*
*/

namespace lce.provider.Enums
{
    /// <summary>
    /// 时间维度
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// 年
        /// </summary>
        Year = 1010,

        /// <summary>
        /// 月
        /// </summary>
        Month = 1020,

        /// <summary>
        /// 日
        /// </summary>
        Day = 1030,

        /// <summary>
        /// 周
        /// </summary>
        Week = 1040,

        /// <summary>
        /// 季
        /// </summary>
        Quarter = 1050,

        /// <summary>
        /// 区间
        /// </summary>
        Region = 1060
    }
}