using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph.Interfaces;

namespace GrapheneCore.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class Graph : IGraph
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Graph (IGrapheneDatabaseContext context)
        {
            Init(context);
        }
    }
}