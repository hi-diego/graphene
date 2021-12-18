using GrapheneCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Author : Authenticable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public override string Identifier { get => Email; set { Email = value; } }
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
