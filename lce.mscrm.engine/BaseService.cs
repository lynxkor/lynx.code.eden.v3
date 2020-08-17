/* file name：lce.mscrm.engine.BaseService.cs
* author：lynx lynx.kor@163.com @ 2019/9/30 13:33:45
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for BaseService
* revision：
*
*/

using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：BaseService
    /// </summary>
    public class BaseService : IBaseService
    {
        /// <summary>
        /// 按条件统计计数
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="filterXml"> </param>
        /// <returns></returns>
        public int Count(IOrganizationService service, string entityName, string filterXml = "")
        {
            try
            {
                var fetchXml = $@"
<fetch distinct='true' mapping='logical' aggregate='true'>
<entity name='{entityName}'>
<attribute name='{entityName}id' alias='totalscount' aggregate='count'/>
{filterXml}
</entity>
</fetch>";
                var entity = service.Retrieve(fetchXml);
                if (null != entity)
                {
                    return entity.Contains("totalscount") ? (int)((AliasedValue)entity["totalscount"]).Value : 0;
                }
            }
            catch { }
            return 0;
        }
    }
}