/*
 * file name：lce.mscrm.engine.ServiceProxyExt.cs
 * author：lynx lynx.kor@163.com @ 2018/9/19 14:25:28
 * copyright (c) 2018 Copyright@lynxce.com
 * desc：
 * > add description for ServiceProxyExt
 * revision：
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel.Description;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：ServiceProxyExt
    /// </summary>
    public static class ServiceProxyExt
    {
        /// <summary>
        /// 分派;把target分派给owner
        /// </summary>
        /// <param name="service">         </param>
        /// <param name="ownerEntityName"> </param>
        /// <param name="ownerId">         </param>
        /// <param name="targetEntityName"></param>
        /// <param name="targetId">        </param>
        public static void Assign(this IOrganizationService service, string ownerEntityName, Guid ownerId, string targetEntityName, Guid targetId)
        {
            var assign = new AssignRequest
            {
                Assignee = new EntityReference(ownerEntityName, ownerId),
                Target = new EntityReference(targetEntityName, targetId)
            };
            service.Execute(assign);
        }

        /// <summary>
        /// 给实体指定查找字段建立关联关系
        /// </summary>
        /// <param name="service">         </param>
        /// <param name="sourceEntityName">查找实体</param>
        /// <param name="sourceId">        查找实体数据id</param>
        /// <param name="targetEntityName">关联实体</param>
        /// <param name="targetFieldName"> 关联字段</param>
        /// <param name="targetId">        关联数据id</param>
        public static void Associate(this IOrganizationService service, string sourceEntityName, Guid sourceId, string targetEntityName, string targetFieldName, Guid targetId)
        {
            var target = service.Retrieve(targetEntityName, targetId, new ColumnSet(new string[] { $@"{targetEntityName}id" }));
            target[targetFieldName] = new EntityReference(sourceEntityName, sourceId);
            service.Update(target);
        }

        /// <summary>
        /// 实体间建立数据1:N/N:N关系
        /// </summary>
        /// <param name="service">          </param>
        /// <param name="relationshipName"> </param>
        /// <param name="targetEntityName"> </param>
        /// <param name="tartgetEntityId">  </param>
        /// <param name="relatedEntityName"></param>
        /// <param name="relatedEntityIds"> </param>
        public static void Associate(this IOrganizationService service, string relationshipName, string targetEntityName, Guid tartgetEntityId, string relatedEntityName, IList<Guid> relatedEntityIds)
        {
            var relatedEntities = new EntityReferenceCollection();
            foreach (var relatedId in relatedEntityIds)
            {
                relatedEntities.Add(new EntityReference(relatedEntityName, relatedId));
            }
            var relationship = new Relationship(relationshipName);
            service.Associate(targetEntityName, tartgetEntityId, relationship, relatedEntities);
        }

        /// <summary>
        /// 更改用户业务部门
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"> </param>
        /// <param name="bizId">  </param>
        public static void ChangeUserBiz(this IOrganizationService service, Guid userId, Guid bizId)
        {
            var request = new SetBusinessSystemUserRequest
            {
                BusinessId = bizId,
                UserId = userId,
                ReassignPrincipal = new EntityReference("systemuser", userId)
            };
            service.Execute(request);
        }

        /// <summary>
        /// 统计数据行
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="fetchXml"></param>
        /// <returns></returns>
        public static int Count(this IOrganizationService service, string fetchXml)
        {
            service.RetrieveMultiple(fetchXml, out int totals);
            return totals;
        }

        /// <summary>
        /// 统计数据行
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="filterXml"> </param>
        /// <returns></returns>
        public static int Count(this IOrganizationService service, string entityName, string filterXml = "")
        {
            var fetchXml = FetchXmlExt.FetchXml(entityName, FetchXmlExt.QueryColumns(entityName, $"{entityName}id")
                , filterXml, page: 1, size: 1);
            return service.Count(fetchXml);
        }

        /// <summary>
        /// 统计数据行
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="filters">   </param>
        /// <returns></returns>
        public static int Count(this IOrganizationService service, string entityName, FilterExpression filters = null)
        {
            var query = new QueryExpression
            {
                Distinct = false,
                EntityName = entityName
            };
            query.ColumnSet.AddColumn($@"{entityName}id");
            query.PageInfo = new PagingInfo
            {
                Count = 1,
                PageNumber = 1,
                ReturnTotalRecordCount = true,
                PagingCookie = null
            };
            if (null != filters) query.Criteria.AddFilter(filters);
            var result = service.RetrieveMultiple(query);
            if (null != result)
            {
                return result.TotalRecordCount;
            }
            return 0;
        }

        /// <summary>
        /// 根据id查询实体指定字段
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="id">        </param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static Entity Get(this IOrganizationService service, string entityName, Guid id, string columns)
        {
            return service.Get(entityName, id, columns.Split(new[] { ',' }));
        }

        /// <summary>
        /// 根据id查询实体指定字段
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="id">        </param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static Entity Get(this IOrganizationService service, string entityName, Guid id, IList<string> columns = null)
        {
            return service.Get(entityName, new[] { new ConditionItem($@"{entityName}id", id) }, columns);
        }

        /// <summary>
        /// 根据字段名及值查询实体指定字段
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="fieldName"> </param>
        /// <param name="filedValue"></param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static Entity Get(this IOrganizationService service, string entityName, string fieldName, object filedValue, IList<string> columns = null)
        {
            return service.Get(entityName, new[] { new ConditionItem(fieldName, filedValue) }, columns);
        }

        /// <summary>
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="conditions"></param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static Entity Get(this IOrganizationService service, string entityName, IList<ConditionItem> conditions, IList<string> columns = null)
        {
            return service.List(entityName, conditions, columns)?.FirstOrDefault();
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="cacheSrv"> </param>
        /// <param name="cacheAuth"></param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(string cacheSrv, string cacheAuth)
        {
            return GetProxy(cacheSrv, cacheAuth, Guid.Empty);
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="cacheSrv"> </param>
        /// <param name="cacheAuth"></param>
        /// <param name="caller">   </param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(string cacheSrv, string cacheAuth, Guid caller)
        {
            ObjectCache cache = MemoryCache.Default;
            return GetProxy(((IServiceManagement<IOrganizationService>)cache[cacheSrv]),
                            ((AuthenticationCredentials)cache[cacheAuth]).ClientCredentials,
                            caller);
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="orgSrvUri"></param>
        /// <param name="username"> </param>
        /// <param name="passowd">  </param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(string orgSrvUri, string username, string passowd)
        {
            return GetProxy(orgSrvUri, username, passowd, Guid.Empty);
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="orgSrvUri"></param>
        /// <param name="username"> </param>
        /// <param name="passowd">  </param>
        /// <param name="caller">   </param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(string orgSrvUri, string username, string passowd, Guid caller)
        {
            var orgSrv = ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(orgSrvUri));
            var orgAuth = new AuthenticationCredentials();
            orgAuth.ClientCredentials.UserName.UserName = username;
            orgAuth.ClientCredentials.UserName.Password = passowd;
            return GetProxy(orgSrv, orgAuth.ClientCredentials, caller);
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="crmSrv"> </param>
        /// <param name="crmAuth"></param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(IServiceManagement<IOrganizationService> crmSrv, ClientCredentials crmAuth)
        {
            return GetProxy(crmSrv, crmAuth, Guid.Empty);
        }

        /// <summary>
        /// 获取OrganizationService
        /// </summary>
        /// <param name="crmSrv"> </param>
        /// <param name="crmAuth"></param>
        /// <param name="caller"> </param>
        /// <returns></returns>
        public static OrganizationServiceProxy GetProxy(IServiceManagement<IOrganizationService> crmSrv, ClientCredentials crmAuth, Guid caller)
        {
            var orgSvc = new OrganizationServiceProxy(crmSrv, crmAuth);
            if (caller != Guid.Empty)
            {
                orgSvc.CallerId = caller;
            }
            // 单个操作10分钟过期；
            orgSvc.Timeout = new TimeSpan(0, 10, 0);
            return orgSvc;
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="conditions"></param>
        /// <param name="columns">   </param>
        /// `
        /// <returns></returns>
        public static IList<Entity> List(this IOrganizationService service, string entityName, IList<ConditionItem> conditions, IList<string> columns = null)
        {
            var fetchXml = FetchXmlExt.FetchXml(entityName, columns, conditions);
            return service.RetrieveMultiple(fetchXml);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="page">      </param>
        /// <param name="size">      </param>
        /// <param name="totals">    </param>
        /// <param name="conditions"></param>
        /// <param name="columns">   </param>
        /// <param name="orders">    </param>
        /// <returns></returns>
        public static IList<Entity> List(this IOrganizationService service, string entityName, int page, int size, out int totals, IList<ConditionItem> conditions = null, IList<string> columns = null, IList<OrderItem> orders = null)
        {
            var fetchXml = FetchXmlExt.FetchXml(entityName, columns, conditions, orders, page, size);
            return service.RetrieveMultiple(fetchXml, out totals);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="conditions"></param>
        /// <param name="columns">   </param>
        /// <returns></returns>
        public static IList<Entity> ListAll(this IOrganizationService service, string entityName, IList<ConditionItem> conditions, IList<string> columns = null)
        {
            var fetchXml = FetchXmlExt.FetchXml(entityName, columns, conditions);
            return service.RetrieveAll(fetchXml);
        }

        /// <summary>
        /// 数据合并
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static Entity Merge(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            if (entities.Count() < 2)
            {
                throw new ArgumentException("entities needs 2 entity at least;");
            }
            entities = entities.OrderByDescending(x => (DateTime)x.Attributes["modifiedon"]);
            var arr = entities.ToArray();
            var main = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                service.Merge(main, arr[i]);
            }
            return main;
        }

        ///// <summary>
        ///// </summary>
        ///// <param name="service">         </param>
        ///// <param name="targetEntityName"></param>
        ///// <param name="targetId">        </param>
        //public static void AssignShopOwner(this IOrganizationService service, string targetEntityName, Guid targetId)
        //{
        //    var domain = System.Configuration.ConfigurationManager.AppSettings["shopDomainName"];
        //    var query = new QueryExpression
        //    {
        //        Distinct = false,
        //        EntityName = "systemuser",
        //        ColumnSet = new ColumnSet("systemuserid"),
        //        Criteria =
        //        {
        //            Filters =
        //            {
        //                new FilterExpression
        //                {
        //                    FilterOperator=LogicalOperator.And,
        //                    Conditions =
        //                    {
        //                        new ConditionExpression("domainname",ConditionOperator.Equal,domain)
        //                    }
        //                }
        //            }
        //        }
        //    };
        //    var collection = service.RetrieveMultiple(query);
        //    if (null != collection)
        //    {
        //        if (collection.Entities.Count > 0)
        //        {
        //            var entity = collection.Entities[0];
        //            Assign(service, entity.LogicalName, entity.Id, targetEntityName, targetId);
        //        }
        //    }
        //}

        /// <summary>
        /// 数据合并
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityMain"></param>
        /// <param name="entitySub"> </param>
        /// <returns></returns>
        public static Entity Merge(this IOrganizationService service, Entity entityMain, Entity entitySub)
        {
            var target = new EntityReference
            {
                Id = entityMain.Id,
                LogicalName = entityMain.LogicalName
            };
            var merge = new MergeRequest
            {
                SubordinateId = entitySub.Id,
                Target = target,
                PerformParentingChecks = false,
                UpdateContent = entitySub
            };
            service.Execute(merge);
            return entityMain;
        }

        /// <summary>
        /// </summary>
        /// <param name="service">   </param>
        /// <param name="entityName"></param>
        /// <param name="entityId">  </param>
        /// <param name="stateCode"> </param>
        /// <param name="statusCode"></param>
        public static void ModifyStatus(this IOrganizationService service, string entityName, Guid entityId, int stateCode, int statusCode)
        {
            var req = new SetStateRequest
            {
                EntityMoniker = new EntityReference(entityName, entityId),
                State = new OptionSetValue(stateCode),
                Status = new OptionSetValue(statusCode)
            };
            service.Execute(req);
        }

        /// <summary>
        /// 单行数据查询
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="fetchXml"></param>
        /// <returns></returns>
        public static Entity Retrieve(this IOrganizationService service, string fetchXml)
        {
            var query = new FetchExpression(fetchXml);
            var entitys = service.RetrieveMultiple(query);
            if (entitys.Entities.Count > 0)
            {
                return entitys.Entities[0];
            }
            return null;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="fetchXml"></param>
        /// <returns></returns>
        public static IList<Entity> RetrieveAll(this IOrganizationService service, string fetchXml)
        {
            int page = 1, count = 4000;
            var doc = XDocument.Parse(fetchXml);
            var list = new List<Entity>();
            while (true)
            {
                doc.Root.SetAttributeValue("page", page.ToString());
                doc.Root.SetAttributeValue("count", count.ToString());

                var query = new FetchExpression(doc.ToString());
                var entitys = service.RetrieveMultiple(query);
                if (entitys.Entities.Count > 0)
                {
                    list.AddRange(entitys.Entities);
                }

                if (!entitys.MoreRecords) break;

                page++;
            }
            return list;
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="fetchXml"></param>
        /// <returns></returns>
        public static IList<Entity> RetrieveMultiple(this IOrganizationService service, string fetchXml)
        {
            var query = new FetchExpression(fetchXml);
            var entitys = service.RetrieveMultiple(query);
            if (entitys.Entities.Count > 0)
            {
                return entitys.Entities.ToList();
            }
            return new List<Entity>();
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="fetchXml"></param>
        /// <param name="totals">  </param>
        /// <returns></returns>
        public static IList<Entity> RetrieveMultiple(this IOrganizationService service, string fetchXml, out int totals)
        {
            var doc = XDocument.Parse(fetchXml);
            doc.Root.SetAttributeValue("returntotalrecordcount", "true");
            var query = new FetchExpression(doc.ToString());
            var entitys = service.RetrieveMultiple(query);
            totals = entitys.TotalRecordCount;
            if (entitys.Entities.Count > 0)
            {
                return entitys.Entities.ToList();
            }
            return new List<Entity>();
        }
    }
}