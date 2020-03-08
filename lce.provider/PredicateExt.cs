// action：lambda PredicateExt
// file name：lce.provider.PredicateExt.cs
// author：lynx lynx.kor@163.com @ 2019/6/5 23:01
// copyright (c) 2019 lynxce.com
// > add description for PredicateExt
// desc：what can you use this.
// var where = PredicateExt.True<model>();
// or where = PredicateExt.False<model>();
// where = where.And(x=>x.feild1 == value1);
// where = where.Or(x=>x.feild2 == value2);
// revision：
//
using System;
using System.Linq;
using System.Linq.Expressions;

namespace lce.provider
{
    /// <summary>
    /// lambda包装器
    /// </summary>
    public static class PredicateExt
    {
        /// <summary>
        /// True
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// False
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Left And Right
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invoked = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, invoked), left.Parameters);
        }

        /// <summary>
        /// Left Or Right
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invoked = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, invoked), left.Parameters);
        }

    }
}
