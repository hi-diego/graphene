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
                .Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 2
                    && mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" 
                        && pi.ParameterType != typeof(string)
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
                var q = query;
                Type relationType = typeof(IEnumerable<GrapheneCore.Models.Interfaces.IModel>);
                var methodInfo = typeof(DynamicExpressionParser)
                    .GetTypeInfo()
                    .GetDeclaredMethods("ParseLambda")
                    .Single((MethodInfo mi) =>
                        mi.GetParameters().Count() == 4 &&
                        mi.GetGenericArguments().Count() == 2 &&
                        mi.GetParameters().Any((ParameterInfo pi) =>
                            pi.Name == "parsingConfig" &&
                            pi.ParameterType == typeof(ParsingConfig)
                        )
                    );
                var expression = methodInfo.MakeGenericMethod(modelType, relationType).Invoke(null, new object[] { new ParsingConfig() { }, true, include, new object[] { } });
                var includeQuery = IncludeMethodInfo.MakeGenericMethod(modelType, relationType).Invoke(null, new object[] { query, expression });
                q = query;
            }
            return query;
        }
    }
}
