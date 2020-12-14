/* file name：${namespace}.ReportQuery.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/19 22:57
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for ReportQuery
* revision：
*
*/

using System;
using System.Collections.Generic;
using lce.provider.Enums;

namespace lce.provider.Requests
{
    /// <summary>
    /// 通用报表查询参数
    /// <para>日期时间为空时默认取当前时间;</para>
    /// <para>时间维度为1010~1050时取Date[0]计算对应日期所在的时间维度第一天到最后一天;</para>
    /// <para>时间维度为1060时根据Date中两个日期时间值指定区间用作运算</para>
    /// </summary>
    public class ReportQuery : PageQuery
    {
        /// <summary>
        /// 日期时间
        /// </summary>
        public IList<DateTime> Date { get; set; } = new List<DateTime> { };

        /// <summary>
        /// 时间维度 1010:年，1020:月，1030:日，1040:周，1050:季，1060:自定义区间
        /// <code>DateTimeExt.DateType</code>
        /// </summary>
        public DateType DateType { get; set; } = DateType.Month;
    }
}