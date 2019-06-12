// action：
// file name：lce.engine.BaseRepository.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 12:40
// copyright (c) 2019 lynxce.com
// desc：
// > add description for BaseRepository
// revision：
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace lce.engine
{
    /// <summary>
    /// Base repository.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : IEntity
    {
        readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<int> Add(T entity)
        {
            var crtOn = typeof(T).GetProperty("CreatedOn");
            if (null != crtOn) crtOn.SetValue(entity, DateTime.Now);

            var mdfOn = typeof(T).GetProperty("ModifiedOn");
            if (null != mdfOn) mdfOn.SetValue(entity, DateTime.Now);

            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(T entity, IList<string> properties = null)
        {
            var mdfOn = typeof(T).GetProperty("ModifiedOn");
            if (null != mdfOn) mdfOn.SetValue(entity, DateTime.Now);

            if (null != properties && properties.Count > 0)
            {
                var modify = _context.Entry(entity);
                properties.ToList().ForEach(p => modify.Property(p).IsModified = true);
                modify.Property("ModifiedBy").IsModified = true;
                modify.Property("ModifiedOn").IsModified = true;
            }
            else
            {
                _context.Entry<T>(entity).State = EntityState.Modified;
                _context.Entry<T>(entity).Property("CreatedOn").IsModified = false;
                _context.Entry<T>(entity).Property("CreatedBy").IsModified = false;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateExcept(T entity, IList<string> properties = null)
        {
            var mdfOn = typeof(T).GetProperty("ModifiedOn");
            if (null != mdfOn) mdfOn.SetValue(entity, DateTime.Now);

            _context.Entry<T>(entity).State = EntityState.Modified;
            _context.Entry<T>(entity).Property("CreatedOn").IsModified = false;
            _context.Entry<T>(entity).Property("CreatedBy").IsModified = false;
            if (null != properties && properties.Count > 0)
            {
                properties.ToList().ForEach(p =>
                {
                    _context.Entry<T>(entity).Property(p).IsModified = false;
                });
            }
            return await _context.SaveChangesAsync();
        }


        public async Task<int> Save(T entity)
        {
            if (entity.Id != 0)
            {
                return await Update(entity, null);
            }
            return await Add(entity);
        }

        public async Task<int> Delete(int id)
        {
            var entity = Find(x => x.Id == id).Result;
            if (null != entity)
            {
                entity.State = 1;
                return await Update(entity, new string[] { "State" });
            }
            throw new NullReferenceException("数据不存在");
        }

        public async Task<int> Delete(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(Expression<Func<T, bool>> predicate)
        {
            var list = _context.Set<T>().Where(predicate).ToList();
            list.ForEach(entity =>
            {
                _context.Set<T>().Attach(entity);
                _context.Entry<T>(entity).State = EntityState.Deleted;
            });
            return await _context.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<IList<T>> FindList(Expression<Func<T, bool>> predicate, IList<OrderParam> orders)
        {
            var list = _context.Set<T>().AsNoTracking().Where(predicate).AsQueryable();
            if (null != orders)
            {
                foreach (var order in orders)
                {
                    list = OrderBy(list, order.Field, order.IsAsc);
                }
            }
            return await list.ToListAsync();
        }

        public async Task<IList<T>> FindList(int page, int size, Expression<Func<T, bool>> predicate, IList<OrderParam> orders)
        {
            var list = _context.Set<T>().AsNoTracking().Where(predicate).AsQueryable();
            if (null != orders)
            {
                foreach (var order in orders)
                {
                    list = OrderBy(list, order.Field, order.IsAsc);
                }
            }
            else
            {
                list = OrderBy(list);
            }
            return await list.Skip<T>((page - 1) * size).Take(size).ToListAsync();
        }

        /// <summary>
        ///  排序扩展
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        private IQueryable<T> OrderBy(IQueryable<T> source, string propertyName = "Id", bool isAsc = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(T), "不能为空");
            if (string.IsNullOrEmpty(propertyName)) return source;
            var _parameter = Expression.Parameter(source.ElementType);
            var _property = Expression.Property(_parameter, propertyName);
            if (_property == null) throw new ArgumentNullException(propertyName, "属性不存在");
            var _lambda = Expression.Lambda(_property, _parameter);
            var _methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var _resultExpression = Expression.Call(typeof(Queryable), _methodName, new Type[] { source.ElementType, _property.Type }, source.Expression, Expression.Quote(_lambda));
            return source.Provider.CreateQuery<T>(_resultExpression);
        }
    }
}
