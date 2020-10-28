/* file name：lce.ext.providers.FetchXmlExt.cs
* author：lynx lynx.kor@163.com @ 2019/9/25 11:11:12
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for FetchXmlExt
* revision：
*
*/

using System.Collections.Generic;
using System.Linq;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：FetchXmlExt
    /// </summary>
    public static class FetchXmlExt
    {
        /// <summary>
        /// 拼接XML
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns">   </param>
        /// <param name="conditions"></param>
        /// <param name="orders">    </param>
        /// <param name="page">      </param>
        /// <param name="size">      </param>
        /// <param name="filterType"></param>
        /// <returns>xml with fetch</returns>
        public static string FetchXml(string entityName, IList<string> columns, IList<ConditionItem> conditions = null, IList<OrderItem> orders = null, int page = 0, int size = 0, string filterType = "and")
        {
            return FetchXml(entityName, QueryColumns(entityName, columns), QueryFilter(conditions, filterType), QueryOrder(orders), "", page, size);
        }

        /// <summary>
        /// 拼接XML
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns">   fields split with ','</param>
        /// <param name="conditions"></param>
        /// <param name="orders">    </param>
        /// <param name="page">      </param>
        /// <param name="size">      </param>
        /// <param name="filterType"></param>
        /// <returns></returns>
        public static string FetchXml(string entityName, string columns, IList<ConditionItem> conditions = null, IList<OrderItem> orders = null, int page = 0, int size = 0, string filterType = "and")
        {
            return FetchXml(entityName, QueryColumns(entityName, columns), QueryFilter(conditions, filterType), QueryOrder(orders), "", page, size);
        }

        /// <summary>
        /// 拼接XML
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columnsXml"></param>
        /// <param name="filterXml"> </param>
        /// <param name="ordersXml"> </param>
        /// <param name="linkXml">   </param>
        /// <param name="page">      </param>
        /// <param name="size">      </param>
        /// <returns></returns>
        public static string FetchXml(string entityName, string columnsXml, string filterXml = "", string ordersXml = "", string linkXml = "", int page = 0, int size = 0)
        {
            if (string.IsNullOrEmpty(entityName)) return null;
            var header = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
            if (0 != page && 0 != size)
            {
                header = $"<fetch returntotalrecordcount='true' distinct='false' {(page == 0 ? "" : $" page='{page}'")} {(size == 0 ? "" : $" count='{size}'")}>";
            }
            return
$@"{header}
<entity name='{entityName}'>
{columnsXml}
{ordersXml}
{linkXml}
{filterXml}
</entity>
</fetch>";
        }

        /// <summary>
        /// 拼接 Link Entity Xml
        /// </summary>
        /// <param name="linkEntity"></param>
        /// <param name="from">      </param>
        /// <param name="to">        </param>
        /// <param name="columnsXml"></param>
        /// <param name="filterXml"> </param>
        /// <param name="linkType">  </param>
        /// <param name="alias">     default as alias_{linkEntity}</param>
        /// <param name="linkXml">   </param>
        /// <returns></returns>
        public static string LinkXml(string linkEntity, string from, string to, string columnsXml = "", string filterXml = "", string linkType = "outer", string alias = "", string linkXml = "")
        {
            return
$@"<link-entity name='{linkEntity}' from='{from}' to='{to}' link-type='{linkType}' alias='{(string.IsNullOrEmpty(alias) ? $"alias_{linkEntity}" : alias)}'>
{columnsXml}
{linkXml}
{filterXml}
</link-entity>";
        }

        /// <summary>
        /// 查询所有字段
        /// </summary>
        /// <returns></returns>
        public static string QueryAllColums()
        {
            return @"<all-attributes /><order attribute='createdon' descending='true' />";
        }

        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string QueryColumns(IList<string> columns)
        {
            if (null == columns || columns.Count == 0) return "";
            var result = "";
            foreach (var col in columns)
            {
                result += $"<attribute name='{col.Trim()}' />";
            }
            return result;
        }

        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns">   </param>
        /// <param name="separator"> </param>
        /// <returns></returns>
        public static string QueryColumns(string entityName, string columns, char separator = ',')
        {
            return QueryColumns(entityName, columns.Split(separator));
        }

        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static string QueryColumns(string entityName, IList<string> columns = null)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                var pkCol = $@"{entityName}id";
                if (null == columns)
                {
                    columns = new string[] { pkCol };
                }
                else if (!columns.Contains(pkCol))
                {
                    columns = columns.ToList();
                    columns.Add(pkCol);
                }
            }
            return QueryColumns(columns);
        }

        /// <summary>
        /// 查询条件组装
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static string QueryCondition(string name, int value, string operators = "eq")
        {
            if (value < 0) return "";
            return new ConditionItem(name, value, operators).ToString();
        }

        /// <summary>
        /// 查询条件组装
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static string QueryCondition(string name, string value, string operators = "eq")
        {
            return new ConditionItem(name, value, operators).ToString();
        }

        /// <summary>
        /// 查询条件组装
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static string QueryCondition(IList<ConditionItem> conditions)
        {
            if (null == conditions || conditions.Count == 0) return "";
            var fetchCondition = "";
            foreach (var c in conditions)
            {
                fetchCondition += c.ToString();
            }
            return fetchCondition;
        }

        /// <summary>
        /// 查询条件组装
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="values">   </param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static string QueryCondition(string name, IList<int> values, string operators = "eq")
        {
            if (null == values || values.Count == 0) return "";
            return new ConditionItem(name, values, operators).ToString();
        }

        /// <summary>
        /// 查询条件组装
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="values">   </param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static string QueryCondition(string name, IList<string> values, string operators = "eq")
        {
            if (null == values || values.Count == 0) return "";
            return new ConditionItem(name, values, operators).ToString();
        }

        /// <summary>
        /// 组装Filter
        /// </summary>
        /// <param name="name">     </param>
        /// <param name="value">    </param>
        /// <param name="operators"></param>
        /// <returns>xml with filter</returns>
        public static string QueryFilter(string name, object value, string operators = "eq")
        {
            return QueryFilter(new ConditionItem(name, value, operators));
        }

        /// <summary>
        /// 组装Filter
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="type">      </param>
        /// <returns>xml with filter</returns>
        public static string QueryFilter(ConditionItem conditions, string type = "and")
        {
            return QueryFilter(new[] { conditions }, type);
        }

        /// <summary>
        /// 组装Filter
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="type">      </param>
        /// <returns>xml with filter</returns>
        public static string QueryFilter(IList<ConditionItem> conditions, string type = "and")
        {
            if (null == conditions || conditions.Count == 0) return "";
            return QueryFilter(QueryCondition(conditions), type);
        }

        /// <summary>
        /// 组装Filter
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="type">        </param>
        /// <returns>xml with filter</returns>
        public static string QueryFilter(string conditionXml, string type = "and")
        {
            return $@"<filter type='{type}'>{conditionXml}</filter>";
        }

        /// <summary>
        /// 排序条件组装
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static string QueryOrder(IList<OrderItem> orders)
        {
            if (null == orders || orders.Count == 0) return "";
            var fetchOrder = "";
            foreach (var o in orders)
            {
                fetchOrder += o.ToString();
            }
            return fetchOrder;
        }

        /// <summary>
        /// 排序条件组装
        /// </summary>
        /// <param name="attribute"> attribute</param>
        /// <param name="descending">is descending</param>
        /// <returns></returns>
        public static string QueryOrder(string attribute, bool descending = true)
        {
            return new OrderItem(attribute, descending).ToString();
        }
    }

    /// <summary>
    /// 查询条件项
    /// </summary>
    public class ConditionItem
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="attr">     </param>
        /// <param name="value">    </param>
        /// <param name="_operator"></param>
        public ConditionItem(string attr, dynamic value, string _operator = "eq")
        {
            Attribute = attr;
            Value = value;
            Operator = _operator;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 运算符
        /// </summary>
        public string Operator { get; set; } = "eq";

        /// <summary>
        /// 字段值
        /// </summary>
        public dynamic Value { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (null == Value && Operator == "eq") return "";
            var type = Value.GetType();
            //var baseType = type.BaseType;
            if (type.Name == "Int32[]")
            {
                return ToString(((int[])Value).Select(x => x.ToString()).ToList());
            }
            else if (type.Name == "String[]")
            {
                return ToString(((string[])Value).Select(x => x.ToString()).ToList());
            }
            if (type.FullName.IndexOf("List") > 0)
            {
                if (type.GenericTypeArguments[0].Name == "Int32")
                {
                    return ToString(((List<int>)Value).Select(x => x.ToString()).ToList());
                }
                else if (type.GenericTypeArguments[0].Name == "String")
                {
                    return ToString(((List<string>)Value).Select(x => x.ToString()).ToList());
                }
                else
                {
                    return ToString(((List<object>)Value).Select(x => x.ToString()).ToList());
                }
            }
            else if (type.FullName.IndexOf("Array") > 0)
            {
                return ToString(((object[])Value).Select(x => x.ToString()).ToList());
            }

            if (string.IsNullOrEmpty(Value.ToString()) && Operator == "eq") return "";
            return $"<condition attribute='{Attribute}' operator='{Operator}' {(string.IsNullOrEmpty(Value.ToString()) ? "" : $"value='{Value}'")} /> ";
        }

        private string ToString(IList<string> values)
        {
            if (null == values || values.Count == 0) return "";
            if (values.Count > 1)
            {
                if (Operator == "eq") Operator = "in";
                var vStr = "";
                foreach (var value in values)
                {
                    vStr += $"<value>{value}</value>";
                }
                return $"<condition attribute='{Attribute}' operator='{Operator}'>{vStr}</condition>";
            }
            if (Operator == "in") Operator = "eq";
            return $"<condition attribute='{Attribute}' operator='{Operator}' {(string.IsNullOrEmpty(values[0].ToString()) ? "" : $"value='{values[0]}'")} /> ";
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="desc"></param>
        public OrderItem(string attr, bool desc = true)
        {
            Attribute = attr;
            Descending = desc;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool Descending { get; set; } = true;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Attribute)) return "";
            return $@"<order attribute='{Attribute}' descending='{Descending.ToString().ToLower()}' />";
        }
    }
}