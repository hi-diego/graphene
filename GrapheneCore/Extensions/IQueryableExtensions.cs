using GrapheneCore.Extensions;
using GrapheneCore.Http;
using GrapheneCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Graphene.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo IncludeMethodInfo = 
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("Include")
                .Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 2 && mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));
        /// <summary>
        /// 
        /// </summary>
        public static MethodInfo StringIncludeMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("Include")
                .Single(
                    (MethodInfo mi) => mi.GetGenericArguments().Count() == 2
                                    && mi.GetParameters().Any(
                                        (ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)
                                    )
                    );
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, Pagination pagination)
            => query.Includes(pagination.Load);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="load"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, string[] load, Type? modelType = null)
        {
            if (load == null) return query;
            foreach (string include in load)
            {
                var e = DynamicExpressionParser.ParseLambda(typeof(object), null, include);
                //ParameterExpression arg = Expression.Parameter(typeof(object), "b");

                var parameter = Expression.Parameter(modelType, "m");

                // var comparison = Expression.GreaterThan (Expression.Property(parameter, Type.GetType("ConsoleApp6.Album").GetProperty("Quantity")), Expression.Constant(100));

                Expression<Func<dynamic, dynamic>> exp = Expression.Lambda<Func<dynamic, dynamic>>(e, parameter);
                // Expression<Func<dynamic, dynamic>> exp = new Expression();
                //ParameterExpression arg = Expression.Parameter(modelType, "b");

                //
                // Expression<Func<object, dynamic>> expp = x => (x as Model).Posts.Take(10);
                query = query.Include(exp);
                // query = query.Include(include);
            }
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, string[] load, Type? modelType = null)
        //{
        //    if (load == null) return query;
        //    foreach (string include in load)
        //    {
        //        //Type type = typeof(Model);
        //        //var e = DynamicExpressionParser.ParseLambda(
        //        //    modelType, null,
        //        //    include
        //        //);
        //        //ParameterExpression arg = Expression.Parameter(typeof(object), "b");
        //        //query = query.Include(Expression.Lambda<Func<dynamic, dynamic>>(e, arg));
        //        //query = query.Provider.CreateQuery<dynamic>(Expression.Call(null, StringIncludeMethodInfo.MakeGenericMethod(typeof(object), typeof(object)), query.Expression, Expression.Constant(include)));

        //        Type type = modelType;
        //        ParameterExpression arg = Expression.Parameter(typeof(object), "x");
        //        Expression expr = null;
        //        PropertyInfo propertyInfo = type.GetProperty(include);
        //        expr = Expression.Property(arg, propertyInfo);
        //        if (propertyInfo.PropertyType.IsValueType) expr = Expression.Convert(expr, typeof(object));
        //        var expression = Expression.Lambda<Func<dynamic, dynamic>>(expr, arg);

        //        query = query.Include(include);


        //        return query;
        //}
        //public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source, [NotParameterized] string navigationPropertyPath) where TEntity : class
        //{
        //    return source.Provider.CreateQuery<TEntity>(Expression.Call(null, StringIncludeMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression, Expression.Constant(navigationPropertyPath)));
        //}
        //public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        //{
        //    return new IncludableQueryable<TEntity, TProperty>( (IQueryable<TEntity>) ((IQueryable<object>) source.Provider.CreateQuery<TEntity>(Expression.Call(null, IncludeMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)), new Expression[2]
        //    {
        //        source.Expression,
        //        Expression.Quote(navigationPropertyPath)
        //    }))));
        //}
    }
    //public sealed class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>, IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IAsyncEnumerable<TEntity>
    //{
    //    private readonly IQueryable<TEntity> _queryable;

    //    public Expression Expression => _queryable.Expression;

    //    public Type ElementType => _queryable.ElementType;

    //    public IQueryProvider Provider => _queryable.Provider;

    //    public IncludableQueryable(IQueryable<TEntity> queryable)
    //    {
    //        _queryable = queryable;
    //    }

    //    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        return ((IAsyncEnumerable<TEntity>)_queryable).GetAsyncEnumerator(cancellationToken);
    //    }

    //    public IEnumerator<TEntity> GetEnumerator()
    //    {
    //        return _queryable.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}
