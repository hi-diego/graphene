using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
