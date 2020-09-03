/* file name：lce.ext.providers.OptionSetExt.cs
* author：lynx lynx.kor@163.com @ 2019/9/18 9:04:37
* copyright (c) 2019 Copyright@lynxce.com
* desc：
* > add description for OptionSetExt
* revision：
*
*/

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action:OptionSetExt
    /// </summary>
    public static class OptionSetExt
    {
        /// <summary>
        /// 根据entity获取EntityMetadata
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static EntityMetadata EntityMetadata(this IOrganizationService service, string entityName)
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = entityName,
                EntityFilters = EntityFilters.Attributes
            };
            var response = (RetrieveEntityResponse)service.Execute(request);
            return response.EntityMetadata;
        }

        /// <summary>
        /// 通过选项值获取选项描述 这个效率更高，建议用这个 通过OptionsSetExt.EntityMetadata 先获取EntityMetadata
        /// </summary>
        /// <param name="entity">       EntityMetadata</param>
        /// <param name="attributeName"></param>
        /// <param name="optionValue">  </param>
        /// <returns></returns>
        public static string OptionLabel(this EntityMetadata entity, string attributeName, int optionValue)
        {
            if (null == entity) return string.Empty;
            if (optionValue == -1) return string.Empty;
            var metadata = (PicklistAttributeMetadata)entity.Attributes.Where(x => x.LogicalName.Equals(attributeName)
                                                && x.AttributeType.Value == AttributeTypeCode.Picklist).FirstOrDefault();
            if (null != metadata)
            {
                var option = metadata.OptionSet.Options.Where(x => x.Value == optionValue).FirstOrDefault();
                if (null != option) return option.Label.UserLocalizedLabel.Label;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过选项值获取选项描述
        /// </summary>
        /// <param name="service">      </param>
        /// <param name="optionSetName"></param>
        /// <param name="optionValue">  </param>
        /// <returns></returns>
        public static string OptionLabel(this IOrganizationService service, string optionSetName, int optionValue)
        {
            var request = new RetrieveOptionSetRequest
            {
                Name = optionSetName
            };
            var response = (RetrieveOptionSetResponse)service.Execute(request);
            var metadata = (OptionSetMetadata)response.OptionSetMetadata;
            var option = metadata.Options.Where(x => x.Value == optionValue).FirstOrDefault();
            if (null != option) return option.Label.UserLocalizedLabel.Label;
            return string.Empty;
        }

        /// <summary>
        /// 获取选项集列表
        /// </summary>
        /// <param name="service">      </param>
        /// <param name="optionSetName"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, int>> OptionList(this IOrganizationService service, string optionSetName)
        {
            var request = new RetrieveOptionSetRequest
            {
                Name = optionSetName
            };
            var response = (RetrieveOptionSetResponse)service.Execute(request);
            var metadata = (OptionSetMetadata)response.OptionSetMetadata;
            var list = metadata.Options.ToArray();
            if (null != list && list.Length > 0)
            {
                var results = new List<KeyValuePair<string, int>>();
                foreach (var item in list)
                {
                    results.Add(new KeyValuePair<string, int>(item.Label.UserLocalizedLabel.Label, item.Value.Value));
                }
                return results;
            }
            return null;
        }

        /// <summary>
        /// 获取选项集列表 这个效率更高，建议用这个
        /// </summary>
        /// <param name="service">      </param>
        /// <param name="entityName">   </param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, int>> OptionSet(this IOrganizationService service, string entityName, string attributeName)
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = entityName,
                EntityFilters = EntityFilters.Attributes
            };
            var response = (RetrieveEntityResponse)service.Execute(request);
            var entityMetadata = response.EntityMetadata;
            var metadata = (PicklistAttributeMetadata)entityMetadata.Attributes.Where(x => x.LogicalName.Equals(attributeName) && x.AttributeType.Value == AttributeTypeCode.Picklist).FirstOrDefault();
            if (null != metadata)
            {
                return metadata.OptionSet.Options.Select(x => new KeyValuePair<string, int>(x.Label.UserLocalizedLabel.Label, x.Value.Value)).ToList();
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="service">      </param>
        /// <param name="optionSetName"></param>
        /// <returns></returns>
        public static Dictionary<string, int> OptionSet(this IOrganizationService service, string optionSetName)
        {
            var type = new Dictionary<string, int>();
            var typeName = optionSetName.ToLower();
            var request = new RetrieveOptionSetRequest { Name = typeName };
            RetrieveOptionSetResponse response;
            try
            {
                response = (RetrieveOptionSetResponse)service.Execute(request);
            }
            catch
            {
                try
                {
                    request = new RetrieveOptionSetRequest
                    {
                        Name = optionSetName.ToLower()
                    };
                    response = (RetrieveOptionSetResponse)service.Execute(request);
                }
                catch { return type; }
            }
            if (null != response)
            {
                var metadata = (OptionSetMetadata)response.OptionSetMetadata;
                var list = metadata.Options.ToArray();
                if (null != list && list.Length > 0)
                {
                    foreach (var item in list)
                    {
                        type.Add(item.Label.UserLocalizedLabel.Label, item.Value.Value);
                    }
                }
            }
            return type;
        }

        /// <summary>
        /// 通过选项描述获取选项值 这个效率更高，建议用这个 通过OptionsSetExt.EntityMetadata 先获取EntityMetadata
        /// </summary>
        /// <param name="entity">       </param>
        /// <param name="attributeName"></param>
        /// <param name="optionLabel">  </param>
        /// <returns></returns>
        public static int OptionValue(this EntityMetadata entity, string attributeName, string optionLabel)
        {
            if (string.IsNullOrEmpty(optionLabel)) return -1;
            var metadata = (PicklistAttributeMetadata)entity.Attributes.Where(x => x.LogicalName.Equals(attributeName)
                                                && x.AttributeType.Value == AttributeTypeCode.Picklist).FirstOrDefault();
            if (null != metadata)
            {
                var option = metadata.OptionSet.Options.Where(x => x.Label.UserLocalizedLabel.Label == optionLabel).FirstOrDefault();
                if (null != option) return option.Value ?? -1;
            }
            return -1;
        }

        /// <summary>
        /// 通过选项描述获取选项值
        /// </summary>
        /// <param name="service">      </param>
        /// <param name="optionSetName"></param>
        /// <param name="optionLabel">  </param>
        /// <returns></returns>
        public static int OptionValue(this IOrganizationService service, string optionSetName, string optionLabel)
        {
            var request = new RetrieveOptionSetRequest
            {
                Name = optionSetName
            };
            var response = (RetrieveOptionSetResponse)service.Execute(request);
            var metadata = (OptionSetMetadata)response.OptionSetMetadata;
            var option = metadata.Options.Where(x => x.Label.UserLocalizedLabel.Label == optionLabel).FirstOrDefault();
            if (null != option) return option.Value ?? -1;
            return -1;
        }
    }
}