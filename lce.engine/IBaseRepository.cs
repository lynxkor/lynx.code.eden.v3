/* file name： IBaseRepository.cs
 * author：lynx <lynx.kor@163.com> @ 2019/6/5 11:09
 * copyright (c) 2019 lynxce.com
 * desc：
 * > add description for IBaseRepository
 * revision：
 *
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lce.engine
{
    /// <summary>
    /// Base repository interface.
    /// </summary>
    public interface IBaseRepository<T> where T : IEntity
    {
        /// <summary>
        /// count entity records
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="predicate">Expression.</param>
        Task<int> Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// add entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Add(T entity);

        /// <summary>
        /// add entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> AddRange(IEnumerable<T> entities);

        /// <summary>
        /// update entity./update entity's properties
        /// </summary>
        /// <param name="entity">    </param>
        /// <param name="properties">null则更新所有字段，NOT NULL则更新指定字段</param>
        /// <returns></returns>
        Task<int> Update(T entity, IList<string> properties = null);

        /// <summary>
        /// update entity except properties list
        /// </summary>
        /// <param name="entity">    </param>
        /// <param name="properties">NULL则更新时不排除字段，NOT NULL则更新指定字段以外的</param>
        /// <returns></returns>
        Task<int> UpdateExcept(T entity, IList<string> properties = null);

        /// <summary>
        /// 保存，根据entity.id=0判断为新增，否则为更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Save(T entity);

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">     Identifier.</param>
        /// <param name="disable"></param>
        Task<int> State(int id, bool disable = true);

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="disable"></param>
        /// <returns></returns>
        Task<int> State(T entity, bool disable = true);

        /// <summary>
        /// delete entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Delete(int id);

        /// <summary>
        /// delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Delete(T entity);

        /// <summary>
        /// delete batch.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// find.
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        Task<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// list all.
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="orders">   排序字段</param>
        /// <returns></returns>
        Task<IList<T>> List(Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders = null);

        /// <summary>
        /// page list.
        /// </summary>
        /// <param name="page">     页码</param>
        /// <param name="size">     页阀</param>
        /// <param name="predicate">条件</param>
        /// <param name="orders">   排序字段</param>
        /// <returns></returns>
        Task<IList<T>> List(int page, int size, Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders = null);
    }
}
