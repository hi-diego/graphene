using GrapheneCore.Extensions;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Http;
using GrapheneCore.Entities;
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
        /// <param name="query"></param>
        /// <param name="load"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, string[] load, Type? entityType = null)
        {
            if (load == null) return query;
            foreach (string include in load) query = query.Include(include);
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="load"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, string[] includes, IGraph graph, Type entityType)
        {
            return graph.SetIncludes(query, entityType, includes);
        }
    }
}
