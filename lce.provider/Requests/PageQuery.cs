/* file name：${namespace}.PageQuery.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/19 22:07
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for PageQuery
* revision：
*
*/

using System.Collections.Generic;

namespace lce.provider.Requests
{
    /// <summary>
    /// 分頁查询
    /// </summary>
    public class PageQuery : BaseQuery
    {
        /// <summary>
        /// 排序条件
        /// </summary>
        public Dictionary<string, bool> Orders { get; set; }

        /// <summary>
        /// 頁码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 頁阀
        /// </summary>
        public int Size { get; set; } = 10;
    }
}