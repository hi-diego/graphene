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
        public List<Post> Posts { get; } = new();
    }
}
