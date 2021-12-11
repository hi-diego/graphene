using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GrapheneCore.Graph.Interfaces
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
        /// Returns the Type of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public Type GetRelationType(Type root, string path);
        /// <summary>
        /// Returns the Type of the relation that corresponds to the given path in the given type
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
        /// <param name="set"></param>
        /// <param name="modelType"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetIncludes(IQueryable<dynamic> set, Type modelType, string[] load);
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
    }
}
