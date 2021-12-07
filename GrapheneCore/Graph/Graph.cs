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
    }
}