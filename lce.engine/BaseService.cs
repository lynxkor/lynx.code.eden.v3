// action：
// file name：lce.engine.BaseService.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 12:54
// copyright (c) 2019 lynxce.com
// desc：
// > add description for BaseService
// revision：
//
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lce.engine
{
    /// <summary>
    /// base service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IBaseService<T> where T : IEntity
    {
        private readonly IBaseRepository<T> _repository;

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(IBaseRepository<T> repository) => _repository = repository;

        /// <summary>
        /// count entity records
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="predicate">Expression.</param>
        public async Task<int> Count(Expression<Func<T, bool>> predicate = null) => await _repository.Count(predicate);

        /// <summary>
        /// add entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add(T entity) => await _repository.Add(entity);

        /// <summary>
        /// update entity./update entity's properties
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">null则更新所有字段，NOT NULL则更新指定字段</param>
        /// <returns></returns>
        public async Task<int> Update(T entity, IList<string> properties = null) => await _repository.Update(entity, properties);

        /// <summary>
        /// update entity except properties list
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">NULL则更新时不排除字段，NOT NULL则更新指定字段以外的</param>
        /// <returns></returns>
        public async Task<int> UpdateExcept(T entity, IList<string> properties = null) => await _repository.UpdateExcept(entity, properties);

        /// <summary>
        /// 保存实体，如果存在刚更新
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="entity">Entity.</param>
        public async Task<int> Save(T entity) => await _repository.Save(entity);

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="disable"></param>
        public async Task<int> State(int id, bool disable = true) => await _repository.State(id, disable);

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="disable"></param>
        /// <returns></returns>
        public async Task<int> State(T entity, bool disable = true) => await _repository.State(entity, disable);

        /// <summary>
        /// delete entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id) => await _repository.Delete(id);

        /// <summary>
        /// delete entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Delete(T entity) => await _repository.Delete(entity);

        /// <summary>
        /// delete batch.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> Delete(Expression<Func<T, bool>> predicate) => await _repository.Delete(predicate);

        /// <summary>
        /// find.
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public async Task<T> Find(Expression<Func<T, bool>> predicate) => await _repository.Find(predicate);

        /// <summary>
        /// list all.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public async Task<IList<T>> List(Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders)
            => await _repository.List(predicate, orders);

        /// <summary>
        /// page list.
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">页阀</param>
        /// <param name="predicate">条件</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public async Task<IList<T>> List(int page, int size, Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders)
            => await _repository.List(page, size, predicate, orders);
    }
}
