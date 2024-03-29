﻿using Graphene.Database.Interfaces;
using Graphene.Extensions;
using Graphene.Entities.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Graphene.Graph.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// All the types including the fake ones;
        /// </summary>
        public IEnumerable<GraphType> Types { get; set; }
        /// <summary>
        /// Returns the _Entity of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public Type GetRelationType(Type root, string path);
        /// <summary>
        /// Returns the _Entity of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IEnumerable<IncludeExpression> GetIncludeExpressions(Type root, string[] includes);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="iExpression"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetInclude(IQueryable<dynamic> set, IncludeExpression iExpression);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IAuthenticable> GetIAuthenticable(IGrapheneDatabaseContext dbContext, string email, string[] includes);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="entityType"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetIncludes(IQueryable<dynamic> set, Type entityType, string[] load);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GraphType? Find(string name);
        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool Exists(IGrapheneDatabaseContext dbContext, ref string entityName);
        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool ItExists(IGrapheneDatabaseContext dbContext, string entityName);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<T> GetSet<T>(IGrapheneDatabaseContext dbContext, string name);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, string name);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, GraphType graphType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, Type graphType);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public bool Exists<T>(IGrapheneDatabaseContext dbContext);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GraphType? Find<T>();
    }
}
