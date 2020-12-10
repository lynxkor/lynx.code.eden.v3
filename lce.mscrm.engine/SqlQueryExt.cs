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
        /// <param name="name">     </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns>AND {name} = {value}</returns>
        public static string And(string name, int? value, string operators = "=")
        {
            if (!value.HasValue || value.Value == -1) return "";
            return $" AND {name} {operators} {value} ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns>AND {name} = '{value}'</returns>
        public static string And(string name, string value, string operators = "=")
        {
            if (string.IsNullOrEmpty(value)) return "";
            return $" AND {name} {operators} '{value}' ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="names">    </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static string And(IList<string> names, string value, string operators = "=")
        {
            if (string.IsNullOrEmpty(value)) return "";
            var conditions = new List<string>();
            foreach (var name in names)
            {
                conditions.Add($" {name} {operators} '{value}' ");
            }
            return $" AND ({string.Join(" OR ", conditions)} )";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name">  </param>
        /// <param name="values"></param>
        /// <returns>AND {name} = {values}/IN({values})</returns>
        public static string And(string name, IList<int> values)
        {
            if (null == values || values.Count == 0) return "";
            if (values.Count == 1)
                return $" AND {name} = {values[0]} ";
            else
                return $" AND {name} IN({string.Join(",", values)}) ";
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="name">  </param>
        /// <param name="values"></param>
        /// <returns>AND {name} = '{values}'/IN('{values}')</returns>
        public static string And(string name, IList<string> values)
        {
            if (null == values || values.Count == 0) return "";
            if (values.Count == 1)
                return $" AND {name} = '{values[0]}' ";
            else
                return $" AND {name} IN({string.Join(",", values.Select(x => $@"'{x}'"))}) ";
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

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">   </param>
        /// <param name="querySql">  </param>
        /// <param name="parameters"></param>
        /// <param name="caller">    </param>
        /// <param name="page">      页码</param>
        /// <param name="size">      页阀</param>
        /// <returns></returns>
        public static IList<T> Query<T>(this DbContext context, string querySql, SqlParameter[] parameters = null, Guid? caller = null, int page = 0, int size = 0)
        {
            if (null != caller && caller != Guid.Empty)
            {
                querySql = $@"DECLARE @binUserGuid VARBINARY(128)
                            SET @binUserGuid = CAST(CAST('{caller}' as UNIQUEIDENTIFIER) AS VARBINARY(128))
                            SET context_info @binUserGuid;
                            {querySql}";
            }
            if (page > 0 && size > 0)
            {
                if (null != parameters)
                    return context.Database.SqlQuery<T>(querySql, parameters)
                        .Skip((page - 1) * size).Take(size).ToList();
                return context.Database.SqlQuery<T>(querySql)
                        .Skip((page - 1) * size).Take(size).ToList();
            }
            if (null != parameters)
                return context.Database.SqlQuery<T>(querySql, parameters).ToListAsync().Result;
            return context.Database.SqlQuery<T>(querySql).ToListAsync().Result;
        }
    }
}