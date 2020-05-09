/* file name：lce.provider.QuerySqlExt.cs
* author：lynx lynx.kor@163.com @ 2020/3/8 10:07
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for QuerySqlExt
* revision：
*
*/

using System.Collections.Generic;
using System.Linq;

namespace lce.provider
{
    /// <summary>
    /// QuerySqlExt
    /// </summary>
    public static class QuerySqlExt
    {
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"></param>
        /// <returns>AND {name} = {value}</returns>
        public static object Condition(string name, int? value)
        {
            if (!value.HasValue || value.Value == -1) return "";
            return $@"AND {name} = {value} ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"></param>
        /// <returns>AND {name} = '{value}'</returns>
        public static object Condition(string name, string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return $@"AND {name} = '{value}' ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name">  </param>
        /// <param name="values"></param>
        /// <returns>AND {name} = {values}/IN({values})</returns>
        public static string Condition(string name, IList<int> values)
        {
            if (null == values || values.Count == 0) return "";
            if (values.Count == 1)
                return $@"AND {name} = {values[0]} ";
            else
                return $@"AND {name} IN({string.Join(",", values)}) ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name">  </param>
        /// <param name="values"></param>
        /// <returns>AND {name} = '{values}'/IN('{values}')</returns>
        public static string Condition(string name, IList<string> values)
        {
            if (null == values || values.Count == 0) return "";
            if (values.Count == 1)
                return $@"AND {name} = '{values[0]}' ";
            else
                return $@"AND {name} IN({string.Join(",", values.Select(x => $@"'{x}'"))}) ";
        }
    }
}