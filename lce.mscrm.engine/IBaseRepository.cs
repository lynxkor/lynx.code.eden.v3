/* file name：lce.mscrm.engine.IBaseRepository.cs
* author：lynx lynx.kor@163.com @ 2020/8/12 17:20:45
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for IBaseRepository
* revision：
*
*/

using System;
using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：IBaseRepository
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Get Entity Id
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="fieldName"> </param>
        /// <param name="filedValue"></param>
        /// <returns></returns>
        Guid? GetId(IOrganizationService service, string entityName, string fieldName, object filedValue);
    }
}