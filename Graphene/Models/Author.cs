using GrapheneCore.Models;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Author : Model
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }
    }
}
