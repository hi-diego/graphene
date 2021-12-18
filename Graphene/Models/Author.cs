using GrapheneCore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [NotMapped]
        public string Email { get => Identifier; }
        /// <summary>
        /// 
        /// </summary>
        [Column("email")]
        public override string Identifier { get; set; }
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
