using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Models;

namespace GrapheneCore.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class Graph : IGraph
    {
        /// <summary>
        /// All the types including the fake ones;
        /// </summary>
        public IEnumerable<GraphType> Types { get; set; } = new List<GraphType>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Graph(IGrapheneDatabaseContext context)
        {
            Init(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(IGrapheneDatabaseContext databaseContext)
        {
            Types = GetGraph(databaseContext);
        }

        /// <summary>
        /// Converts every Model from the project namespace (classes that implements IEntity)
        /// to IEntityDescriptor and concatenates the Custom Entities descriptors from the database
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GraphType> GetGraph(IGrapheneDatabaseContext context)
            => context.ModelDictionary.ToList()
                    .Where(m => !m.Value.IsAbstract && m.Value.IsSubclassOf(typeof(Model)))
                    .Select(m => new GraphType(m.Value)) // until Rules are implemented, context.Rule.Where(r => r.Entity == m.Key).ToList()))
                    .ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GraphType? Find(string name)
        {
            return Types.FirstOrDefault(t => t.PascalName == name.DbSetName());
        }

        /// <summary>
        /// Returns the Type of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public Type GetRelationType(Type root, string path)
        {
            return GetRelationGraphType(root, path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type GetRelationGraphType(Type root, string path)
        {
            GraphType rootGraphType = Types.Single(t => t.SystemType == root);
            GraphType graphType = rootGraphType;
            GraphType prevGraphType = rootGraphType;
            foreach (var piece in path.Split("."))
            {
                prevGraphType = graphType;
                graphType = graphType.Fields.Single(f => f.PascalName == piece.UcFirst());
            }
            return graphType.SystemType;
            //return prevGraphType.Multiple
            //    ? typeof(IEnumerable<>).MakeGenericType(graphType.SystemType)
            //    : graphType.SystemType;
        }

        /// <summary>
        /// Returns the Type of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IEnumerable<IncludeExpression> GetIncludeExpressions(Type root, string[] includes)
        {
            GraphType rootGraphType = Types.Single(t => t.SystemType == root);
            List<IncludeExpression> includeExpressions = new List<IncludeExpression>(); //  = includes.Select(i => new IncludeExpression(rootGraphType, i, this));
            IncludeExpression prevIncludeExpression = null;
            foreach (string i in includes) {
                IncludeExpression includeExpression = new IncludeExpression(rootGraphType, i, this, prevIncludeExpression);
                prevIncludeExpression = includeExpression;
                includeExpressions.Add(includeExpression);
            }
            //IEnumerable<IncludeExpression> includeExpressions = includes.Select(i => new IncludeExpression(rootGraphType, i, this));
            return includeExpressions;
        }
    }
}