using GrapheneCore.Entities;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Blog : Entity
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
