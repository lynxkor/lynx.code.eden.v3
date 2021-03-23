/* file name：lce.provider.DateTimeExt.cs
* author：lynx lynx.kor@163.com @ 2019/6/5 23:25
* copyright (c) 2019 lynxce.com
* desc：
* > add description for DateTimeExt
* revision：
*
*/

using System;
using lce.provider.Enums;

namespace lce.provider
{
    /// <summary>
    /// Date time ext.
    /// </summary>
    public static class DateTimeExt
    {
        /// <summary>
        /// Format datetime,default yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="input"> </param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(this DateTime input, string format = "yyyy-MM-dd HH:mm:ss")
        {
            try
            {
                return input.ToString(format);
            }
            catch
            {
                return "参数有误";
            }
        }

        /// <summary>
        /// Format yyyy-MM-dd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatShort(this DateTime input)
        {
            return input.Format("yyyy-MM-dd");
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime MonthFirstDay(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).Date;
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime MonthLastDay(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).Date.AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 季度
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int Quarter(this DateTime dateTime)
        {
            return dateTime.Month / 3 + 1;
        }

        /// <summary>
        /// 季度第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime QuarterFirstDay(this DateTime dateTime)
        {
            return dateTime.AddMonths(0 - ((dateTime.Month - 1) % 3)).MonthFirstDay();
        }

        /// <summary>
        /// 季度最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime QuarterLastDay(this DateTime dateTime)
        {
            return dateTime.AddMonths(3 - ((dateTime.Month - 1) % 3)).Date.AddSeconds(-1); //最后一秒
        }

        /// <summary>
        /// 根据日期和类型返回对应的开始时间和结束时间
        /// </summary>
        /// <param name="type">     类型：日、周、月、季、年</param>
        /// <param name="date">     日期</param>
        /// <param name="startDate">开始时间 yyyy-MM-dd 00:00:00</param>
        /// <param name="endDate">  结束时间 yyyy-MM-dd 23:59:59</param>
        public static void SwitchType4Date(this DateTime date, DateType type, out DateTime startDate, out DateTime endDate)
        {
            switch (type)
            {
                case DateType.Week://周
                    startDate = date.WeekFirstDay();
                    endDate = startDate.WeekLastDay();
                    break;

                case DateType.Month://月
                    startDate = date.MonthFirstDay();
                    endDate = startDate.MonthLastDay();
                    break;

                case DateType.Quarter://季
                    startDate = date.QuarterFirstDay();
                    endDate = startDate.QuarterLastDay();
                    break;

                case DateType.Year://年
                    startDate = date.YearFirstDay();
                    endDate = startDate.YearLastDay();
                    break;

                case DateType.Day://日
                default://默认
                    startDate = date.Date;
                    endDate = startDate.AddDays(1).AddSeconds(-1);
                    break;
            }
        }

        /// <summary>
        /// Format datetime 4 code
        /// </summary>
        /// <param name="input"> </param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToCode(this DateTime input, string format = "yyyyMMddHHmmss")
        {
            try
            {
                return input.ToString(format);
            }
            catch
            {
                return "参数有误";
            }
        }

        /// <summary>
        /// string 2 datetime
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object input)
        {
            try
            {
                return Convert.ToDateTime(input);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// string 2 local time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToLocal(this object input)
        {
            try
            {
                return Convert.ToDateTime(input).ToLocalTime();
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// string 2 utc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToUTC(this object input)
        {
            try
            {
                return Convert.ToDateTime(input).ToUniversalTime();
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 取得本周第一天（周日为第一天）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime WeekFirstDay(this DateTime dateTime)
        {
            return dateTime.AddDays(0 - (int)dateTime.DayOfWeek).Date;
        }

        /// <summary>
        /// 得到本周最后一天（周六为最后一天）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime WeekLastDay(this DateTime dateTime)
        {
            return dateTime.AddDays(7 - (int)dateTime.DayOfWeek).Date.AddSeconds(-1);
        }

        /// <summary>
        /// 根据年度周及年到周最后一天
        /// </summary>
        /// <param name="week"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime WeekLastDay(int week, int year)
        {
            var firstday = new DateTime(year, 1, 1);
            int weekday = (int)firstday.DayOfWeek;
            return firstday.AddDays(-weekday).AddDays(week * 7);
        }

        /// <summary>
        /// 取日期在某月的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int WeekOfMonth(this DateTime dateTime)
        {
            var mth1thDay = dateTime.MonthFirstDay();   //当月第一天
            var weekday = (int)mth1thDay.DayOfWeek;     //当月第一天为周几
            var lstDay1thWeek = 7 - ((weekday == 0 ? 7 : weekday) - 1); //第一周最后一天
            var diffDay = dateTime.Day - lstDay1thWeek; //当前日期与第一周最后一天差值
            diffDay = diffDay > 0 ? diffDay : 1;        //差值为负则置为1
            //当前是第几周,如果整除7就减一天
            return (diffDay % 7 == 0
                ? (diffDay / 7 - 1)
                : diffDay / 7) + 1 + (dateTime.Day > lstDay1thWeek ? 1 : 0);
        }

        /// <summary>
        /// 取日期在年中的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime dateTime)
        {
            var firstday = dateTime.YearFirstDay();
            int weekday = (int)firstday.DayOfWeek;
            int weeknum = (dateTime.DayOfYear + weekday - 2) / 7 + 1;
            return weeknum;
        }

        /// <summary>
        /// 取得某年的第一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime YearFirstDay(this DateTime dateTime)
        {
            return dateTime.AddMonths(1 - dateTime.Month).AddDays(1 - dateTime.Day).Date;
        }

        /// <summary>
        /// 取得某年的最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime YearLastDay(this DateTime dateTime)
        {
            //次年第一个月
            dateTime = dateTime.AddMonths(1 - dateTime.Month).Date.AddYears(1);
            //月度第一天 00:00 减一分钟=当年最后一分钟最后一秒
            return dateTime.AddDays(1 - dateTime.Day).Date.AddSeconds(-1);
        }
    }
}