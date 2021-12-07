using GrapheneCore.Models;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Post : Model
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BlogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Blog? Blog { get; set; }
    }
}
