/* file name：lce.mscrm.engine.SqlQueryExt.cs
* author：lynx lynx.kor@163.com @ 2019/12/17 17:01:32
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for SqlQueryExt
* revision：
*
*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：SqlQueryExt
    /// </summary>
    public static class SqlQueryExt
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
            return $@" AND {name} = {value} ";
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
            return $@" AND {name} = '{value}' ";
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
                return $@" AND {name} = {values[0]} ";
            else
                return $@" AND {name} IN({string.Join(",", values)}) ";
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
                return $@" AND {name} = '{values[0]}' ";
            else
                return $@" AND {name} IN({string.Join(",", values.Select(x => $@"'{x}'"))}) ";
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"> </param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public static IList<T> Query<T>(this DbContext context, string querySql)
        {
            return context.Query<T>(querySql);
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">   </param>
        /// <param name="caller">    </param>
        /// <param name="querySql">  </param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<T> Query<T>(this DbContext context, string querySql, SqlParameter[] parameters = null, Guid? caller = null)
        {
            if (null != caller && caller != Guid.Empty)
            {
                querySql = $@"DECLARE @binUserGuid VARBINARY(128)
                            SET @binUserGuid = CAST(CAST('{caller}' as UNIQUEIDENTIFIER) AS VARBINARY(128))
                            SET context_info @binUserGuid;
                            {querySql}";
            }
            return context.Database.SqlQuery<T>(querySql, parameters).ToListAsync().Result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="context">   </param>
        /// <param name="commandSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int Execute(this DbContext context, string commandSql, SqlParameter[] parameters)
        {
            return context.Database.ExecuteSqlCommand(commandSql, parameters);
        }
    }
}