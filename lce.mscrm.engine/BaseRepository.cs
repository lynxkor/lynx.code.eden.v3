/* file name：lce.mscrm.engine.BaseRepository.cs
* author：lynx lynx.kor@163.com @ 2020/8/12 17:23:16
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for BaseRepository
* revision：
*
*/

using System;
using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：BaseRepository
    /// </summary>
    public class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// Get Entity Id
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="fieldName"> </param>
        /// <param name="filedValue"></param>
        /// <returns></returns>
        public Guid? GetId(IOrganizationService service, string entityName, string fieldName, object filedValue)
        {
            var fetchXml = FetchXmlExt.FetchXml(entityName
                , $"{entityName}id"
                , new[] { new ConditionItem(fieldName, filedValue) });
            return service.Retrieve(fetchXml)?.Id;
        }
    }
}