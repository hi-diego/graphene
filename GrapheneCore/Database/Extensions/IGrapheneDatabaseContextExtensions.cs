using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Database.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GrapheneDatabaseContextExtensions
    {
        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static bool Exists(this IGrapheneDatabaseContext dbContext, ref string entityName)
        {
            entityName = entityName.DbSetName();
            return dbContext.ModelDictionary.ContainsKey(entityName);
        }

        /// <summary>
        /// Get a Resource set by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<T> GetSet<T>(this IGrapheneDatabaseContext dbContext, string name)
            => (IQueryable<T>) dbContext.GetType().GetProperty(name.DbSetName()).GetValue(dbContext);

        /// <summary>
        /// Get a Dynamic Resource set by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<dynamic> GetSet(this IGrapheneDatabaseContext dbContext, string name)
            => (IQueryable<dynamic>) dbContext.GetType().GetProperty(name.DbSetName()).GetValue(dbContext);

    }
}
