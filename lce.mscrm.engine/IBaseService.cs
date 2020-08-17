/* file name：lce.mscrm.engine.IBaseService.cs
* author：lynx lynx.kor@163.com @ 2019/9/30 13:13:35
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for IBaseService
* revision：
*
*/

using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：IBaseService
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 统计指定条件下数据记录数
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="filterXml"> </param>
        /// <returns></returns>
        int Count(IOrganizationService service, string entityName, string filterXml);
    }
}