/* file name：lce.engine.BaseRepository.cs
* author：lynx lynx.kor@163.com @ 2019/6/5 12:40
* copyright (c) 2019 lynxce.com
* desc：
* > add description for BaseRepository
* revision：
*
*/

using lce.provider;
using lce.provider.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lce.engine
{
    /// <summary>
    /// Base repository.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IUser _caller;

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="caller"> </param>
        public BaseRepository(DbContext context, IUser caller)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _caller = caller;
        }

        /// <summary>
        /// count entity records.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> Count(Expression<Func<T, bool>> predicate = null) => await _dbSet.CountAsync(predicate);

        /// <summary>
        /// add entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add(T entity)
        {
            entity.CreatedBy = _caller.Id;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedBy = _caller.Id;
            entity.ModifiedOn = DateTime.Now;
            entity.OwnerId = _caller.Id;
            entity.OwnerOrganId = _caller.OrganId;
            if (string.IsNullOrEmpty(entity.Code)) entity.Code = Cryptology.Code();
            if (string.IsNullOrEmpty(entity.Name)) entity.Name = DateTime.Now.ToCode();

            _dbSet.Add(entity);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// add range entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Add(entity);
            }
            return 1;
        }

        /// <summary>
        /// update entity./update entity's properties
        /// </summary>
        /// <param name="entity">    </param>
        /// <param name="properties">null则更新所有字段，NOT NULL则更新指定字段</param>
        /// <returns></returns>
        public async Task<int> Update(T entity, IList<string> properties = null)
        {
            entity.ModifiedBy = _caller.Id;
            entity.ModifiedOn = DateTime.Now;

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

        /// <summary>
        /// update entity except properties list
        /// </summary>
        /// <param name="entity">    </param>
        /// <param name="properties">NULL则更新时不排除字段，NOT NULL则更新指定字段以外的</param>
        /// <returns></returns>
        public async Task<int> UpdateExcept(T entity, IList<string> properties = null)
        {
            entity.ModifiedBy = _caller.Id;
            entity.ModifiedOn = DateTime.Now;

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

        /// <summary>
        /// 保存，根据entity.id=0判断为新增，否则为更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Save(T entity)
        {
            if (entity.Id != 0)
            {
                return await Update(entity, null);
            }
            return await Add(entity);
        }

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">     Identifier.</param>
        /// <param name="disable"></param>
        public async Task<int> State(int id, bool disable = true)
        {
            var entity = Find(x => x.Id == id).Result;
            if (null != entity)
            {
                entity.State = disable ? 1 : 0;
                return await Update(entity, new string[] { "State" });
            }
            throw new NullReferenceException("数据不存在");
        }

        /// <summary>
        /// 禁用数据,表中必须有State字段
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="disable"></param>
        /// <returns></returns>
        public async Task<int> State(T entity, bool disable = true)
        {
            entity.State = disable ? 1 : 0;
            return await Update(entity, new string[] { "State" });
        }

        /// <summary>
        /// delete entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(int id)
        {
            var entity = Find(x => x.Id == id).Result;
            if (null != entity)
            {
                return await Delete(entity);
            }
            throw new NullReferenceException("数据不存在");
        }

        /// <summary>
        /// delete entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Delete(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// delete batch.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> Delete(Expression<Func<T, bool>> predicate)
        {
            var list = _dbSet.Where(predicate).ToList();
            list.ForEach(entity =>
            {
                _dbSet.Attach(entity);
                _context.Entry<T>(entity).State = EntityState.Deleted;
            });
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// find.
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// list all.
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="orders">   排序字段</param>
        /// <returns></returns>
        public async Task<IList<T>> List(Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders)
        {
            var list = _dbSet.AsNoTracking().Where(predicate).AsQueryable();
            if (null != orders)
            {
                foreach (var order in orders)
                {
                    list = OrderBy(list, order.Key, order.Value);
                }
            }
            return await list.ToListAsync();
        }

        /// <summary>
        /// page list.
        /// </summary>
        /// <param name="page">     页码</param>
        /// <param name="size">     页阀</param>
        /// <param name="predicate">条件</param>
        /// <param name="orders">   排序字段</param>
        /// <returns></returns>
        public async Task<IList<T>> List(int page, int size, Expression<Func<T, bool>> predicate, Dictionary<string, bool> orders)
        {
            var list = _dbSet.AsNoTracking().Where(predicate).AsQueryable();
            if (null != orders)
            {
                foreach (var order in orders)
                {
                    list = OrderBy(list, order.Key, order.Value);
                }
            }
            else
            {
                list = OrderBy(list);
            }
            return await list.Skip<T>((page - 1) * size).Take(size).ToListAsync();
        }

        /// <summary>
        /// 排序扩展
        /// </summary>
        /// <param name="source">      </param>
        /// <param name="propertyName"></param>
        /// <param name="isAsc">       </param>
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