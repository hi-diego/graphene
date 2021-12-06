using GrapheneCore.Extensions;
using GrapheneCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns></returns>
        public static IQueryable<dynamic> Includes(this IQueryable<dynamic> query, string[] load)
        {
            if (load == null) return query;
            foreach (string include in load) query = query.Include(include.UcFirst());
            return query;
        }
    }
}
