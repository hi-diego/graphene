using GrapheneCore.Models;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Blog : Model
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }
    }
}
