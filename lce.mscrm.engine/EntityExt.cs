/* file name：lce.mscrm.engine.EntityExt.cs
* author：lynx lynx.kor@163.com @ 2020/03/18 09:52:36
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for EntityExt
* revision：
*
*/

using System;
using System.Linq;
using System.Reflection;
using lce.mscrm.engine.Attributes;
using lce.provider;
using Microsoft.Xrm.Sdk;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：EntityExt
    /// </summary>
    public static class EntityExt
    {
        /// <summary>
        /// Model To Dynamics Entity
        /// <para>Class need flag EntityNameAttribute.</para>
        /// <para>Model's property need flag EntityColumnAttribute.</para>
        /// <para>Property value of null will ignore.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Entity ToEntity<T>(this T model) where T : class
        {
            if (null == model) return null;
            var type = model.GetType();
            var entityName = (EntityNameAttribute)Attribute.GetCustomAttributes(type, typeof(EntityNameAttribute), true).FirstOrDefault();
            if (null == entityName) return null;

            var entity = new Entity($"{entityName.Prefix}{entityName.Name}");

            var fields = type.GetProperties();
            foreach (var f in fields)
            {
                var column = f.GetCustomAttribute<EntityColumnAttribute>();
                if (null != column)
                {
                    var value = f.GetValue(model, null);
                    if (null != value && !string.IsNullOrEmpty(value.ToString()))
                    {
                        if (column.Name == "id")
                        {
                            entity.Id = Guid.Parse(value.ToString());
                        }
                        else
                        {
                            switch (column.DataType)
                            {
                                case EntityDataType.Guid:
                                case EntityDataType.Int:
                                case EntityDataType.String:
                                case EntityDataType.Decimal:
                                case EntityDataType.Double:
                                    entity[column.Name] = value;
                                    break;

                                case EntityDataType.Bool:
                                    var tp = value.GetType();
                                    if (tp.Equals(typeof(int)))
                                        entity[column.Name] = int.Parse(value.ToString()) != 0;
                                    else
                                        entity[column.Name] = value;
                                    break;

                                case EntityDataType.DateTime:
                                    entity[column.Name] = value.ToUTC();
                                    break;

                                case EntityDataType.OptionSetValue:
                                    entity[column.Name] = new OptionSetValue(value.ToInt32());
                                    break;

                                case EntityDataType.EntityReference:
                                    if (string.IsNullOrEmpty(value.ToString())) break;
                                    entity[column.Name] = new EntityReference(column.LookUp, Guid.Parse(value.ToString()));
                                    break;

                                case EntityDataType.Money:
                                    entity[column.Name] = new Money(decimal.Parse(value.ToString()));
                                    break;
                            }
                        }
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// Dynamics Entity To Model
        /// <para>Model's property need flag EntityColumnAttribute.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T ToModel<T>(this Entity entity) where T : class
        {
            var result = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                var column = p.GetCustomAttribute<EntityColumnAttribute>();
                if (null == column) continue;
                if (column.Name == "id")
                {
                    p.SetValue(result, entity.Id.ToString());
                    continue;
                }
                if (entity.Contains(column.Name))
                {
                    if (column.IsAlias)
                    {
                        var value = ((AliasedValue)entity[column.Name]).Value;
                        switch (column.DataType)
                        {
                            case EntityDataType.Guid:
                            case EntityDataType.Int:
                            case EntityDataType.String:
                            case EntityDataType.Decimal:
                            case EntityDataType.Double:
                            case EntityDataType.Bool:
                                p.SetValue(result, value);
                                break;

                            case EntityDataType.DateTime:
                                p.SetValue(result, ((DateTime)value).ToLocalTime());
                                break;

                            case EntityDataType.OptionSetValue:
                                p.SetValue(result, ((OptionSetValue)value).Value);
                                break;

                            case EntityDataType.EntityReference:
                                if (!column.IsLookUpName)
                                    p.SetValue(result, ((EntityReference)value).Id);
                                else
                                    p.SetValue(result, ((EntityReference)value).Name);
                                break;

                            case EntityDataType.Money:
                                p.SetValue(result, ((Money)value).Value);
                                break;
                        }
                    }
                    else
                    {
                        switch (column.DataType)
                        {
                            case EntityDataType.Guid:
                            case EntityDataType.Int:
                            case EntityDataType.String:
                            case EntityDataType.Decimal:
                            case EntityDataType.Double:
                            case EntityDataType.Bool:
                                p.SetValue(result, entity[column.Name]);
                                break;

                            case EntityDataType.DateTime:
                                p.SetValue(result, entity.GetAttributeValue<DateTime>(column.Name).ToLocalTime());
                                break;

                            case EntityDataType.OptionSetValue:
                                p.SetValue(result, entity.GetAttributeValue<OptionSetValue>(column.Name).Value);
                                break;

                            case EntityDataType.EntityReference:
                                if (!column.IsLookUpName)
                                    p.SetValue(result, entity.GetAttributeValue<EntityReference>(column.Name).Id);
                                else
                                    p.SetValue(result, entity.GetAttributeValue<EntityReference>(column.Name).Name);
                                break;

                            case EntityDataType.Money:
                                p.SetValue(result, entity.GetAttributeValue<Money>(column.Name).Value);
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}