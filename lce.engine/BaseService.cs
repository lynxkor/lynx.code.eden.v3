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
    public class BaseService<T> : IBaseService<T> where T : IEntity
    {
        readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            return await _repository.Count(predicate);
        }

        public async Task<int> Add(T entity)
        {
            return await _repository.Add(entity);
        }

        public async Task<int> Delete(T entity)
        {
            return await _repository.Delete(entity);
        }

        public async Task<int> Delete(Expression<Func<T, bool>> predicate)
        {
            return await _repository.Delete(predicate);
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _repository.Find(predicate);
        }

        public async Task<IList<T>> FindList(Expression<Func<T, bool>> predicate, IList<OrderParam> orders)
        {
            return await _repository.FindList(predicate, orders);
        }

        public async Task<IList<T>> FindList(int page, int size, Expression<Func<T, bool>> predicate, IList<OrderParam> orders)
        {
            return await _repository.FindList(page, size, predicate, orders);
        }

        public async Task<int> Update(T entity, IList<string> properties)
        {
            return await _repository.Update(entity, properties);
        }

        public async Task<int> UpdateExcept(T entity, IList<string> properties)
        {
            return await _repository.UpdateExcept(entity, properties);
        }
    }
}
