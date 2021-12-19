using Graphene.Entities;

namespace GrapheneTemplate.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Post : Entity
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
        /// <summary>
        /// 
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Author? Author { get; set; }
    }
}
