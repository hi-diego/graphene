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
            foreach (string include in load) query = query.Include(include);
            return query;
        }
    }
}
